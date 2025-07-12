using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FileController : Controller
    {
        private readonly TaskFile_Repo TaskFileService = new TaskFile_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await TaskFileService.GetAllFile());
        }
        [HttpGet]
        public async Task<ActionResult> TaskFileDetail(int id)
        {
            return View(await TaskFileService.GetTaskFileById(id));
        }
        
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var file = await TaskFileService.GetTaskFileById(id);
            return View(file);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await TaskFileService.DeleteTaskFile(id);
            if (result) return RedirectToAction("Index");
            ModelState.AddModelError("", "Xóa thất bại.");
            var file = await TaskFileService.GetTaskFileById(id);
            return View("Delete", file);
        }
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var file = await TaskFileService.GetTaskFileById(id);
            return View(file);
        }
        [HttpPost]
        public async Task<ActionResult> Update(TaskFileItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await TaskFileService.UpdateTaskFile(model.FileId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật thất bại.");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(TaskFileItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await TaskFileService.CreateTaskFile(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm file thất bại.");
            }
            return View(model);
        }
    }
}
