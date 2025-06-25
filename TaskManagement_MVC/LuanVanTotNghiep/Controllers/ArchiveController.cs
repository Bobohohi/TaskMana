using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LuanVanTotNghiep.Controllers
{
    public class ArchiveController : Controller
    {

        private readonly Task_Repo taskService = new Task_Repo();

        public async Task<ActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Sign_in", "Index");
            }

            return View(await taskService.GetAllTaskByUserId(userId));
        }
        [HttpPost]
        public async Task<IActionResult> Restore(int taskId, string currentStatus)
        {
            string newStatus = currentStatus == "Delete" ? "Doing" : "Delete";
            bool result = await taskService.UpdateTaskStatus_Query(taskId, newStatus);
            return RedirectToAction("Index");
        }
    }
}
