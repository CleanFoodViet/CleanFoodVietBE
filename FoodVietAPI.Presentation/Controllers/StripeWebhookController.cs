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
        // Ensure these two App Settings exist in Azure:
        // Stripe:WebhookSecretTest  = <your whsec_test_… from Dashboard>
        // Stripe:WebhookSecretLive  = <your whsec_live_… from Dashboard>
        _testSecret = config["Stripe:WebhookSecretTest"]!;
        _liveSecret = config["Stripe:WebhookSecretLive"]!;
        _orderSvc = orderSvc;
        _logger = logger;
    }

    /// <summary>
    /// Test‐only endpoint
    /// POST {AdminApiEndpoint}/webhook/stripe/test-post
    /// </summary>
    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpointTest)]
    [SwaggerOperation(Summary = "Test stripe payment webhook call")]
    public Task<IActionResult> TestPost()
        => HandleWebhookAsync(_testSecret, prefix: "▶️ Test");

    /// <summary>
    /// Live webhook endpoint
    /// POST {AdminApiEndpoint}/webhook/stripe
    /// </summary>
    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpoint)]
    [SwaggerOperation(Summary = "Api for stripe payment webhook call (currently used)")]
    public Task<IActionResult> Post()
        => HandleWebhookAsync(_liveSecret, prefix: "▶️ Live");

    private async Task<IActionResult> HandleWebhookAsync(string secret, string prefix)
    {
        _logger.LogInformation("{Prefix} webhook POST hit", prefix);

        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json: json,
                stripeSignatureHeader: signature,
                secret: secret,
                throwOnApiVersionMismatch: false
            );

            _logger.LogInformation(
                "✔️  {Prefix} webhook parsed OK: {Type} [evt_{Id}]",
                prefix, stripeEvent.Type, stripeEvent.Id);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning(
                "⚠️  {Prefix} webhook invalid signature: {Error}",
                prefix, ex.Message);
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

            case "checkout.session.expired":
                var expiredSession = (Session)stripeEvent.Data.Object!;
                _logger.LogWarning(
                    "➡️  Handling checkout.session.expired for session {SessionId} (order {OrderId})",
                    expiredSession.Id,
                    expiredSession.Metadata.GetValueOrDefault("orderId"));
                await _orderSvc.HandleSessionExpiredAsync(expiredSession);
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