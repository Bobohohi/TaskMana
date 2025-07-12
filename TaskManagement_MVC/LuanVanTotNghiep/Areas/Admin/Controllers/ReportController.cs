using LuanVanTotNghiep.Models;
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
        [HttpGet]
        public async Task<ActionResult> ReportDetail(int id)
        {
            var report = await ReportService.GetReportById(id);
            return View(report);
        }

        // GET: Thêm mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Thêm mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ReportItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await ReportService.CreateReport(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm báo cáo thất bại.");
            }
            return View(model);
        }

        // GET: Cập nhật
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var report = await ReportService.GetReportById(id);
            return View(report);
        }

        // POST: Cập nhật
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ReportItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await ReportService.UpdateReport(model.ReportId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật thất bại.");
            }
            return View(model);
        }

        // GET: Xác nhận xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var report = await ReportService.GetReportById(id);
            return View(report);
        }

        // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await ReportService.DeleteReport(id);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Xóa thất bại.");
            var report = await ReportService.GetReportById(id);
            return View("Delete", report);
        }
    }
}
