namespace Task_API.MyModels
{
    public class Add_ProjectTask
    {
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public int? AssignedTo { get; set; }
        public int? ProjectId { get; set; }
        public int? UserId { get; set; }
        public int? BoardId { get; set; }
    }
}
