using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;

[ApiController]
[Route("api/v1/admin/webhook/stripe")]
public class StripeWebhookController : ControllerBase
{
    private readonly string _whSecret;
    private readonly ILogger<StripeWebhookController> _logger;

    public StripeWebhookController(
        IConfiguration config,
        ILogger<StripeWebhookController> logger)
    {
        _whSecret = config["Stripe:WebhookSecret"]!;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
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
}
