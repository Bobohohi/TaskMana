
using System.Security.Cryptography;
using System.Text;

namespace API_HOTEL.Helpers
{
    public class HmacHelper
    {
        public static string Compute(ZaloPayHMAC mode, string key, string data)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }

    public enum ZaloPayHMAC
    {
        HMACSHA256
    }
}
