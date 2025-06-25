namespace LuanVanTotNghiep.Models
{
    public class TaskDetailItem
    {
        public int TaskDetailId { get; set; }
        public int? UserId { get; set; }
        public int? TaskId { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
    }
    public class TaskDetailItemInput
    {
        public int? UserId { get; set; }
        public int? TaskId { get; set; }
    }


}
