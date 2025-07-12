using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using System.Text;

namespace LuanVanTotNghiep.Repository
{
    public class Notification_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Notice";
        public async Task<List<NotificationItem>> GetAllNotice()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<NotificationItem>>(json);
            }
        }
        public async Task<bool> CreateNotice(NotificationItem newG)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newG);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<NotificationItem>> GetAllNoticeByUserId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/UserId?userId=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<NotificationItem>>(json);
            }
        }
        public async Task<bool> UpdateIsRead(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"{apiUrl}/UpdateIsRead/{id}", null);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> CreateNoticeList(List<NotificationItem> noticeList)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(noticeList);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl + "/addListNotice", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> DeleteNotice(int notificationId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{notificationId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateNotice(int notificationId, NotificationItem updatedNotice)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedNotice);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{notificationId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<NotificationItem> GetNoticeById(int notificationId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{notificationId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<NotificationItem>(json);
            }
        }
    }
}
