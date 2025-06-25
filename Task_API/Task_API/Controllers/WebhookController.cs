using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly PayOS _payOS;

        public WebhookController(PayOS payOS)
        {
            _payOS = payOS;
        }

        [HttpPost("receive")]
        public IActionResult ReceiveWebhook([FromBody] WebhookType webhook)
        {
            try
            {
                var verifiedData = _payOS.verifyPaymentWebhookData(webhook);
                Console.WriteLine($"Payment received: Order {verifiedData.orderCode}");

                return Ok(new { code = 0, message = "Webhook received" });
            }
            catch
            {
                return BadRequest(new { code = -1, message = "Verification failed" });
            }
        }
    }

}
