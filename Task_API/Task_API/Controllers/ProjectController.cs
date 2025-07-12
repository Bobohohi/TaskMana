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
        [HttpPut("UpdateStatus/{projectId}")]
        public IActionResult UpdateProjectStatus(int projectId, [FromBody] string newStatus)
        {
            try
            {
                var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

                if (project == null)
                {
                    return NotFound(new { message = "Project not found" });
                }

                project.Status = newStatus;
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Cập nhật trạng thái thành công",
                    projectId = project.ProjectId,
                    status = project.Status
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }
        [HttpPut("UpdateName/{projectId}")]
        public IActionResult UpdateProjectName(int projectId, [FromBody] string newName)
        {
            try
            {
                var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);
                if (project == null)
                {
                    return NotFound(new { message = "Project không tồn tại" });
                }

                project.ProjectName = newName;
                _context.SaveChanges();

                return Ok(new { message = "Đổi tên dự án thành công", projectId = project.ProjectId, projectName = project.ProjectName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }
        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject(int projectId)
        {
            try
            {
                var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

                if (project == null)
                {
                    return NotFound(new { message = "Project không tồn tại" });
                }

                // Xử lý xóa các dữ liệu liên kết nếu có (ví dụ Task, Report)
                //var tasks = _context.Tasks.Where(t => t.ProjectId == projectId).ToList();
                //if (tasks.Any())
                //{
                //    _context.Tasks.RemoveRange(tasks);
                //}

                //var reports = _context.Reports.Where(r => r.ProjectId == projectId).ToList();
                //if (reports.Any())
                //{
                //    _context.Reports.RemoveRange(reports);
                //}

                _context.Projects.Remove(project);
                _context.SaveChanges();

                return Ok(new { message = "Dự án đã được xóa thành công", projectId = projectId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi xóa dự án", error = ex.Message });
            }
        }
        [HttpPut("UpdateProject/{projectId}")]
        public IActionResult UpdateProject(int projectId, [FromBody] Project model)
        {
            try
            {
                var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

                if (project == null)
                {
                    return NotFound(new { message = "Project không tồn tại" });
                }

                project.ProjectName = model.ProjectName;
                project.Descript = model.Descript;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.GroupId = model.GroupId;
                project.CreatedAt = model.CreatedAt;
                project.UserId = model.UserId;

                _context.Projects.Update(project);
                _context.SaveChanges();

                return Ok(new { message = "Cập nhật dự án thành công", projectId = projectId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật dự án", error = ex.Message });
            }
        }
        [HttpGet("{projectId}")]
        public IActionResult GetProjectById(int projectId)
        {
            try
            {
                var project = _context.Projects.FirstOrDefault(p => p.ProjectId == projectId);

                if (project == null)
                {
                    return NotFound(new { message = "Không tìm thấy dự án." });
                }

                return Ok(project);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }


    }
}
