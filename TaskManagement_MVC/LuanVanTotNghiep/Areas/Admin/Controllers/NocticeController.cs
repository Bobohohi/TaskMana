using LuanVanTotNghiep.Models;
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
        [HttpGet]
        public async Task<ActionResult> NoticeDetail(int id)
        {
            var notice = await NoticeService.GetNoticeById(id);
            return View(notice);
        }

        // GET: Tạo mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NotificationItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await NoticeService.CreateNotice(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm thông báo thất bại.");
            }
            return View(model);
        }

        // GET: Cập nhật
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var notice = await NoticeService.GetNoticeById(id);
            return View(notice);
        }

        // POST: Cập nhật
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(NotificationItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await NoticeService.UpdateNotice(model.NotificationId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật thất bại.");
            }
            return View(model);
        }

        // GET: Xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var notice = await NoticeService.GetNoticeById(id);
            return View(notice);
        }

        // POST: Xóa xác nhận
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await NoticeService.DeleteNotice(id);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Xóa thất bại.");
            var notice = await NoticeService.GetNoticeById(id);
            return View("Delete", notice);
        }
    }
}
