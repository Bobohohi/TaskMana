using System.Text.RegularExpressions;

namespace LuanVanTotNghiep.Models
{
    public class GroupDetailItem
    {
        public int? GroupId { get; set; }
        public int? UserId { get; set; }
        public string? RoleInGroup { get; set; }
    }
    public class AddMemberViewModel
    {
        public int GroupId { get; set; }
        public string Email { get; set; }
        public string RoleInGroup { get; set; }

    }
    public class MemberViewModel
    {
        public int? UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleInGroup { get; set; }

    }
    public class RoleResult
    {
        public string RoleInGroup { get; set; }
    }
}
