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


    }
}
