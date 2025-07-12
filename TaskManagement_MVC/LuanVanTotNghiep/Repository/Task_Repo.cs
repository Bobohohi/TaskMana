using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LuanVanTotNghiep.Repository
{
    public class Task_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Task";

        public async Task<List<TaskItem>> GetAllTasks()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskItem>>(json);
            }
        }
        public async Task<bool> CreateTask(Add_Single_Task newTask)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> CreateTasks(TaskItem newTask)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl + "/addTasks", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<TaskItem>> GetAllTaskByUserId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetListTaskByUserId/" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskItem>>(json);
            }
        }
        public async Task<List<TaskItem>> GetAllTaskByProjectId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetListTaskByProjectId/" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TaskItem>>(json);
            }
        }
        public async Task<TaskItem> GetTaskByTaskId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl+'/'+ Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TaskItem>(json);
            }
        }
        public async Task<bool> ResetDailyTasks()
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(apiUrl+"/ResetDailyTasks", null);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateTaskStatus_Query(int taskId, string newStatus)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, apiUrl+ "/UpdateStatus?taskId="+taskId+"&newStatus="+newStatus);
                var response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> DeleteTask(int taskId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{taskId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateTask(int taskId, TaskItem updatedTask)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedTask);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{taskId}", content);
                return response.IsSuccessStatusCode;
            }
        }

    }
}


