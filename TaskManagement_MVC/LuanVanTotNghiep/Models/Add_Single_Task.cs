namespace LuanVanTotNghiep.Models
{
    public class Add_Single_Task
    {
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public int AssignedTo { get; set; }
        public int UserId { get; set; }
        public bool? IsDaily { get; set; }
    }
}
