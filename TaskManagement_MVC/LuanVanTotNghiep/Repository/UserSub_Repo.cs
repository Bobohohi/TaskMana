using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LuanVanTotNghiep.Repository
{
    public class UserSub_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/UserSub";
        private class SubscriptionCheckResponse
        {
            public bool isValid { get; set; }
        }
        public async Task<bool> CheckValidSubscription(int userId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/CheckValidSubscription?userId=" + userId);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SubscriptionCheckResponse>(json);
                return result?.isValid ?? false;
            }
        }
        public async Task<bool> CreateUserSub(UserSubItem newG)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newG);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }

    }
} 