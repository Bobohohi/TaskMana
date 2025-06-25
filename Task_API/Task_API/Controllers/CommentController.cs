using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public CommentController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getComment()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Comments
                  .Include(c => c.User)
                  .Select(c => new {
                      CommentId = c.CommentId,
                      TaskId = c.TaskId,
                      UserId = c.UserId,
                      Name = c.User.Name,
                      Content = c.Content,
                      CreatedAt = c.CreatedAt
                  }).ToList();
                return Ok(ds);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult addComment(Add_Comment x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                Comment newP = new Comment
                {
                    TaskId = x.TaskId,
                    UserId = x.UserId,
                    Content = x.Content,
                    CreatedAt = x.CreatedAt,
                };
                db.Comments.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetCommentByTaskId")]
        public IActionResult GetCommentByTaskId(int taskId)
        {
            try
            {
                var comments = _context.Comments
                    .Where(c => c.TaskId == taskId)
                    .Select(t => new
                    {
                        CommentId = t.CommentId,
                        TaskId = t.TaskId,
                        UserId = t.UserId,
                        Content = t.Content,
                        CreatedAt = t.CreatedAt,
                    })
                    .ToList();

                if (!comments.Any())
                {
                    return NotFound("Không có bình luận nào cho task này.");
                }

                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi lấy bình luận: " + ex.Message);
            }
        }

    }
}
