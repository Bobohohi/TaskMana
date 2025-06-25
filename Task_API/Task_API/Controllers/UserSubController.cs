using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public UserSubController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getUSub()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.UserSubs.Select(t => new
                {
                    SubscriptionId = t.SubscriptionId,
                    UserId = t.UserId,
                    PlanId = t.PlanId,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    IsActive = t.IsActive,
                }).ToList();
                return Ok(ds);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult addUserSub(Add_UserSub x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                UserSub newP = new UserSub
                {
                    UserId = x.UserId,
                    PlanId = x.PlanId,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                };
                db.UserSubs.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetByUserId/{userId}")]
        public IActionResult GetUserSubByUserId(int userId)
        {
            try
            {
                var subscriptions = _context.UserSubs
                    .Where(t => t.UserId == userId)
                    .Select(t => new
                    {
                        SubscriptionId = t.SubscriptionId,
                        UserId = t.UserId,
                        PlanId = t.PlanId,
                        StartDate = t.StartDate,
                        EndDate = t.EndDate,
                        IsActive = t.IsActive,
                    })
                    .ToList();

                if (subscriptions == null || subscriptions.Count == 0)
                {
                    return NotFound("Không tìm thấy đăng ký nào cho người dùng này.");
                }

                return Ok(subscriptions);
            }
            catch
            {
                return BadRequest("Có lỗi xảy ra khi truy xuất dữ liệu.");
            }
        }
        [HttpGet("CheckValidSubscription")]
        public IActionResult CheckValidSubscription(int userId)
        {
            try
            {
                var currentDate = DateTime.Now;

                // 1. Tự động cập nhật các gói hết hạn
                var expiredSubs = _context.UserSubs
                    .Where(us => us.EndDate < currentDate && us.IsActive == true)
                    .ToList();

                foreach (var sub in expiredSubs)
                {
                    sub.IsActive = false;
                }
                _context.SaveChanges();

                // 2. Kiểm tra gói còn hiệu lực
                var isValid = _context.UserSubs.Any(us =>
                    us.UserId == userId &&
                    us.StartDate <= currentDate &&
                    us.EndDate >= currentDate &&
                    us.IsActive == true
                );

                return Ok(new { isValid });
            }
            catch
            {
                return BadRequest("Lỗi kiểm tra đăng ký hợp lệ.");
            }
        }

        [HttpGet("GetCurrentSubscriptionByUserId/{userId}")]
        public IActionResult GetCurrentSubscriptionByUserId(int userId)
        {
            try
            {
                var currentDate = DateTime.Now;

                var currentSub = _context.UserSubs
                    .Where(us =>
                        us.UserId == userId &&
                        us.StartDate <= currentDate &&
                        us.EndDate >= currentDate &&
                        us.IsActive == true)
                    .OrderByDescending(us => us.StartDate) // Nếu có nhiều, lấy cái mới nhất
                    .FirstOrDefault();

                if (currentSub == null)
                {
                    return NotFound("Không có gói đăng ký đang hoạt động.");
                }

                return Ok(new
                {
                    currentSub.SubscriptionId,
                    currentSub.UserId,
                    currentSub.PlanId,
                    currentSub.StartDate,
                    currentSub.EndDate,
                    currentSub.IsActive
                });
            }
            catch
            {
                return BadRequest("Lỗi khi lấy thông tin gói đăng ký hiện tại.");
            }
        }


    }
}
