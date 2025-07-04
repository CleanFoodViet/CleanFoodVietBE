using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
public class StripeWebhookController : ControllerBase
{
    private readonly string _testSecret;
    private readonly string _liveSecret;
    private readonly IServicePackageOrderService _orderSvc;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(
        IConfiguration config,
        IServicePackageOrderService orderSvc,
        ILogger<StripeWebhookController> logger)
    {
        // you must add both of these in Azure App Settings:
        // Stripe:WebhookSecretTest  = <your whsec_test_…>
        // Stripe:WebhookSecretLive  = <your whsec_live_…>
        _testSecret = config["Stripe:WebhookSecretTest"]!;
        _liveSecret = config["Stripe:WebhookSecretLive"]!;
        _orderSvc = orderSvc;
        _logger = logger;
    }

    /// <summary>
    /// Test‐only endpoint
    /// POST /api/v1/admin/webhook/stripe/test-post
    /// </summary>
    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpointTest)]
    public Task<IActionResult> TestPost()
        => HandleWebhookAsync(_testSecret, isTest: true);

    /// <summary>
    /// Health‐check
    /// GET /api/v1/admin/webhook/stripe/healthz
    /// </summary>
    [HttpGet("/api/v1/admin/webhook/stripe/healthz")]
    public IActionResult Healthz()
    {
        _logger.LogInformation("🔥 Healthz ping at {Now}", DateTime.UtcNow);
        return Ok("alive");
    }

    /// <summary>
    /// Live webhook
    /// POST /api/v1/admin/webhook/stripe
    /// </summary>
    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpoint)]
    public Task<IActionResult> Post()
        => HandleWebhookAsync(_liveSecret, isTest: false);

    private async Task<IActionResult> HandleWebhookAsync(string secret, bool isTest)
    {
        var prefix = isTest ? "▶️ Test" : "▶️ Live";
        _logger.LogInformation("{Prefix} webhook POST hit", prefix);

        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var sigHeader = Request.Headers["Stripe-Signature"].FirstOrDefault();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json: json,
                stripeSignatureHeader: sigHeader,
                secret: secret,
                throwOnApiVersionMismatch: false
            );

            _logger.LogInformation(
                "✔️  {Prefix} webhook parsed OK: {Type} [evt_{Id}]",
                prefix, stripeEvent.Type, stripeEvent.Id
            );
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(
                "⚠️  {Prefix} webhook invalid signature: {Error}",
                prefix, ex.Message
            );
            return BadRequest();
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                var session = (Session)stripeEvent.Data.Object!;
                _logger.LogInformation(
                    "➡️  Handling checkout.session.completed for session {SessionId} (order {OrderId})",
                    session.Id, session.ClientReferenceId);
                await _orderSvc.HandleCheckoutSessionCompletedAsync(session);
                break;

            case "payment_intent.payment_failed":
                var intent = (PaymentIntent)stripeEvent.Data.Object!;
                _logger.LogError(
                    "➡️  Handling payment_intent.payment_failed for intent {IntentId} (order {OrderId})",
                    intent.Id,
                    intent.Metadata.GetValueOrDefault("orderId"));
                await _orderSvc.HandlePaymentFailedAsync(intent);
                break;

            default:
                _logger.LogDebug(
                    "↩️  Ignoring unhandled event type {Type}", stripeEvent.Type);
                break;
        }

        _logger.LogInformation(
            "✅  {Prefix} webhook {Type} processed",
            prefix, stripeEvent.Type);

        return Ok();
    }
}