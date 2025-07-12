using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProjectController : Controller
    {
        private readonly Project_Repo ProjectService = new Project_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await ProjectService.GetAllProject());
        }
        [HttpGet]
        public async Task<ActionResult> ProjectDetail(int id)
        {
            var project = await ProjectService.GetProjectById(id);
            return View(project);
        }

        // GET: Tạo mới dự án
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo mới dự án
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProjectItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await ProjectService.CreateProject(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm dự án thất bại.");
            }
            return View(model);
        }

        // GET: Cập nhật dự án
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var project = await ProjectService.GetProjectById(id);
            return View(project);
        }

        // POST: Cập nhật dự án
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(ProjectItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await ProjectService.UpdateProject(model.ProjectId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật dự án thất bại.");
            }
            return View(model);
        }

        // GET: Xác nhận xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var project = await ProjectService.GetProjectById(id);
            return View(project);
        }

        // POST: Xóa dự án
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await ProjectService.DeleteProject(id);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Xóa dự án thất bại.");
            var project = await ProjectService.GetProjectById(id);
            return View("Delete", project);
        }
    }
}
