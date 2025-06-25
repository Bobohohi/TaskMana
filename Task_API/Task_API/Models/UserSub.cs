using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class UserSub
    {
        public int SubscriptionId { get; set; }
        public int? UserId { get; set; }
        public int? PlanId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }

        public virtual MembershipPlan? Plan { get; set; }
        public virtual User? User { get; set; }
    }
}
