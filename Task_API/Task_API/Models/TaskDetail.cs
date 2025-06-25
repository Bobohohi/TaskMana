using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class TaskDetail
    {
        public int TaskDetailId { get; set; }
        public int? UserId { get; set; }
        public int? TaskId { get; set; }

        public virtual Task? Task { get; set; }
        public virtual User? User { get; set; }
    }
}
