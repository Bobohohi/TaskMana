using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class Project
    {
        public Project()
        {
            Boards = new HashSet<Board>();
            Reports = new HashSet<Report>();
            Tasks = new HashSet<Task>();
        }

        public int ProjectId { get; set; }
        public string? ProjectName { get; set; }
        public string? Descript { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? GroupId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public virtual Group? Group { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<Board> Boards { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
