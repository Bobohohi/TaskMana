namespace LuanVanTotNghiep.Models
{
    public class UserSubItem
    {
        public int? UserId { get; set; }
        public int? PlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
