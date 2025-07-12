using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaskManagementController : Controller
    {
        private readonly Task_Repo TaskService = new Task_Repo();
        public async Task<ActionResult> Index()
        {
            return View(await TaskService.GetAllTasks());
        }
        [HttpGet]
        public async Task<ActionResult> TaskDetail(int id)
        {
            var task = await TaskService.GetTaskByTaskId(id);
            return View(task);
        }

        // GET: Tạo công việc mới
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo công việc mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Add_Single_Task model)
        {
            if (ModelState.IsValid)
            {
                var result = await TaskService.CreateTask(model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Thêm công việc thất bại.");
            }
            return View(model);
        }

        // GET: Cập nhật công việc
        [HttpGet]
        public async Task<ActionResult> Update(int id)
        {
            var task = await TaskService.GetTaskByTaskId(id);
            return View(task);
        }

        // POST: Cập nhật công việc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(TaskItem model)
        {
            if (ModelState.IsValid)
            {
                var result = await TaskService.UpdateTask(model.TaskId, model);
                if (result) return RedirectToAction("Index");
                ModelState.AddModelError("", "Cập nhật thất bại.");
            }
            return View(model);
        }

        // GET: Xác nhận xóa
        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var task = await TaskService.GetTaskByTaskId(id);
            return View(task);
        }

        // POST: Thực hiện xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var result = await TaskService.DeleteTask(id);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Xóa công việc thất bại.");
            var task = await TaskService.GetTaskByTaskId(id);
            return View("Delete", task);
        }
    }
}
