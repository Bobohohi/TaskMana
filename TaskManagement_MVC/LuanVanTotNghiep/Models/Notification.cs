using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class NotificationItem
    {
        [JsonIgnore]
        public int NotificationId { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
