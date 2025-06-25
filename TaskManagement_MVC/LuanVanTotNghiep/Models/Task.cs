using System.Text.Json.Serialization;

namespace LuanVanTotNghiep.Models
{
    public class TaskItem
    {
        [JsonIgnore]
        public int TaskId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public int? AssignedTo { get; set; }
        public int? ProjectId { get; set; }
        public int UserId { get; set; } 
        public int? BoardId { get; set; }
        public bool? IsDaily { get; set; }
        public virtual ICollection<CommentItem> Comments { get; set; }
    }

}
