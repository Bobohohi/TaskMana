using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Task_API.Models
{
    public partial class Group
    {
        public Group()
        {
            GroupDetails = new HashSet<GroupDetail>();
            Projects = new HashSet<Project>();
        }

        public int GroupId { get; set; }
        public string? GroupName { get; set; }
        public int? GroupSize { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
        [JsonIgnore]
        public virtual ICollection<GroupDetail> GroupDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; }
    }
}
