using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NocticeController : Controller
    {
        private readonly Notification_Repo NoticeService = new Notification_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await NoticeService.GetAllNotice());
        }
    }
}
