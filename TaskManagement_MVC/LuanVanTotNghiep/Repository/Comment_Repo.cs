using System.Text;
using LuanVanTotNghiep.Models;
using Newtonsoft.Json;

namespace LuanVanTotNghiep.Repository
{
    public class Comment_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Comment";
        public async Task<List<CommentItem>> GetAllCommentByTaskId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl + "/GetCommentByTaskId?taskId=" + Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CommentItem>>(json);
            }
        }

        public async Task<bool> CreateComment(CommentItemInput newG)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newG);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<List<CommentItem>> GetAllComments()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CommentItem>>(json);
            }
        }
        public async Task<bool> DeleteComment(int commentId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.DeleteAsync($"{apiUrl}/{commentId}");
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<bool> UpdateComment(int commentId, CommentItem updatedComment)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(updatedComment);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"{apiUrl}/{commentId}", content);
                return response.IsSuccessStatusCode;
            }
        }
        public async Task<CommentItem> GetCommentById(int commentId)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{apiUrl}/{commentId}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CommentItem>(json);
            }
        }


    }
}
