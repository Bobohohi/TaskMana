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
                    Status=t.Status
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
                            Status=r.Status
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
        [HttpPut("UpdateStatus/{groupId}")]
        public IActionResult UpdateGroupStatus(int groupId, [FromBody] string newStatus)
        {
            try
            {
                var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);

                if (group == null)
                {
                    return NotFound(new { message = "Group not found" });
                }

                group.Status = newStatus; // Make sure the Group model includes a 'Status' property
                _context.SaveChanges();

                return Ok(new { message = "Status updated successfully", groupId = groupId, status = newStatus });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal Server Error", error = ex.Message });
            }
        }
        [HttpPut("UpdateName/{groupId}")]
        public IActionResult UpdateGroupName(int groupId, [FromBody] string newName)
        {
            try
            {
                var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);
                if (group == null)
                {
                    return NotFound(new { message = "Group không tồn tại" });
                }

                group.GroupName = newName;
                _context.SaveChanges();

                return Ok(new { message = "Đổi tên nhóm thành công", groupId = group.GroupId, groupName = group.GroupName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }
        [HttpDelete("{groupId}")]
        public IActionResult DeleteGroup(int groupId)
        {
            try
            {
                var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);

                if (group == null)
                {
                    return NotFound(new { message = "Group không tồn tại" });
                }

                // Xóa các GroupDetails liên quan trước nếu có ràng buộc khóa ngoại
                //var groupDetails = _context.GroupDetails.Where(d => d.GroupId == groupId).ToList();
                //if (groupDetails.Any())
                //{
                //    _context.GroupDetails.RemoveRange(groupDetails);
                //}

                _context.Groups.Remove(group);
                _context.SaveChanges();

                return Ok(new { message = "Xóa nhóm thành công", groupId = groupId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa nhóm", error = ex.Message });
            }
        }
        [HttpPut("UpdateGroup/{groupId}")]
        public IActionResult UpdateGroup(int groupId, [FromBody] Group updatedGroup)
        {
            try
            {
                var group = _context.Groups.FirstOrDefault(g => g.GroupId == groupId);

                if (group == null)
                {
                    return NotFound(new { message = "Group không tồn tại" });
                }

                group.GroupName = updatedGroup.GroupName;
                group.GroupSize = updatedGroup.GroupSize;
                group.UserId = updatedGroup.UserId;
                group.Status = updatedGroup.Status;

                _context.Groups.Update(group);
                _context.SaveChanges();

                return Ok(new { message = "Cập nhật nhóm thành công", groupId = group.GroupId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật nhóm", error = ex.Message });
            }
        }


    }
}
