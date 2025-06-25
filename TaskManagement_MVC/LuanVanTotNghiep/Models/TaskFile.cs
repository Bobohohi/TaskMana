using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Models
{
    public class TaskFileItem
    {
        public int FileId { get; set; }
        public int? TaskId { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadedAt { get; set; }
        public int? UserId { get; set; }
    }
    public class TaskFileUrl
    {
        [JsonProperty("url")]
        public string FileUrl { get; set; }
    }
    public class TaskFileInput
    {
        public int? TaskId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn file.")]
        public IFormFile FileInput { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public DateTime? UploadedAt { get; set; }
        public int? UserId { get; set; }
    }
}
