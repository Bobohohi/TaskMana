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
    }
}
