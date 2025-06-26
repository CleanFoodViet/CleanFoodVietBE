using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using CleanFoodVietAPI.Application.Utils;

namespace CleanFoodVietAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/admin/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly SessionService _sessionService;
        private readonly StripeOptions _stripeOptions;
        private readonly IConfiguration _config;

        public PaymentsController(
            SessionService sessionService,
            IOptions<StripeOptions> stripeOpts,
            IConfiguration config)
        {
            _sessionService = sessionService;
            _stripeOptions = stripeOpts.Value;
            _config = config;
        }

        /// <summary>
        /// Creates a Stripe Checkout Session and returns its URL.
        /// </summary>
        [HttpPost("create-checkout-session")]
        public async Task<IActionResult> CreateCheckoutSession()
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
    }
}
