using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class GroupDetail_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/GroupDetail";
        public async Task<bool> CreateGroupDetail(GroupDetailItem newB)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newB);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<GroupItem>> GetAllGroupByUserId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetAllGroupByUserId/" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<GroupItem>>(json);
            }
        }

        public async Task<List<MemberViewModel>> GetAllUserByGroupId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetAllUserByGroupId/" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<MemberViewModel>>(json);
            }
        }
        public async Task<string?> GetRoleByGroupAndUser(int groupId, int userId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/GetRoleByGroupAndUser/{groupId}/{userId}");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<RoleResult>(json);
                return result?.RoleInGroup;
            }
        }

    }
}
