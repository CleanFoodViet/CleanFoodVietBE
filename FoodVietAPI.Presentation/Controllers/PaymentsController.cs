using CleanFoodVietAPI.Application.DTOs.Payment;
using CleanFoodVietAPI.Application.Exceptions;
using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly SessionService _stripeSession;
        private readonly IServicePackageService _packageSvc;
        private readonly IAccountService _accountSvc;
        private readonly IServicePackageOrderService _orderSvc;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        private readonly string _frontendDomain;

        public PaymentsController(
            SessionService stripeSession,
            IServicePackageService packageSvc,
            IAccountService accountSvc,
            IServicePackageOrderService orderSvc,
            IConfiguration config,
            IWebHostEnvironment env)
        {
            _stripeSession = stripeSession;
            _packageSvc = packageSvc;
            _accountSvc = accountSvc;
            _orderSvc = orderSvc;
            _config = config;
            _env = env;
            _frontendDomain = config["App:FrontendDomain"]!;
        }

        /// <summary>
        /// Creates a Stripe Checkout Session and returns its URL. TEST ONLY.
        /// </summary>
        [HttpPost(ApiEndpointConstant.Payment.GardenerTestPaymentsEndpoint)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Creates a Stripe Checkout Session and returns its URL. TEST ONLY.")]
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
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
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
                                Name = productName,
                                Description =
                                    $"Test subscription: {productName} (30 days)\n" +
                                    $" - Quantity: 1"
                            }
                        },
                        Quantity = 1
                    }
                },

                //SuccessUrl = $"{_config["App:FrontendDomain"]}/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                //CancelUrl = $"{_config["App:FrontendDomain"]}/payment-cancelled"
                SuccessUrl = $"{_frontendDomain}/gardener/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{_frontendDomain}/gardener/payment/fail"
            };

            // Create the session
            var session = await _stripeSession.CreateAsync(options);

            // Return the URL to your FE so you can window.location = session.Url
            return Ok(new { sessionUrl = session.Url });
        }


        /// <summary>
        /// Creates a real Checkout Session for the specified service package.
        /// Gardener’s email is collected by Stripe at checkout.
        /// </summary>
        [HttpPost(ApiEndpointConstant.Payment.GardenerPaymentsEndpoint)]
        [SwaggerOperation(Summary = "Creates a real Checkout Session for the specified service package.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CreateCheckoutSession(
            [FromBody, Required] CreateCheckoutRequest req)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 1) Parse only the packageId
            if (!Ulid.TryParse(req.ServicePackageId, out var pkgId))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid package id",
                    Detail = $"'{req.ServicePackageId}' is not a valid Ulid.",
                    Extensions = { ["errorCode"] = "InvalidPackageId" }
                });
            }

            // 2) GardenerId is already a Ulid, so take it directly
            var gardenerId = req.GardenerId;

            try
            {
                // 3) Load package detail
                var packageDto = await _packageSvc.GetServicePackageDetailAsync(req.ServicePackageId);

                // 4) Insert a PENDING order + payment
                var orderId = _orderSvc.CreatePendingOrder(
                    gardenerId,
                    packageDto.ServicePackageId,
                    packageDto.Price
                );

                // 5) Build the Stripe session options (omit CustomerEmail)
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    Mode = "payment",
                    ClientReferenceId = orderId.ToString(),
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new()
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency   = "vnd",
                                UnitAmount = (long)packageDto.Price,
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name        = packageDto.PackageName,
                                    Description =
                                        $"Subscription: {packageDto.PackageName} ({packageDto.Duration} days)\n" +
                                        $" - Quantity: {req.Quantity}"
                                }
                            },
                            Quantity = req.Quantity
                        }
                    },
                    SuccessUrl = $"{_frontendDomain}/gardener/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"{_frontendDomain}/gardener/payment/fail"
                };

                // 6) Create the Stripe session
                var session = await _stripeSession.CreateAsync(options);

                // 7) Return only session URL + gardener name & phone (no email)
                var gardenerInfo = await _accountSvc.GetAccountInformation(gardenerId.ToString());
                return Ok(new
                {
                    sessionUrl = session.Url,
                    gardenerId = gardenerInfo.AccountId,
                    gardenerName = gardenerInfo.Name,
                    gardenerPhone = gardenerInfo.PhoneNumber
                });
            }
            catch (DomainValidationException dv)
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid payment request",
                    Detail = dv.Message,
                    Extensions = { ["errorCode"] = "PaymentRequestInvalid" }
                });
            }
            catch (NotFoundException nf)
            {
                return NotFound(new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Title = "Resource not found",
                    Detail = nf.Message
                });
            }
        }
    }
}
