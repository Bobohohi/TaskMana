using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Task_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageAvatarController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _cloudName = "dp69yuu6v"; // Thay bằng Cloudinary cloud_name của bạn
        private readonly string _uploadPreset = "UploadAvatar"; // Phải là unsigned preset

        public ImageAvatarController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadToCloudinary(IFormFile file)
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
                $"https://api.cloudinary.com/v1_1/{_cloudName}/image/upload",
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
    }
}
