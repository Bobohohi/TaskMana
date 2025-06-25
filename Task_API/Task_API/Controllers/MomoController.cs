using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MomoController : ControllerBase
    {
        // Thông tin tích hợp Sandbox từ MoMo (demo)
        private const string Endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
        private const string PartnerCode = "MOMO";
        private const string AccessKey = "F8BBA842ECF85";
        private const string SecretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
        private const string RedirectUrl = "https://localhost:44340/Membership/PaymentCallback"; // Địa chỉ bên ASP.NET MVC
        private const string IpnUrl = "https://webhook.site/ipn-demo";

        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] MoMoRequest request)
        {
            string orderId = DateTime.Now.Ticks.ToString(); // Sử dụng ticks cho orderId duy nhất
            string requestId = Guid.NewGuid().ToString();
            string amount = request.Amount.ToString();
            string orderInfo = "Demo MoMo Payment";
            string extraData = ""; // Có thể thêm mã hoá base64 nếu cần

            // Chuỗi cần ký HMAC SHA256
            string rawHash = $"accessKey={AccessKey}&amount={amount}&extraData={extraData}&ipnUrl={IpnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={PartnerCode}&redirectUrl={RedirectUrl}&requestId={requestId}&requestType=captureWallet";

            // Tính chữ ký (signature)
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SecretKey));
            var signatureBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(rawHash));
            string signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();

            // Dữ liệu gửi đến MoMo
            var message = new
            {
                partnerCode = PartnerCode,
                accessKey = AccessKey,
                requestId,
                amount,
                orderId,
                orderInfo,
                redirectUrl = RedirectUrl,
                ipnUrl = IpnUrl,
                extraData,
                requestType = "captureWallet",
                signature,
                lang = "en"
            };

            // Gửi HTTP POST đến API MoMo
            using var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(Endpoint, content);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("MoMo Response: " + result);

            // Trả về cho Flutter
            return Content(result, "application/json");

        }
    }

    // Model nhận từ Flutter
    public class MoMoRequest
    {
        public int Amount { get; set; }
    }

}
