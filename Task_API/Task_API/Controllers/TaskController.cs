using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public TaskController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getTask()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Tasks.Select(t => new
                {
                    TaskId = t.TaskId,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    Status = t.Status,
                    ProjectId = t.ProjectId,
                    UserId=t.UserId,
                    BoardId=t.BoardId,
                    IsDaily=t.IsDaily
                }).ToList();
                return Ok(ds);

            }
            catch
            {
                return BadRequest();// đúng thì ok, sai thì badrequest()
                                    //NotFound -> không tìm thấy!!!
            }

        }
        [HttpGet("{id}")]
        public ActionResult GetTaskById(int id)
        {
            try
            {
                using var db = new TaskManagementContext();

                var task = db.Tasks
                    .Include(r => r.Comments)
                    .ThenInclude(c => c.User) // include tên người bình luận
                    .Where(r => r.TaskId == id)
                    .Select(r => new
                    {
                        TaskId = r.TaskId,
                        Description = r.Description,
                        CreatedDate = r.CreatedDate,
                        DueDate = r.DueDate,
                        Status = r.Status,
                        AssignedTo = r.AssignedTo,
                        ProjectId = r.ProjectId,
                        UserId = r.UserId,
                        BoardId = r.BoardId,
                        Comments = r.Comments.Select(c => new
                        {
                            CommentId = c.CommentId,
                            UserId = c.UserId,
                            UserName = c.User.Name, // ✅ Lấy tên người comment
                            Content = c.Content,
                            CreatedAt = c.CreatedAt
                        }).ToList()
                    })
                    .FirstOrDefault();

                if (task == null)
                {
                    return NotFound(new { message = "Task not found" });
                }

                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult addTask([FromBody] Add_Single_Task model)
        {
            try
            {
                Task_API.Models.Task newTask = new Task_API.Models.Task
                {
                    Description = model.Description,
                    DueDate = model.DueDate,
                    CreatedDate = model.CreatedDate,
                    Status = model.Status,
                    AssignedTo = model.AssignedTo,
                    UserId = model.UserId,
                    IsDaily=model.IsDaily,
                };
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi tạo Task", error = ex.Message });
            }
        }
        [HttpPost("addTasks")]
        public IActionResult addTask([FromBody] Add_ProjectTask model)
        {
            try
            {
                Task_API.Models.Task newTask = new Task_API.Models.Task
                {
                    Description = model.Description,
                    DueDate = model.DueDate,
                    CreatedDate = model.CreatedDate,
                    Status = model.Status,
                    AssignedTo = model.AssignedTo,
                    UserId = model.UserId,
                    ProjectId=model.ProjectId,
                    BoardId=model.BoardId
                };
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi tạo Task", error = ex.Message });
            }
        }

        [HttpGet("GetListTaskByProjectId/{projectId}")]
        public ActionResult<IEnumerable<Models.Task>> GetListTaskByProjectId(int projectId)
        {
            var Tasks = (from p in _context.Tasks
                          where p.ProjectId == projectId
                          select p).ToList();
            return Ok(Tasks);
        }
        [HttpGet("GetListTaskByUserId/{UserId}")]
        public ActionResult<IEnumerable<Add_Single_Task>> GetListTaskByUserId(int UserId)
        {
            var Tasks = (from p in _context.Tasks
                         where p.UserId == UserId
                         select p).ToList();

            return Ok(Tasks);
        }
        [HttpPost("ResetDailyTasks")]
        public IActionResult ResetDailyTasks()
        {
            try
            {
                var today = DateTime.Today;

                var dailyTasks = _context.Tasks
                    .Where(t => t.IsDaily == true &&
            (!t.FlagDaily.HasValue || t.FlagDaily.Value.Date < today))

                    .ToList();


                foreach (var task in dailyTasks)
                {
                    task.Status = "Doing";
                    task.FlagDaily = DateTime.Now;
                }

                _context.SaveChanges();
                return Ok(new { message = "Đã reset các daily task." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi reset task", error = ex.Message });
            }
        }
        [HttpPatch("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(int taskId, [FromQuery] string newStatus)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            task.Status = newStatus;
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();

            return Ok();
        }




    }
}
