using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using System.Text;

namespace LuanVanTotNghiep.Repository
{
    public class TaskFile_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/TaskFile";
        private readonly string apiDictUrl = "https://localhost:7208/api/FileTask/file";
        public async Task<bool> CreateTaskFile(TaskFileItem newTask)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<TaskFileUrl> GetTaskFileUrl(IFormFile file)
        {
            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                using (var ms = new MemoryStream())
                {
                    await file.CopyToAsync(ms);
                    ms.Position = 0;

                    var fileContent = new ByteArrayContent(ms.ToArray());
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    content.Add(fileContent, "file", file.FileName);

                    var response = await client.PostAsync(apiDictUrl, content);
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TaskFileUrl>(json);
                }
            }
        }
        public async Task<List<TaskFileItem>> GetAllFile()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskFileItem>>(json);
            }
        }
        public async Task<List<TaskFileItem>> GetAllTaskFileByTaskId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetTaskFileByTaskId?taskId=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskFileItem>>(json);
            }
        }
        public async Task<bool> DeleteTaskFile(int fileId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{fileId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateTaskFile(int fileId, TaskFileItem updatedFile)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedFile);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{fileId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<TaskFileItem> GetTaskFileById(int fileId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{fileId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TaskFileItem>(json);
            }
        }

    }
}
