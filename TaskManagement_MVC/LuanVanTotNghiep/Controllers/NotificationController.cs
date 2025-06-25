using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Controllers
{
    public class NotificationController : Controller
    {
        private readonly Notification_Repo NotificationService = new Notification_Repo();
        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            int UserId = int.Parse(userIdString);
            return View(await NotificationService.GetAllNoticeByUserId(UserId));
        }
        public async Task<IActionResult> UpdateIsReadNotice(int id)
        {

            await NotificationService.UpdateIsRead(id);
            return RedirectToAction("Index");
        }

    }
}
