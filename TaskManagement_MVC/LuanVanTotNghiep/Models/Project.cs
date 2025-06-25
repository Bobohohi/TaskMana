using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class ProjectItem
    {
        [JsonIgnore]
        public int? ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Descript { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
    }
}
