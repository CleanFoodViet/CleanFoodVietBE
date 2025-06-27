using CleanFoodVietAPI.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

[ApiController]
[Route("api/v1/admin/webhook/stripe")]
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

    [HttpPost]
    public async Task<IActionResult> TestPost()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, signature, _whSecret);
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

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"];

        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json, signature, _whSecret);
        }
        catch (StripeException e)
        {
            _logger.LogWarning("Invalid webhook signature: {Msg}", e.Message);
            return BadRequest();
        }

        _logger.LogInformation("Webhook received: {Type}", stripeEvent.Type);

        switch (stripeEvent.Type)
        {
            case "checkout.session.completed":
                var session = stripeEvent.Data.Object as Session;
                if (session != null)
                {
                    _logger.LogInformation("Handling checkout.session.completed for {REF}",
                                            session.ClientReferenceId);
                    await _orderSvc.HandleCheckoutSessionCompletedAsync(session);
                }
                break;

            case "payment_intent.payment_failed":
                var intent = stripeEvent.Data.Object as PaymentIntent;
                if (intent != null)
                {
                    _logger.LogInformation("Handling payment_intent.payment_failed for {ID}",
                                            intent.Id);
                    await _orderSvc.HandlePaymentFailedAsync(intent);
                }
                break;

            default:
                _logger.LogInformation("Ignored event type {Type}", stripeEvent.Type);
                break;
        }

        return Ok();
    }
}
