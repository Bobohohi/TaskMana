using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class CommentItem
    {
        public int CommentId { get; set; }
        public int? TaskId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public virtual TaskItem Task { get; set; }
    }
    public class CommentItemInput
    {
        public int? TaskId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
}
