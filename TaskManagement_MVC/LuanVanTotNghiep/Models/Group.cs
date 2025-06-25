using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class GroupItem
    {
        [JsonIgnore]
        public int? GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? GroupSize { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
    }
}
