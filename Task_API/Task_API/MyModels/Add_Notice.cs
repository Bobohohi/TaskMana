namespace Task_API.MyModels
{
    public class Add_Notice
    {
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
