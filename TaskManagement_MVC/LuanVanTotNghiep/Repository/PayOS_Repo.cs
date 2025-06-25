using System.Net.Http;
using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class PayOS_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/PayOS/create";

        public async Task<string> CreatePaymentLink(PayOSItem item)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();
                return result; // Đây là chuỗi checkoutUrl
            }
        }
    }
}
