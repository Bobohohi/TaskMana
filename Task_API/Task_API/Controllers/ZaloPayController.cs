using API_HOTEL.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZaloPayController : ControllerBase
    {
        private const string AppId = "2553";
        private const string Key1 = "PcY4iZIKFCIdgZvA6ueMcMHHUbRLYjPL";
        private const string CreateOrderUrl = "https://sb-openapi.zalopay.vn/v2/create";

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder([FromBody] ZaloPayRequest request)
        {
            var appTransId = DateTime.Now.ToString("yyMMdd") + "_" + new Random().Next(1000000);
            var param = new Dictionary<string, string>
    {
        { "app_id", AppId },
        { "app_user", request.AppUser },
        { "app_time", Utils.GetTimeStamp().ToString() },
        { "amount", request.Amount.ToString() },
        { "app_trans_id", appTransId },
        { "embed_data", "{}" },
        { "item", "[]" },
        { "description", $"Demo thanh toán #{appTransId}" },
        {"appReturnUrl", "myapp://zalopay-success" },
        { "callback_url", "https://localhost:44340/GroupAndProject" }
};

            var data = string.Join("|", AppId, appTransId, request.AppUser, request.Amount, param["app_time"], param["embed_data"], param["item"]);
            param["mac"] = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, Key1, data);

            var result = await HttpHelper.PostFormAsync(CreateOrderUrl, param);

            if (result != null && result.ContainsKey("return_code") && result["return_code"] == "1")
            {
                return Ok(new
                {
                    order_url = result["order_url"],
                    token = result["zp_trans_token"],
                    transactionId = appTransId,
                    message = result["return_message"],
                    mac = param["mac"],
                    data = data

                });
            }

            // Nếu lỗi, trả nguyên thông báo từ ZaloPay
            return BadRequest(result);
        }
        [HttpPost("payment-callback")]
        public IActionResult PaymentCallback([FromBody] ZaloPayCallback callback)
        {
            // 👇 Đây là chuỗi JSON string (escaped), dùng nguyên văn để tính MAC
            string data = callback.data;
            string mac = callback.mac;

            // ✅ Tính lại MAC
            string calculatedMac = HmacHelper.Compute(ZaloPayHMAC.HMACSHA256, Key1, data);

            if (mac != calculatedMac)
            {
                Console.WriteLine("❌ MAC mismatch");
                Console.WriteLine("Expected MAC: " + calculatedMac);
                Console.WriteLine("Received MAC: " + mac);
                Console.WriteLine("Raw data: " + data);
                return BadRequest(new { return_code = -1, return_message = "Invalid MAC" });
            }

            // ✅ Đúng MAC, xử lý data
            dynamic jsonData = JsonConvert.DeserializeObject(data);
            string transId = jsonData.app_trans_id;
            long amount = jsonData.amount;
            int status = jsonData.status;

            Console.WriteLine($"✅ Payment callback OK: {transId}, amount: {amount}, status: {status}");

            return Ok(new { return_code = 1, return_message = "Success" });
        }

        public class ZaloPayCallback
        {
            public string data { get; set; }
            public string mac { get; set; }
        }

        public class ZaloPayRequest
        {
            public int Amount { get; set; }
            public string AppUser { get; set; } = "user123";
        }

        private static readonly HashSet<string> PaidTransactions = new();
        [HttpPost("notify")]
        public IActionResult Notify([FromQuery] string transactionId)
        {
            PaidTransactions.Add(transactionId);
            Console.WriteLine("notify: added " + transactionId);
            return Ok(new { added = transactionId });
        }

        [HttpGet("status")]
        public IActionResult GetPaymentStatus([FromQuery] string transactionId)
        {
            Console.WriteLine("PaidTransactions: " + string.Join(", ", PaidTransactions));
            if (PaidTransactions.Contains(transactionId))

            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false });
        }

    }
}
