using System.Security.Cryptography;
using System.Text;

namespace Libs
{
    public class FuncUtils
    {
        public static string HashPassword(string password, string secretKey)
        {
            // Chuyển secret key từ chuỗi sang mảng byte
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            // Tạo đối tượng HMAC-SHA256 với secret key
            using (HMACSHA256 hmac = new HMACSHA256(keyBytes))
            {
                // Chuyển password từ chuỗi sang mảng byte
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Thực hiện băm với secret key
                byte[] hashBytes = hmac.ComputeHash(passwordBytes);

                // Chuyển đổi mảng byte sau khi băm thành chuỗi hexadecimal
                StringBuilder hashString = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashString.Append(b.ToString("x2"));
                }

                return hashString.ToString();
            }
        }
    }
}
