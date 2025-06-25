using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly Report_Repo ReportService = new Report_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await ReportService.GetAllReport());
        }
    }
}
