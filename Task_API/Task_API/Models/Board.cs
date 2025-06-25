using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Task_API.Models
{
    public partial class Board
    {
        public Board()
        {
            Tasks = new HashSet<Task>();
        }

        public int? BoardId { get; set; }
        public int? ProjectId { get; set; }
        public string? BoardName { get; set; }
        public DateTime? CreatedAt { get; set; }
        [JsonIgnore]
        public virtual Project? Project { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
