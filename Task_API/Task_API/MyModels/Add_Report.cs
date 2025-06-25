namespace Task_API.MyModels
{
    public class Add_Report
    {
        public int? ProjectId { get; set; }
        public string? ReportName { get; set; }
        public string? Description { get; set; }
        public string? ReportType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
    }
}
