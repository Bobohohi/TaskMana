using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Controllers
{
    public class StaticReportController : Controller
    {
        private readonly StaticReport_Repo StaticReportSẻvice = new StaticReport_Repo();

        public async Task<ActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int userId= int.Parse(userIdString);
            var stats = await StaticReportSẻvice.GetUserStatsByIdAsync(userId);
            if (stats == null)
            {
                ViewBag.Error = "Không thể tải dữ liệu.";
                return View();
            }

            return View(stats);
        }
    }
}
