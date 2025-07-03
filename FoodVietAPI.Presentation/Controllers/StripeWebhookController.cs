using CleanFoodVietAPI.Application.Services.Interfaces;
using CleanFoodVietAPI.Presentation.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

[ApiController]
public class StripeWebhookController : ControllerBase
{
    private readonly string _whSecret;
    private readonly IServicePackageOrderService _orderSvc;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(
        IConfiguration config,
        IServicePackageOrderService orderSvc,
        ILogger<StripeWebhookController> logger)
    {
        _whSecret = config["Stripe:WebhookSecret"]!;
        _orderSvc = orderSvc;
        _logger = logger;
    }

    // Test endpoint (no side-effects)
    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpointTest)]
    public async Task<IActionResult> TestPost()
    {
        _logger.LogInformation("▶️ TestPost webhook hit");
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
              json: json,
              stripeSignatureHeader: signature,
              secret: _whSecret,
              throwOnApiVersionMismatch: false
            );

            _logger.LogInformation("✔️  Test webhook parsed OK: {Type} [evt_{Id}]",
                                   stripeEvent.Type, stripeEvent.Id);
            return Ok(new { Received = stripeEvent.Type });
        }
        catch (StripeException ex)
        {
            _logger.LogWarning("⚠️  Test webhook signature invalid: {Error}", ex.Message);
            return BadRequest();
        }
    }

    // Real webhook
    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpoint)]
    public async Task<IActionResult> Post()
    {
        _logger.LogInformation("▶️  Stripe webhook POST hit");
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
              json: json,
              stripeSignatureHeader: signature,
              secret: _whSecret,
              throwOnApiVersionMismatch: false
            );
            _logger.LogInformation("✔️  Valid signature; event {Type} [evt_{Id}]",
                                   stripeEvent.Type, stripeEvent.Id);
        }
        catch (StripeException ex)
        {
            _logger.LogWarning("⚠️  Invalid signature: {Error}", ex.Message);
            return BadRequest();
        }

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                var session = (Session)stripeEvent.Data.Object!;
                _logger.LogInformation("➡️  Handling checkout.session.completed for session {SessionId} (order {OrderId})",
                                       session.Id, session.ClientReferenceId);
                await _orderSvc.HandleCheckoutSessionCompletedAsync(session);
                break;

            case "payment_intent.payment_failed":
                var intent = (PaymentIntent)stripeEvent.Data.Object!;
                _logger.LogError("➡️  Handling payment_failed for intent {IntentId} (order {OrderId})",
                                 intent.Id, intent.Metadata.GetValueOrDefault("orderId"));
                await _orderSvc.HandlePaymentFailedAsync(intent);
                break;

            default:
                _logger.LogDebug("↩️  Ignoring unhandled event type {Type}", stripeEvent.Type);
                break;
        }

        _logger.LogInformation("✅  Webhook {Type} processed", stripeEvent.Type);
        return Ok();
    }
}
