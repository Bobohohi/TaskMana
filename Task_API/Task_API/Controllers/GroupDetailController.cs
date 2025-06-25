using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupDetailController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public GroupDetailController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getGDetail()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.GroupDetails.Select(t => new
                {
                    GroupDetailId = t.GroupDetailId,
                    GroupId = t.GroupId,
                    UserId = t.UserId,
                    RoleInGroup = t.RoleInGroup,    
                }).ToList();
                return Ok(ds);
            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult AddMember([FromBody] Add_GroupDetail newMember)
        {
            try
            {
                using (var db = new TaskManagementContext())
                {
                    // Kiểm tra tồn tại
                    var exists = db.GroupDetails.Any(g =>
                        g.GroupId == newMember.GroupId && g.UserId == newMember.UserId);

                    if (exists)
                    {
                        return BadRequest("Thành viên đã tồn tại trong nhóm.");
                    }
                    var entity = new GroupDetail
                    {
                        GroupId = newMember.GroupId,
                        UserId = newMember.UserId,
                        RoleInGroup = newMember.RoleInGroup
                    };

                    db.GroupDetails.Add(entity);
                    db.SaveChanges();
                    return Ok("Thêm thành viên thành công.");
                }
            }
            catch (Exception ex)
            {
                string innerMessage = ex.InnerException?.Message ?? ex.Message;
                return BadRequest("Lỗi khi thêm thành viên: " + innerMessage);
            }
        }

        [HttpGet("GetAllUserByGroupId/{groupId}")]
        public IActionResult GetAllUserByGroupId(int groupId)
        {
            try
            {
                using (var db = new TaskManagementContext())
                {
                    var users = (from gd in db.GroupDetails
                                 join u in db.Users on gd.UserId equals u.UserId
                                 where gd.GroupId == groupId
                                 select new
                                 {
                                     u.UserId,
                                     u.Name,
                                     u.Email,
                                     gd.RoleInGroup
                                 }).ToList();

                    return Ok(users);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi: " + ex.Message);
            }
        }

        [HttpGet("GetAllGroupByUserId/{userId}")]
        public IActionResult GetAllGroupByUserId(int userId)
        {
            try
            {
                using (var db = new TaskManagementContext())
                {
                    var groups = (from gd in db.GroupDetails
                                  join g in db.Groups on gd.GroupId equals g.GroupId
                                  where gd.UserId == userId
                                  select new
                                  {
                                      g.GroupId,
                                      g.GroupName,
                                      g.GroupSize,
                                      gd.RoleInGroup
                                  }).ToList();

                    return Ok(groups);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi lấy danh sách nhóm: " + ex.Message);
            }
        }
        [HttpGet("GetRoleByGroupAndUser/{groupId}/{userId}")]
        public IActionResult GetRoleByGroupAndUser(int groupId, int userId)
        {
            try
            {
                using (var db = new TaskManagementContext())
                {
                    var role = db.GroupDetails
                                 .Where(gd => gd.GroupId == groupId && gd.UserId == userId)
                                 .Select(gd => gd.RoleInGroup)
                                 .FirstOrDefault();

                    if (role == null)
                    {
                        return NotFound("Không tìm thấy vai trò của người dùng trong nhóm.");
                    }

                    return Ok(new { RoleInGroup = role });
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi truy vấn vai trò: " + ex.Message);
            }
        }

    }
}
