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
    } 
}
