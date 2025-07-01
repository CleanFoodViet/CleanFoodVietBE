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

    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpointTest)]
    [SwaggerOperation(Summary = "Test post payment")]
    public async Task<IActionResult> TestPost()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json: json,
                stripeSignatureHeader: signature,
                secret: _whSecret,
                throwOnApiVersionMismatch: false
            );
        }
        catch (StripeException e)
        {
            _logger.LogWarning("⚠️ Invalid signature: {Error}", e.Message);
            return BadRequest();
        }

        // Always log what came in
        _logger.LogInformation("← Webhook received: {Type} [evt_{Id}]",
                               stripeEvent.Type, stripeEvent.Id);

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                _logger.LogInformation("✔️ Handling checkout.session.completed");
                return Ok("Payment succeeded");

            case "payment_intent.payment_failed":
                _logger.LogInformation("❌ Handling payment_intent.payment_failed");
                return Ok("Payment failed");

            default:
                // Just acknowledge everything else
                return Ok();
        }
    }

    [HttpPost(ApiEndpointConstant.Webhook.StripeWebhookEndpoint)]
    [SwaggerOperation(Summary = "Post Payment for Payment Return Process")]
    public async Task<IActionResult> Post()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];
        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json: json,
                stripeSignatureHeader: signature,
                secret: _whSecret,
                throwOnApiVersionMismatch: false
            );
        }
        catch (StripeException e)
        {
            _logger.LogWarning("Invalid signature: {Msg}", e.Message);
            return BadRequest();
        }

        _logger.LogInformation("Webhook received: {Type}", stripeEvent.Type);

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                var session = (Session)stripeEvent.Data.Object!;
                _logger.LogInformation("Handling session {ID}", session.Id);
                await _orderSvc.HandleCheckoutSessionCompletedAsync(session);
                break;

            case "payment_intent.payment_failed":
                var intent = (PaymentIntent)stripeEvent.Data.Object!;
                _logger.LogInformation("Handling payment failure {ID}", intent.Id);
                await _orderSvc.HandlePaymentFailedAsync(intent);
                break;
        }
        return Ok();
    }
}
