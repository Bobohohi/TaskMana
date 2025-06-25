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

    }
}
