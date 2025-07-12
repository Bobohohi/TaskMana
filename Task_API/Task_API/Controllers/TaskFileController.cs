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
        [HttpDelete("{fileId}")]
        public IActionResult DeleteTaskFile(int fileId)
        {
            try
            {
                var taskFile = _context.TaskFiles.FirstOrDefault(f => f.FileId == fileId);

                if (taskFile == null)
                {
                    return NotFound(new { message = "TaskFile not found" });
                }

                _context.TaskFiles.Remove(taskFile);
                _context.SaveChanges();

                return Ok(new { message = "File deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa file", error = ex.Message });
            }
        }
        [HttpPut("{fileId}")]
        public IActionResult UpdateTaskFile(int fileId, [FromBody] TaskFile model)
        {
            try
            {
                var taskFile = _context.TaskFiles.FirstOrDefault(f => f.FileId == fileId);

                if (taskFile == null)
                {
                    return NotFound(new { message = "TaskFile not found" });
                }

                // Gán lại các giá trị từ model
                taskFile.TaskId = model.TaskId;
                taskFile.FileName = model.FileName;
                taskFile.FileType = model.FileType;
                taskFile.FilePath = model.FilePath;
                taskFile.UploadedAt = model.UploadedAt;
                taskFile.UserId = model.UserId;

                _context.TaskFiles.Update(taskFile);
                _context.SaveChanges();

                return Ok(new { message = "File updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật file", error = ex.Message });
            }
        }
        [HttpGet("{fileId}")]
        public IActionResult GetTaskFileById(int fileId)
        {
            try
            {
                var taskFile = _context.TaskFiles
                    .Where(f => f.FileId == fileId)
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
                    .FirstOrDefault();

                if (taskFile == null)
                {
                    return NotFound(new { message = "Không tìm thấy file." });
                }

                return Ok(taskFile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }

    }
}
