using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class ReportItem
    {
        [JsonIgnore]
        public int ReportId { get; set; }
        public int? ProjectId { get; set; }
        public string? ReportName { get; set; }
        public string? Description { get; set; }
        public string? ReportType { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
    }
}
