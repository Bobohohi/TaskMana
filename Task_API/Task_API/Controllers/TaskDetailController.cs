using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskDetailController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public TaskDetailController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getTDetail()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.TaskDetails.Select(t => new
                {
                    TaskDetailId = t.TaskDetailId,
                    TaskId = t.TaskId,
                    UserId = t.UserId,
                }).ToList();
                return Ok(ds);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult addTaskDetail(Add_TaskDetail x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                TaskDetail newP = new TaskDetail
                {
                    UserId = x.UserId,
                    TaskId = x.TaskId,
                };
                db.TaskDetails.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetUserByTaskId")]
        public IActionResult GetUserByTaskId(int taskId)
        {
            try
            {
                var users = _context.TaskDetails
                    .Where(td => td.TaskId == taskId)
                    .Join(_context.Users,
                          td => td.UserId,
                          u => u.UserId,
                          (td, u) => new
                          {
                              u.UserId,
                              u.Name,
                              u.Email,
                              td.TaskId,
                              td.TaskDetailId
                          })
                    .ToList();

                if (!users.Any())
                    return NotFound("Không tìm thấy người dùng nào cho task này.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi truy xuất dữ liệu: " + ex.Message);
            }
        }
        [HttpGet("GetAllTaskByUserId/{userId}")]
        public IActionResult GetAllTaskByUserId(int userId)
        {
            try
            {
                using (var db = new TaskManagementContext())
                {
                    var tasks = (from td in db.TaskDetails
                                 join t in db.Tasks on td.TaskId equals t.TaskId
                                 where td.UserId == userId
                                 select new
                                 {
                                     t.TaskId,
                                     t.Description,
                                     t.Status,
                                     t.DueDate,
                                     t.CreatedDate,
                                     t.IsDaily,
                                     t.AssignedTo,
                                     t.ProjectId,
                                     t.BoardId
                                 }).ToList();

                    return Ok(tasks);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi lấy danh sách task: " + ex.Message);
            }
        }
    }
}
