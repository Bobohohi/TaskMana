using System.Threading.Tasks;
using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static LuanVanTotNghiep.Repository.Task_Repo;

namespace LuanVanTotNghiep.Controllers
{
    public class Dashboard_TaskController : Controller
    {
        private readonly Task_Repo taskService = new Task_Repo();

        public async Task<ActionResult> Index()
        {
            await taskService.ResetDailyTasks();
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Sign_in", "Index");
            }

            return View(await taskService.GetAllTaskByUserId(userId));
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(); // Trả về view có form nhập Title, Description, DueDate
        }
        [HttpPost]
        public async Task<ActionResult> Create(Add_Single_Task model)
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }
            model.UserId = int.Parse(userIdString);
            model.CreatedDate = DateTime.Now;
            model.Status = "Doing";
            model.AssignedTo = int.Parse(userIdString);

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = await taskService.CreateTask(model);
                    if (result)
                        return RedirectToAction("Index");

                    ModelState.AddModelError("", "Tạo task thất bại (API không trả về thành công).");
                }
                catch (Exception ex)
                {
                    var fullError = ex.InnerException?.Message ?? ex.Message;
                    return BadRequest(new { message = "Lỗi khi tạo Task", error = fullError });
                }
            }

            return View(model);
        }

        public IActionResult DueDateInput()
        {
            return PartialView("DueDateInput");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int taskId, string currentStatus)
        {
            string newStatus = currentStatus == "Delete"
                ? "Delete"
                : (currentStatus == "Done" ? "Doing" : "Done");
            bool result = await taskService.UpdateTaskStatus_Query(taskId, newStatus);


            return RedirectToAction("Index"); 
        }

    }
}

