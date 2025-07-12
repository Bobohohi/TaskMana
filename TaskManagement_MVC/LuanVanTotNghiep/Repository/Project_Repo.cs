using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class Project_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Project";

        public async Task<List<ProjectItem>> GetAllProjectByGroupId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl+"/GetListProjectByGroupId/"+Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProjectItem>>(json);
            }
        }
        public async Task<bool> CreateProject(ProjectItem newB)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newB);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<ProjectItem>> GetAllProject()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ProjectItem>>(json);
            }
        }
        public async Task<bool> UpdateProjectStatus(int projectId, string newStatus)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(newStatus), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{apiUrl}/UpdateStatus/{projectId}", content);
                return response.IsSuccessStatusCode;
            }
        }


        public async Task<bool> UpdateProjectName(int projectId, string newName)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(newName), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"{apiUrl}/UpdateName/{projectId}", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteProject(int projectId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{projectId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateProject(int projectId, ProjectItem updatedProject)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedProject);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/UpdateProject/{projectId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<ProjectItem> GetProjectById(int projectId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{projectId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProjectItem>(json);
            }
        }

    }
}
