using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_API.Models;
using Task_API.MyModels;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly TaskManagementContext _context;
        public ReportController(TaskManagementContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult getReport()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.Reports.Select(t => new
                {
                    ReportId = t.ReportId,
                    ProjectId = t.ProjectId,
                    ReportName = t.ReportName,
                    Description = t.Description,
                    ReportType = t.ReportType,
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
        [HttpPost]
        public IActionResult addReport(Add_Report t)
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                Report newP = new Report
                {
                    ProjectId = t.ProjectId,
                    ReportName = t.ReportName,
                    Description = t.Description,
                    ReportType = t.ReportType,
                    CreatedAt = t.CreatedAt,
                    UserId = t.UserId,
                };
                db.Reports.Add(newP);
                db.SaveChanges();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("GetReportByProjectId")]
        public IActionResult GetReportByProjectId(int projectId)
        {
            try
            {
                var reports = _context.Reports
                    .Where(r => r.ProjectId == projectId)
                    .Select(t => new
                    {
                        ReportId = t.ReportId,
                        ProjectId = t.ProjectId,
                        ReportName = t.ReportName,
                        Description = t.Description,
                        ReportType = t.ReportType,
                        CreatedAt = t.CreatedAt,
                        UserId = t.UserId,
                    })
                    .ToList();

                if (!reports.Any())
                    return NotFound("Không có báo cáo nào cho project này.");

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return BadRequest("Lỗi khi lấy báo cáo: " + ex.Message);
            }
        }

    }
}
