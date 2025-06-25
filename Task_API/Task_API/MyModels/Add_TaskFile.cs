namespace Task_API.MyModels
{
    public class Add_TaskFile
    {
        public int? TaskId { get; set; }
        public string? FileName { get; set; }
        public string? FileType { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadedAt { get; set; }
        public int? UserId { get; set; }
    }
}
