using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskFileController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public TaskFileController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getTaskFile()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.TaskFiles.Select(t => new
                {
                    FileId = t.FileId,
                    TaskId = t.TaskId,
                    FileName = t.FileName,
                    FileType = t.FileType,
                    FilePath = t.FilePath,
                    UploadedAt = t.UploadedAt,
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
        public IActionResult addTaskFile(Add_TaskFile x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                TaskFile newP = new TaskFile
                {
                    TaskId = x.TaskId,
                    FileName = x.FileName,
                    FileType = x.FileType,
                    FilePath = x.FilePath,
                    UploadedAt = x.UploadedAt,
                    UserId = x.UserId,
                };
                db.TaskFiles.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetTaskFileByTaskId")]
        public IActionResult GetTaskFileByTaskId(int taskId)
        {
            try
            {
                var files = _context.TaskFiles
                    .Where(f => f.TaskId == taskId)
                    .Select(t => new
                    {
                        FileId = t.FileId,
                        TaskId = t.TaskId,
                        FileName = t.FileName,
                        FileType = t.FileType,
                        FilePath = t.FilePath,
                        UploadedAt = t.UploadedAt,
                        UserId = t.UserId,
                    })
                    .ToList();

                if (!files.Any())
                    return NotFound("Không có file nào cho task này.");

                return Ok(files);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi lấy danh sách file: " + ex.Message);
            }
        }

    }
}
