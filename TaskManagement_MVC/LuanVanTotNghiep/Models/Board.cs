using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class BoardItem
    {
        [JsonIgnore]
        public int? BoardId { get; set; }
        public int? ProjectId { get; set; }
        public string? BoardName { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
