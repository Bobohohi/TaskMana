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
        [HttpDelete("{reportId}")]
        public IActionResult DeleteReport(int reportId)
        {
            try
            {
                var report = _context.Reports.FirstOrDefault(r => r.ReportId == reportId);

                if (report == null)
                {
                    return NotFound(new { message = "Report not found" });
                }

                _context.Reports.Remove(report);
                _context.SaveChanges();

                return Ok(new { message = "Report deleted successfully" });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Không thể xóa báo cáo do ràng buộc dữ liệu", error = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi không xác định khi xóa báo cáo", error = ex.Message });
            }
        }
        [HttpPut("{reportId}")]
        public IActionResult UpdateReport(int reportId, [FromBody] Report model)
        {
            try
            {
                var report = _context.Reports.FirstOrDefault(r => r.ReportId == reportId);

                if (report == null)
                {
                    return NotFound(new { message = "Report not found" });
                }

                // Gán lại dữ liệu từ model
                report.ProjectId = model.ProjectId;
                report.ReportName = model.ReportName;
                report.Description = model.Description;
                report.ReportType = model.ReportType;
                report.CreatedAt = model.CreatedAt;
                report.UserId = model.UserId;

                _context.Reports.Update(report);
                _context.SaveChanges();

                return Ok(new { message = "Report updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Lỗi khi cập nhật báo cáo", error = ex.Message });
            }
        }
        [HttpGet("{reportId}")]
        public IActionResult GetReportById(int reportId)
        {
            try
            {
                var report = _context.Reports
                    .Where(r => r.ReportId == reportId)
                    .Select(r => new
                    {
                        ReportId = r.ReportId,
                        ProjectId = r.ProjectId,
                        ReportName = r.ReportName,
                        Description = r.Description,
                        ReportType = r.ReportType,
                        CreatedAt = r.CreatedAt,
                        UserId = r.UserId,
                    })
                    .FirstOrDefault();

                if (report == null)
                {
                    return NotFound(new { message = "Không tìm thấy báo cáo." });
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi hệ thống", error = ex.Message });
            }
        }

    }
}
