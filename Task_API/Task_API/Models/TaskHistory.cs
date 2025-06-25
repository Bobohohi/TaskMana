using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class TaskHistory
    {
        public int HistoryId { get; set; }
        public int? TaskId { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string? Action { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Task? Task { get; set; }
        public virtual User? UpdatedByNavigation { get; set; }
    }
}
