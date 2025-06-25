namespace LuanVanTotNghiep.Models
{
    public class UserItem
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }
        public string? GoogleId { get; set; }
        public string? AuthProvider { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }

    }
    public class UserItemUpdate
    {
        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Sex { get; set; }
        public DateTime? BirthDay { get; set; }

    }
}
