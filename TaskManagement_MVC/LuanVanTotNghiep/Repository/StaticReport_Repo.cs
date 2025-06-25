using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class StaticReport_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/User";

        public async Task<StaticReportItem?> GetUserStatsByIdAsync(int userId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/CountByUserId/{userId}");

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StaticReportItem>(json);
            }
        }
    }
}
