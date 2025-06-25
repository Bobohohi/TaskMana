using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Task_API.Models
{
    public partial class Task
    {
        public Task()
        {
            Comments = new HashSet<Comment>();
            TaskDetails = new HashSet<TaskDetail>();
            TaskFiles = new HashSet<TaskFile>();
            TaskHistories = new HashSet<TaskHistory>();
        } 
        public int TaskId { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }

        public string? Status { get; set; }
        public int? AssignedTo { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public int? BoardId { get; set; }
        public bool? IsDaily { get; set; } = false;
        public DateTime? FlagDaily { get; set; }
        [JsonIgnore]
        public virtual User? AssignedToNavigation { get; set; }
        [JsonIgnore]
        public virtual Board? Board { get; set; }
        [JsonIgnore]
        public virtual Project? Project { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<TaskDetail> TaskDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<TaskFile> TaskFiles { get; set; }
        [JsonIgnore]
        public virtual ICollection<TaskHistory> TaskHistories { get; set; }
    }
}
