using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly TaskManagementContext _context;

        public ProjectController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getProject()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Projects.Select(t => new
                {
                    ProjectId = t.ProjectId,
                    ProjectName = t.ProjectName,
                    Descript = t.Descript,
                    StartDate = t.StartDate,
                    EndDate = t.EndDate,
                    GroupId = t.GroupId,
                    CreatedAt = t.CreatedAt,
                    UserId = t.UserId,

                }).ToList();
                return Ok(ds);

            }
            catch
            {
                return BadRequest();
            }

        }
        [HttpGet("GetListProjectByGroupId/{groupId}")]
        public ActionResult<IEnumerable<Project>> GetListProjectByGroupId(int groupId)
        {
            var projects = (from p in _context.Projects
                            where p.GroupId == groupId
                            select p).ToList();
            return Ok(projects);
        }
        [HttpPost]
        public IActionResult addProject(Add_Project x)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                Project newP = new Project
                {
                    ProjectName = x.ProjectName,
                    Descript = x.Descript,
                    EndDate = x.EndDate,
                    StartDate = x.EndDate,
                    GroupId = x.GroupId,
                    CreatedAt = x.CreatedAt
                };
                db.Projects.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
