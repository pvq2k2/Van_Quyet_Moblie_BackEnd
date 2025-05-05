using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json;
using System.Text;
namespace Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            try
            {
                var userRequest = new Users() { Email = email, FullName = fullName, Password = password };
                // Tạo một đối tượng HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Địa chỉ API mà bạn muốn gọi
                    string url = "https://localhost:7202/api/auth/register";

                    // Chuyển đổi dữ liệu thành JSON
                    string jsonData = JsonConvert.SerializeObject(userRequest);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Gọi API với phương thức POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Kiểm tra kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("API response: " + result);
                        //TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = result;
                        return RedirectToAction("Register");
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Register");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var userRequest = new Users() { Email = email, Password = password };
                // Tạo một đối tượng HttpClient
                using (HttpClient client = new HttpClient())
                {
                    // Địa chỉ API mà bạn muốn gọi
                    string url = "https://localhost:7202/api/auth/login";

                    // Chuyển đổi dữ liệu thành JSON
                    string jsonData = JsonConvert.SerializeObject(userRequest);
                    StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Gọi API với phương thức POST
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // Kiểm tra kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();

                        //// Chuyển đổi JSON response thành đối tượng C#
                        //var user = JsonConvert.DeserializeObject<Users>(result);

                        // Chuyển đổi JSON response thành dynamic object
                        //dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(result)!;
                        //string userJson = JsonConvert.SerializeObject(jsonObject);
                        //string userJson = JsonConvert.SerializeObject(result);
                        SessionExtensions.SetString(HttpContext.Session, "user", result);
                        //TempData["SuccessMessage"] = "Đăng nhập thành công!";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        TempData["ErrorMessage"] = result;
                        return RedirectToAction("Login");
                    }
                }

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Login");
            }
        }

        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Logout()
        {
            // Xóa session
            HttpContext.Session.Remove("user");

            // Hoặc xóa tất cả các dữ liệu trong session
            // HttpContext.Session.Clear();

            TempData.Clear();

            // Chuyển hướng đến trang đăng nhập hoặc trang chủ
            return RedirectToAction("Login", "Auth"); // Thay đổi theo controller và action của bạn
        }
    }
}
