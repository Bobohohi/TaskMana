using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net;
using Net.payOS;
using Net.payOS.Types;
[Route("api/[controller]")]
[ApiController]
public class PayOSController : ControllerBase
{

    private readonly PayOS _payOS;

    public PayOSController(PayOS payOS)
    {
        _payOS = payOS;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentLinkRequest body)
    {
        try
        {
            int orderCode = int.Parse(DateTimeOffset.Now.ToUnixTimeSeconds().ToString());

            var item = new ItemData(body.ProductName, 1, body.Amount);
            var items = new List<ItemData> { item };

            var paymentData = new PaymentData(
                orderCode,
                body.Amount,
                body.Description,
                items,
                body.CancelUrl,
                body.ReturnUrl
            );

            var result = await _payOS.createPaymentLink(paymentData);
            return Ok(result.checkoutUrl);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPost("confirm-webhook")]
    public async Task<IActionResult> ConfirmWebhook([FromBody] string webhookUrl)
    {
        try
        {
            await _payOS.confirmWebhook(webhookUrl);
            return Ok("Webhook confirmed.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

public class CreatePaymentLinkRequest
{
    public string ProductName { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public string CancelUrl { get; set; }
    public string ReturnUrl { get; set; }
}