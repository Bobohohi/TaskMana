using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class Report_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Report";
        public async Task<List<ReportItem>> GetAllReportByProjectId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetReportByProjectId?projectId=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ReportItem>>(json);
            }
        }
        public async Task<List<ReportItem>> GetAllReport()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<ReportItem>>(json);
            }
        }
        public async Task<bool> CreateReport(ReportItem newB)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newB);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteReport(int reportId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{reportId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateReport(int reportId, ReportItem updatedReport)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedReport);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{reportId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<ReportItem> GetReportById(int reportId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{reportId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ReportItem>(json);
            }
        }

    }
}
