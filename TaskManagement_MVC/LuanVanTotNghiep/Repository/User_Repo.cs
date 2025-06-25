using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LuanVanTotNghiep.Repository
{
    public class User_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/User";
        private readonly string apiUrlFile = "https://localhost:7208/api/ImageAvatar/image";
        public async Task<UserItem> GetUserIdByEmail(string Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetUserIdByEmail?email=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserItem>(json);
            }
        }
        public async Task<UserItem> GetUserById(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetUserById?userId=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserItem>(json);
            }
        }
        public async Task<List<UserItem>> GetAllUser()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserItem>>(json);
            }
        }
        public async Task<string?> UploadAvatar(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using (var client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                await using var fileStream = file.OpenReadStream();
                content.Add(new StreamContent(fileStream), "file", file.FileName);

                var response = await client.PostAsync(apiUrlFile, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return null;

                var json = JObject.Parse(responseContent);
                return json["url"]?.ToString(); // secure_url được map thành "url" trong API trả về
            }
        }
        public async Task<bool> UpdateUser(UserItemUpdate user)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(apiUrl + "/UpdateUser", content);
                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> UpdatePictureUrlByUserIdAndPictureUrl(int userId, string pictureUrl)
        {
            using (var client = new HttpClient())
            {
                var requestUri = $"{apiUrl}/UpdatePictureUrl?userId={userId}&pictureUrl={Uri.EscapeDataString(pictureUrl)}";
                var response = await client.PutAsync(requestUri, null);
                return response.IsSuccessStatusCode;
            }
        }
    }
}

