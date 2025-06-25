using LuanVanTotNghiep.Models;
using Newtonsoft.Json;
using System.Text;

namespace LuanVanTotNghiep.Repository
{
    public class Board_Repo
    {
        private readonly string apiUrl = "https://localhost:7208/api/Board";

        public async Task<List<BoardItem>> GetAllBoard()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<BoardItem>>(json);
            }
        }
        public async Task<bool> CreateBoard(BoardItem newG)
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(newG);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                return response.IsSuccessStatusCode;
            }
        }
    
        public async Task<List<BoardItem>> GetAllBoardByProjectId(int Gid)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl+"/GetListBoardByProjectId/"+Gid);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<BoardItem>>(json);
            }
        }
    }
}
