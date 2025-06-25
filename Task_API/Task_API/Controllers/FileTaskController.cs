using System.Net.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task_API.Models;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileTaskController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _cloudName = "dp69yuu6v"; // Thay bằng Cloudinary cloud_name của bạn
        private readonly string _uploadPreset = "UploadFile"; // Phải là unsigned preset

        public FileTaskController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpPost("file")]
        public async Task<IActionResult> UploadFileToCloudinary(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is missing.");

            var httpClient = _httpClientFactory.CreateClient();

            using var content = new MultipartFormDataContent();
            await using var fileStream = file.OpenReadStream();

            content.Add(new StreamContent(fileStream), "file", file.FileName);

            var uploadPresetContent = new StringContent(_uploadPreset);
            uploadPresetContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
            {
                Name = "\"upload_preset\""
            };
            content.Add(uploadPresetContent);

            var response = await httpClient.PostAsync(
                $"https://api.cloudinary.com/v1_1/{_cloudName}/raw/upload",
                content
            );

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new
                {
                    status = response.StatusCode,
                    error = responseContent
                });
            }

            var json = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var secureUrl = json.GetProperty("secure_url").GetString();

            return Ok(new { url = secureUrl });
        }
        [HttpGet]
        public IActionResult getFiletask()
        {
            try
            {
                TaskManagementContext db = new TaskManagementContext();
                var ds = db.TaskFiles.Select(t => new
                {
                    FileId = t.FileId,
                    TaskId = t.TaskId,
                    FileName = t.FileName,
                    FilePath = t.FilePath,
                    UploadedAt = t.UploadedAt,
                    UserId = t.UserId,

                }).ToList();
                return Ok(ds);

            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
