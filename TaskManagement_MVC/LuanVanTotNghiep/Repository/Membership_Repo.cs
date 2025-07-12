using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class Membership_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Membership";

        public async Task<List<MembershipItem>> GetAllMembership()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<MembershipItem>>(json);
            }
        }
        public async Task<bool> DeleteMembership(int planId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{planId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateMembership(int planId, MembershipItem updatedPlan)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedPlan);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{planId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<MembershipItem> GetMembershipById(int planId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{planId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<MembershipItem>(json);
            }
        }

        public async Task<bool> CreateMembership(MembershipItem model)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }

    }
}
