using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Text;

namespace LuanVanTotNghiep.Repository
{
    public class Login_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Login";

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl+ "/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("🔍 Response JSON: " + result);
                    return JsonConvert.DeserializeObject<LoginResponse>(result);
                }

                return null; 
            }
        }
        public async Task<bool> SendCode(string email)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(email);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl+"/sendcode", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> Register(RegisterRequest request)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl + "/register", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> ForgotPassword(string email)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(email);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl+ "/forgotpassword", content);
                return response.IsSuccessStatusCode;
            }
        }

    }
}
