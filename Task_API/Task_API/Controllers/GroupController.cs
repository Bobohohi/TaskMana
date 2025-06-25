using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public GroupController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getGroup()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Groups.Select(t => new
                {
                    GroupId = t.GroupId,
                    GroupName = t.GroupName,
                    GroupSize = t.GroupSize,
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
        public IActionResult addGroup(Add_Group x)
        {
            try
            {
                using var db = new TaskManagementContext();

                // 1. Tạo nhóm mới
                Group newG = new Group
                {
                    GroupName = x.GroupName,
                    GroupSize = x.GroupSize,
                    UserId = x.UserId // Người tạo nhóm
                };

                db.Groups.Add(newG);
                db.SaveChanges(); // Sau lệnh này, newG.GroupId đã có giá trị

                // 2. Thêm người tạo vào GroupDetail với Role là "owner"
                GroupDetail detail = new GroupDetail
                {
                    GroupId = newG.GroupId,
                    UserId = x.UserId,
                    RoleInGroup = "owner"
                };

                db.GroupDetails.Add(detail);
                db.SaveChanges();

                return Ok(new { message = "Tạo nhóm thành công", groupId = newG.GroupId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("GetListGroupByUserId/{UserId}")]
        public ActionResult<IEnumerable<Group>> GetListGroupByUserId(int UserId)
        {
            var Groups = (from p in _context.Groups
                         where p.UserId == UserId
                         select p).ToList();
            return Ok(Groups);
        }

        [HttpGet("{id}")]
        public ActionResult GetGroupById(int id)
        {
            try
            {
                using (TaskManagementContext db = new TaskManagementContext())
                {
                    var Group = db.Groups
                        .Where(r => r.GroupId == id)
                        .Select(r => new
                        {
                            GroupId = r.GroupId,
                            GroupName = r.GroupName,
                            GroupSize = r.GroupSize,
                            UserId = r.UserId,
                        })
                        .FirstOrDefault();
                    if (Group == null)
                    {
                        return NotFound(new { message = "Group not found" });
                    }

                    return Ok(Group);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
    }
}
