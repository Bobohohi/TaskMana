using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class Report
    {
        public int ReportId { get; set; }
        public int? ProjectId { get; set; }
        public string? ReportName { get; set; }
        public string? Description { get; set; }
        public string? ReportType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }

        public virtual Project? Project { get; set; }
        public virtual User? User { get; set; }
    }
}
