using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? TaskId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Status { get; set; }
        public virtual Task? Task { get; set; }
        public virtual User? User { get; set; }
    }
}
