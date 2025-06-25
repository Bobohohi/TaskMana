namespace Task_API.MyModels
{
    public class Add_User
    {
        public string Email { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }
        public string? GoogleId { get; set; }
        public string? AuthProvider { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
