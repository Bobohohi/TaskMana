namespace Task_API.MyModels
{
    public class Add_Comment
    {
        public int? TaskId { get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
