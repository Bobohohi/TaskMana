using System;
using System.Collections.Generic;

namespace Task_API.Models
{
    public partial class GroupDetail
    {
        public int GroupDetailId { get; set; }
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public string? RoleInGroup { get; set; }

        public virtual Group? Group { get; set; }
        public virtual User? User { get; set; }
    }
}
