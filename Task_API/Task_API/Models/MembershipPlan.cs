using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class MembershipPlan
    {
        public MembershipPlan()
        {
            UserSubs = new HashSet<UserSub>();
        }

        public int PlanId { get; set; }
        public string? PlanName { get; set; }
        public int? DurationInDays { get; set; }
        public int? MaxGroups { get; set; }
        public int? MaxGroupMember { get; set; }
        public decimal? Price { get; set; }

        public virtual ICollection<UserSub> UserSubs { get; set; }
    }
}
