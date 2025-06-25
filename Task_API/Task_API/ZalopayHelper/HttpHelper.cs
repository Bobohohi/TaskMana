
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace API_HOTEL.Helpers
{
    public class HttpHelper
    {
        public static async Task<Dictionary<string, string>> PostFormAsync(string url, Dictionary<string, string> data)
        {
            using (var client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(data);
                var response = await client.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseString);
            }
        }
    }
}
