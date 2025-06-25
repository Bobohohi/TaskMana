using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class TaskFile
    {
        public int FileId { get; set; }
        public int? TaskId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadedAt { get; set; }
        public int? UserId { get; set; }

        public virtual Task? Task { get; set; }
        public virtual User? User { get; set; }
    }
}
