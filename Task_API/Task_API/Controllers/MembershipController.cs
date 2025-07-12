using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        [HttpGet]
        public IActionResult getMembership()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.MembershipPlans.Select(t => new
                {
                    PlanId = t.PlanId,
                    PlanName = t.PlanName,
                    DurationInDays = t.DurationInDays,
                    MaxGroups = t.MaxGroups,
                    MaxGroupMember=t.MaxGroupMember,
                    Price = t.Price,
                }).ToList();
                return Ok(ds);

            }
            catch
            {
                return BadRequest();// đúng thì ok, sai thì badrequest()
                                    //NotFound -> không tìm thấy!!!
            }

        }
        [HttpGet("{planId}")]
        public IActionResult GetMembershipById(int planId)
        {
            try
            {
                using var db = new TaskManagementContext();
                var plan = db.MembershipPlans
                    .Where(p => p.PlanId == planId)
                    .Select(p => new
                    {
                        PlanId = p.PlanId,
                        PlanName = p.PlanName,
                        DurationInDays = p.DurationInDays,
                        MaxGroups = p.MaxGroups,
                        MaxGroupMember = p.MaxGroupMember,
                        Price = p.Price,
                    })
                    .FirstOrDefault();

                if (plan == null)
                    return NotFound(new { message = "Không tìm thấy gói membership." });

                return Ok(plan);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi truy xuất", error = ex.Message });
            }
        }
        [HttpPut("{planId}")]
        public IActionResult UpdateMembership(int planId, [FromBody] MembershipPlan updatedPlan)
        {
            try
            {
                using var db = new TaskManagementContext();
                var existingPlan = db.MembershipPlans.FirstOrDefault(p => p.PlanId == planId);

                if (existingPlan == null)
                    return NotFound(new { message = "Gói membership không tồn tại." });

                existingPlan.PlanName = updatedPlan.PlanName;
                existingPlan.DurationInDays = updatedPlan.DurationInDays;
                existingPlan.MaxGroups = updatedPlan.MaxGroups;
                existingPlan.MaxGroupMember = updatedPlan.MaxGroupMember;
                existingPlan.Price = updatedPlan.Price;

                db.SaveChanges();

                return Ok(new { message = "Cập nhật thành công", planId = planId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật", error = ex.Message });
            }
        }
        [HttpDelete("{planId}")]
        public IActionResult DeleteMembership(int planId)
        {
            try
            {
                using var db = new TaskManagementContext();
                var plan = db.MembershipPlans.FirstOrDefault(p => p.PlanId == planId);

                if (plan == null)
                    return NotFound(new { message = "Gói membership không tồn tại." });

                db.MembershipPlans.Remove(plan);
                db.SaveChanges();

                return Ok(new { message = "Đã xóa gói membership", planId = planId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa", error = ex.Message });
            }
        }

    }
}
