namespace LuanVanTotNghiep.Models
{
    public class MembershipItem
    {
        public int PlanId { get; set; }
        public string? PlanName { get; set; }
        public int? DurationInDays { get; set; }
        public int? MaxGroups { get; set; }
        public int? MaxGroupMember { get; set; }
        public decimal? Price { get; set; }
    }
}
