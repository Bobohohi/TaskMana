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

    }
}
