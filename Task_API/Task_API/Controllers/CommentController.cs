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
        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment(int commentId)
        {
            try
            {
                var comment = _context.Comments.FirstOrDefault(c => c.CommentId == commentId);

                if (comment == null)
                {
                    return NotFound(new { message = "Không tìm thấy bình luận." });
                }

                _context.Comments.Remove(comment);
                _context.SaveChanges();

                return Ok(new { message = "Đã xóa bình luận thành công", commentId = commentId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa bình luận", error = ex.Message });
            }
        }
        [HttpPut("{commentId}")]
        public IActionResult UpdateComment(int commentId, [FromBody] Comment model)
        {
            try
            {
                var comment = _context.Comments.FirstOrDefault(c => c.CommentId == commentId);

                if (comment == null)
                {
                    return NotFound(new { message = "Không tìm thấy bình luận." });
                }

                comment.Content = model.Content;
                comment.CreatedAt = model.CreatedAt; // Có thể giữ nguyên thời gian cũ nếu muốn
                _context.SaveChanges();

                return Ok(new { message = "Cập nhật bình luận thành công", commentId = comment.CommentId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật bình luận", error = ex.Message });
            }
        }
        [HttpGet("{commentId}")]
        public IActionResult GetCommentById(int commentId)
        {
            try
            {
                var comment = _context.Comments
                    .Include(c => c.User)
                    .Where(c => c.CommentId == commentId)
                    .Select(c => new
                    {
                        CommentId = c.CommentId,
                        TaskId = c.TaskId,
                        UserId = c.UserId,
                        Name = c.User.Name,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt
                    })
                    .FirstOrDefault();

                if (comment == null)
                {
                    return NotFound(new { message = "Không tìm thấy bình luận." });
                }

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }


    }
}
