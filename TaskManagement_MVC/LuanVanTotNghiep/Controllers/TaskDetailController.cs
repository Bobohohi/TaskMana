using LuanVanTotNghiep.Models;
using LuanVanTotNghiep.Repository;
using Microsoft.AspNetCore.Mvc;

namespace LuanVanTotNghiep.Controllers
{
    public class TaskDetailController : Controller
    {
        private readonly Comment_Repo CommentService = new Comment_Repo();
        private readonly Task_Repo TaskService = new Task_Repo();
        private readonly TaskFile_Repo TaskFileService = new TaskFile_Repo();
        private readonly TaskDetail_Repo TaskDetailService = new TaskDetail_Repo();
        private readonly GroupDetail_Repo GroupDetailService = new GroupDetail_Repo();
        public async Task<ActionResult> Index(int id)
        {
            return View(await TaskService.GetTaskByTaskId(1));
        }
        public async Task<IActionResult> LoadFileList(int taskId)
        {
            var Files = await TaskFileService.GetAllTaskFileByTaskId(taskId);
            return PartialView("LoadFileList", Files);
        }
        public async Task<IActionResult> LoadTaskMemberList(int taskId)
        {
            var UsersOutput = await TaskDetailService.GetAllUserByTaskId(taskId);
            return PartialView("LoadTaskMemberList", UsersOutput);
        }
        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentItemInput model, int groupId)
        {
            ViewBag.GroupId = groupId;
            if (!ModelState.IsValid)
                return BadRequest("Dữ liệu không hợp lệ.");
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ModelState.AddModelError("", "Không xác định được người dùng.");
                return View(model);
            }
            model.UserId = int.Parse(userIdString);
            var result = await CommentService.CreateComment(model);
            return result ? Ok() : StatusCode(500);
        }
    }
}
