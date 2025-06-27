using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Application.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    public class CreateCheckoutRequest
    {
        public Ulid GardenerId { get; set; }
        public Ulid ServicePackageId { get; set; }
    }

    [ApiController]
    [Route("api/v1/admin/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly IServicePackageService _packageSvc;
        private readonly IAccountService _accountSvc;
        private readonly IServicePackageOrderService _orderSvc;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public PaymentsController(
            SessionService sessionService,
            IServicePackageService packageSvc,
            IAccountService accountSvc,
            IServicePackageOrderService orderSvc,
            IConfiguration config,
            IWebHostEnvironment env)
        {
            _sessionService = sessionService;
            _packageSvc = packageSvc;
            _accountSvc = accountSvc;
            _orderSvc = orderSvc;
            _config = config;
            _env = env;
        }

        public class CreateCheckoutRequest
        {
            public Ulid GardenerId { get; set; }
            public Ulid ServicePackageId { get; set; }
            public int Quantity { get; set; } = 1;
            public string Location { get; set; } = "VN"; // For test only, hard-coded to Vietnam
        }

        /// <summary>
        /// Creates a Stripe Checkout Session and returns its URL. TEST ONLY.
        /// </summary>
        [HttpPost("test-create-checkout-session")]
        public async Task<IActionResult> TestCreateCheckoutSession()
        {
            // In real life, pull these from your ServicePackage entity:
            const long unitAmount = 9900;  // in cents ($99.00)
            const string currency = "usd";
            const string productName = "Pro Plan (30-day)";
            // 1. For test only: hard-coded Bob email + Vietnam
            const string rawEmail = "ThisisBobEmail@gmail.com";
            const string location = "VN";

            var parts = rawEmail.Split('@');
            var email = parts.Length == 2
                ? $"{parts[0]}+location_{location}@{parts[1]}"
                : rawEmail;

            // Build the session options
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                CustomerEmail = email,  // For test purpose, this is email+location_XX
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency   = currency,
                            UnitAmount = unitAmount,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = productName
                            }
                        },
                        Quantity = 1
                    }
                },

                SuccessUrl = $"{_config["App:FrontendDomain"]}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_config["App:FrontendDomain"]}/payment-cancelled"
            };

            // Create the session
            var session = await _sessionService.CreateAsync(options);

            // Return the URL to your FE so you can window.location = session.Url
            return Ok(new { sessionUrl = session.Url });
        }

        /// <summary>
        /// Creates a Stripe Checkout Session for a Service Package Order.
        /// </summary>
        /// 
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession(
            [FromBody] CreateCheckoutRequest req)
        {
            // 1) Load Gardener & Package from your services
            var gardenerDto = await _accountSvc
                .GetAccountInformation(req.GardenerId.ToString());
            var packageDto = await _packageSvc
                .GetServicePackageDetailAsync(req.ServicePackageId);

            // 2) Create the pending order (synchronous!)
            //    This inserts Order + Payment (PENDING) and returns orderId
            var orderId = _orderSvc.CreatePendingOrder(
                gardenerDto.AccountId,
                packageDto.ServicePackageId,
                packageDto.Price
            );

            // 3) Build test email for locale (dev only)
            var emailRaw = gardenerDto.Email;
            if (_env.IsDevelopment() && !string.IsNullOrEmpty(req.Location))
            {
                var parts = emailRaw.Split('@');
                if (parts.Length == 2)
                    emailRaw = $"{parts[0]}+location_{req.Location}@{parts[1]}";
            }

            // 4) Build Stripe Checkout Session
            var sessionOptions = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                CustomerEmail = emailRaw,
                ClientReferenceId = orderId.ToString(),
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency   = "usd",
                        UnitAmount = (long)(packageDto.Price * 100m),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name        = packageDto.PackageName,
                            Description =
                               $"Subscription: {packageDto.PackageName} ({packageDto.Duration} days)"
                        }
                    },
                    // Quantity of package is always 1 in this case
                    Quantity = 1
                }
            },
                SuccessUrl =
                  $"{_config["App:FrontendDomain"]}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl =
                  $"{_config["App:FrontendDomain"]}/payment-cancelled"
            };

            var session = await _sessionService.CreateAsync(sessionOptions);

            // 5) Return Session URL + Gardener info
            return Ok(new
            {
                sessionUrl = session.Url,
                gardenerId = gardenerDto.AccountId,
                //gardenerName = gardenerDto.Name,
                gardenerEmail = gardenerDto.Email,
                gardenerPhone = gardenerDto.PhoneNumber
            });
        }
    }
}
