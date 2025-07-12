using LuanVanTotNghiep.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace LuanVanTotNghiep.Repository
{
    public class Group_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Group";

        public async Task<List<GroupItem>> GetAllGroups()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GroupItem>>(json);
            }
        }
        public async Task<bool> CreateGroup(GroupItem newG)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newG);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<GroupItem>> GetAllGroupByUserId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetListGroupByUserId/" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GroupItem>>(json);
            }
        }
        public async Task<GroupItem> GetGroupById(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl+"/"+id);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GroupItem>(json);
            }
        }
        public async Task<bool> UpdateGroupStatus(int groupId, string newStatus)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(newStatus), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/UpdateStatus/{groupId}", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdateGroupName(int groupId, string newName)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(newName), Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/UpdateName/{groupId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> DeleteGroup(int groupId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{groupId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateGroup(int groupId, GroupItem updatedGroup)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedGroup);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/UpdateGroup/{groupId}", content);
                return response.IsSuccessStatusCode;
            }
        }

    }
}
