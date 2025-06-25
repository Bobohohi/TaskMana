using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Controllers
{
    public class MembershipController : Controller
    {
        private readonly Membership_Repo MemService = new Membership_Repo();
        private readonly UserSub_Repo UserSubService = new UserSub_Repo();
        private readonly PayOS_Repo PayOSService = new PayOS_Repo();
        public async Task<IActionResult> Index()
        {
            TempData["ShowModal"] = true;
            return View(await MemService.GetAllMembership());
        }



        [HttpPost]
        public async Task<IActionResult> PayOS(int PlanId, decimal Amount)
        {
            var item = new PayOSItem
            {
                ProductName = PlanId.ToString(),
                Amount = ((int)Amount),
                Description = "Subcribe Membership",
                CancelUrl = "https://localhost:44340/cancel",
                ReturnUrl = $"https://localhost:44340/GroupAndProject/PaymentSuccess?planId={PlanId}"
            };

            var url = await PayOSService.CreatePaymentLink(item);

            return Redirect(url);
        }

        //[HttpPost]
        //public async Task<IActionResult> StartMomoPayment(int PlanId, decimal Amount)
        //{
        //    string userId = HttpContext.Session.GetString("UserId");
        //    if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Auth");

        //    // Chuẩn bị dữ liệu gửi sang Web API
        //    var requestData = new
        //    {
        //        Amount = (int)Amount + 2000
        //    };

        //    using var client = new HttpClient();
        //    client.BaseAddress = new Uri("https://localhost:7208/"); // Địa chỉ Web API MoMo
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    var jsonContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

        //    var response = await client.PostAsync("api/Momo/create-payment", jsonContent);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        TempData["Error"] = "Không thể kết nối cổng thanh toán.";
        //        return RedirectToAction("Index");
        //    }

        //    var result = await response.Content.ReadAsStringAsync();
        //    var paymentResponse = JsonConvert.DeserializeObject<dynamic>(result);

        //    // Điều hướng người dùng tới MoMo
        //    string payUrl = paymentResponse.payUrl;
        //    return Redirect(payUrl);
        //}
        //public async Task<IActionResult> PaymentCallback(string resultCode, string orderId, string extraData)
        //{
        //    if (resultCode != "0")
        //    {
        //        TempData["Error"] = "Thanh toán thất bại hoặc bị huỷ.";
        //        return RedirectToAction("Index");
        //    }

        //    // Giải mã extraData nếu cần hoặc lưu session trước đó
        //    string userId = HttpContext.Session.GetString("UserId");
        //    int planId = HttpContext.Session.GetInt32("PendingPlanId") ?? 0;

        //    var newSub = new UserSubItem
        //    {
        //        UserId = int.Parse(userId),
        //        PlanId = planId,
        //        StartDate = DateTime.Now,
        //        EndDate = DateTime.Now.AddMonths(1),
        //        IsActive = true
        //    };

        //    await UserSubService.CreateUserSub(newSub);
        //    TempData["Message"] = "Đăng ký gói thành công!";
        //    return RedirectToAction("Index", "GroupAndProject");
        //}

    }
}
