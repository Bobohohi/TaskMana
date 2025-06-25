using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Task_API.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            GroupDetails = new HashSet<GroupDetail>();
            Groups = new HashSet<Group>();
            Notifications = new HashSet<Notification>();
            Projects = new HashSet<Project>();
            Reports = new HashSet<Report>();
            TaskAssignedToNavigations = new HashSet<Task>();
            TaskDetails = new HashSet<TaskDetail>();
            TaskFiles = new HashSet<TaskFile>();
            TaskHistories = new HashSet<TaskHistory>();
            TaskUsers = new HashSet<Task>();
            UserSubs = new HashSet<UserSub>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        [JsonIgnore]
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }

        public string? GoogleId { get; set; }

        public string? AuthProvider { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? Address { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }
        public string? PhoneNumber { get; set; }
        [JsonIgnore]
        public string? Role { get; set; }
        [JsonIgnore]
        public string? Token { get; set; }
        [JsonIgnore]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        public virtual ICollection<GroupDetail> GroupDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<Group> Groups { get; set; }
        [JsonIgnore]
        public virtual ICollection<Notification> Notifications { get; set; }
        [JsonIgnore]
        public virtual ICollection<Project> Projects { get; set; }
        [JsonIgnore]
        public virtual ICollection<Report> Reports { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> TaskAssignedToNavigations { get; set; }
        [JsonIgnore]
        public virtual ICollection<TaskDetail> TaskDetails { get; set; }
        [JsonIgnore]
        public virtual ICollection<TaskFile> TaskFiles { get; set; }
        [JsonIgnore]
        public virtual ICollection<TaskHistory> TaskHistories { get; set; }
        [JsonIgnore]
        public virtual ICollection<Task> TaskUsers { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserSub> UserSubs { get; set; }
    }
}
