using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class TaskDetail_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/TaskDetail";
        public async Task<List<TaskDetailItem>> GetAllUserByTaskId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetUserByTaskId?taskId=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskDetailItem>>(json);
            }
        }
        public async Task<bool> AddUserToTask(TaskDetailItemInput newG)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newG);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<TaskItem>> GetAllTaskByUserId(int userId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/GetAllTaskByUserId/{userId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskItem>>(json);
            }
        }

    }
}
