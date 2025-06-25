using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_API.Models;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public UserController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getUser()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Users.Select(t => new
                {
                    UserId = t.UserId,
                    Email = t.Email,
                    PasswordHash = t.PasswordHash,
                    Name = t.Name,
                    PictureUrl = t.PictureUrl,
                    CreatedAt = t.CreatedAt,
                    Address = t.Address,
                    PhoneNumber = t.PhoneNumber,

                }).ToList();
                return Ok(ds);

            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            if (userId <= 0)
                return BadRequest("UserId không hợp lệ.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng với UserId này.");

            return Ok(user);
        }
        [HttpGet("GetUserIdByEmail")]
        public async Task<IActionResult> GetUserIdByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest("Email không được để trống.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return NotFound("Không tìm thấy người dùng với email này.");

            return Ok(new { user.UserId });
        }

        [HttpGet("CountByUserId/{userId}")]
        public IActionResult CountByUserId(int userId, int? month = null, int? year = null)
        {
            try
            {
                var comments = _context.Comments.Where(c => c.UserId == userId);
                var notifications = _context.Notifications.Where(n => n.UserId == userId);
                var tasksCreated = _context.Tasks.Where(t => t.UserId == userId);
                var tasksAssigned = _context.Tasks.Where(t => t.AssignedTo == userId);
                var taskDetails = _context.TaskDetails.Where(td => td.UserId == userId);
                var groups = _context.Groups.Where(g => g.UserId == userId);
                var groupDetails = _context.GroupDetails.Where(gd => gd.UserId == userId);
                var projects = _context.Projects.Where(p => p.UserId == userId);
                var reports = _context.Reports.Where(r => r.UserId == userId);
                var taskFiles = _context.TaskFiles.Where(f => f.UserId == userId);
                var taskHistories = _context.TaskHistories.Where(h => h.UpdatedBy == userId);
                var subscriptions = _context.UserSubs.Where(us => us.UserId == userId);

                // Nếu có tháng và năm thì lọc theo CreatedAt hoặc UpdatedAt (nếu có trường đó trong DB)
                if (month != null && year != null)
                {
                    DateTime start = new DateTime(year.Value, month.Value, 1);
                    DateTime end = start.AddMonths(1);
                    comments = comments.Where(c => c.CreatedAt >= start && c.CreatedAt < end);
                    tasksCreated = tasksCreated.Where(t => t.CreatedAt >= start && t.CreatedAt < end);
                    tasksAssigned = tasksAssigned.Where(t => t.CreatedAt >= start && t.CreatedAt < end);
                    projects = projects.Where(p => p.CreatedAt >= start && p.CreatedAt < end);
                    reports = reports.Where(r => r.CreatedAt >= start && r.CreatedAt < end);
                }

                var stats = new
                {
                    Comments = comments.Count(),
                    Notifications = notifications.Count(),
                    Tasks_Created = tasksCreated.Count(),
                    Tasks_Assigned = tasksAssigned.Count(),
                    TaskDetails = taskDetails.Count(),
                    Groups = groups.Count(),
                    GroupDetails = groupDetails.Count(),
                    Projects = projects.Count(),
                    Reports = reports.Count(),
                    FilesUploaded = taskFiles.Count(),
                    TaskHistories = taskHistories.Count(),
                    Subscriptions = subscriptions.Count()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi thống kê dữ liệu: " + ex.Message);
            }
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] User updatedUser)
        {
            if (updatedUser == null || updatedUser.UserId <= 0)
                return BadRequest("Thông tin người dùng không hợp lệ.");

            var user = await _context.Users.FindAsync(updatedUser.UserId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng.");

            // Cập nhật thông tin
            user.Name = updatedUser.Name;
            user.Address = updatedUser.Address;
            user.PhoneNumber = updatedUser.PhoneNumber;
            user.Email = updatedUser.Email;
            user.BirthDay = updatedUser.BirthDay;
            user.Sex = updatedUser.Sex;
            await _context.SaveChangesAsync();
            return Ok("Cập nhật thông tin người dùng thành công.");
        }
        [HttpPut("UpdatePictureUrl")]
        public async Task<IActionResult> UpdatePictureUrlByUserIdAndPictureUrl(int userId, string pictureUrl)
        {
            if (userId <= 0 || string.IsNullOrEmpty(pictureUrl))
                return BadRequest("Dữ liệu không hợp lệ.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Không tìm thấy người dùng.");

            user.PictureUrl = pictureUrl;
            await _context.SaveChangesAsync();

            return Ok("Cập nhật ảnh đại diện thành công.");
        }


    }
}

