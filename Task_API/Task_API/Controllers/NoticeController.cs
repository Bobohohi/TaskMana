using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticeController : ControllerBase
    {
        private readonly TaskManagementContext _context;

        public NoticeController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getNotice()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Notifications.Select(t => new
                {
                    NotificationId = t.NotificationId,
                    UserId = t.UserId,
                    Message = t.Message,
                    IsRead = t.IsRead,
                    SentDate = t.SentDate,
           

                }).ToList();
                return Ok(ds);

            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpGet("UserId")]
        public ActionResult<IEnumerable<Notification>> GetNoticeByUserId(int userId)
        {
            var notifications = (from p in _context.Notifications
                            where p.UserId == userId
                            select p).ToList();
            return Ok(notifications);
        }
        [HttpPost]
        public IActionResult addNotice(Add_Notice x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                Notification newP = new Notification
                {
                    UserId = x.UserId,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    SentDate = x.SentDate,
                };
                db.Notifications.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost("UpdateIsRead/{id}")]
        public IActionResult UpdateIsRead(int id)
        {
            try
            {
                var notice = _context.Notifications.FirstOrDefault(n => n.NotificationId == id);
                if (notice == null)
                {
                    return NotFound("Không tìm thấy thông báo.");
                }

                notice.IsRead = true;
                _context.SaveChanges();

                return Ok("Đã đánh dấu là đã đọc.");
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi cập nhật thông báo: " + ex.Message);
            }
        }
        [HttpPost("addListNotice")]
        public IActionResult AddListNotice([FromBody] List<Add_Notice> listNotice)
        {
            try
            {
                using (var db = new TaskManagementContext())
                {
                    var notifications = listNotice.Select(x => new Notification
                    {
                        UserId = x.UserId,
                        Message = x.Message,
                        IsRead = x.IsRead,
                        SentDate = x.SentDate
                    }).ToList();

                    db.Notifications.AddRange(notifications);
                    db.SaveChanges();
                }

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpDelete("{notificationId}")]
        public IActionResult DeleteNotice(int notificationId)
        {
            try
            {
                var notice = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);

                if (notice == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông báo." });
                }

                _context.Notifications.Remove(notice);
                _context.SaveChanges();

                return Ok(new { message = "Thông báo đã được xóa thành công.", notificationId = notificationId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa thông báo", error = ex.Message });
            }
        }
        [HttpPut("{notificationId}")]
        public IActionResult UpdateNotice(int notificationId, [FromBody] Notification model)
        {
            try
            {
                var notice = _context.Notifications.FirstOrDefault(n => n.NotificationId == notificationId);

                if (notice == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông báo." });
                }

                notice.Message = model.Message;
                notice.IsRead = model.IsRead;
                notice.SentDate = model.SentDate;
                notice.UserId = model.UserId;

                _context.SaveChanges();

                return Ok(new { message = "Cập nhật thông báo thành công.", notificationId = notificationId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật thông báo", error = ex.Message });
            }
        }

        [HttpGet("{notificationId}")]
        public IActionResult GetNoticeById(int notificationId)
        {
            try
            {
                var notice = _context.Notifications
                    .Where(n => n.NotificationId == notificationId)
                    .Select(n => new
                    {
                        NotificationId = n.NotificationId,
                        UserId = n.UserId,
                        Message = n.Message,
                        IsRead = n.IsRead,
                        SentDate = n.SentDate
                    })
                    .FirstOrDefault();

                if (notice == null)
                {
                    return NotFound(new { message = "Không tìm thấy thông báo." });
                }

                return Ok(notice);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }

    }
}
