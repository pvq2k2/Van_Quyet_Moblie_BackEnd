using Azure;
using System.Globalization;
using System.Text.RegularExpressions;
using Van_Quyet_Moblie_BackEnd.Handle.Request.AuthRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.OrderRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.ProductRequest;
using Van_Quyet_Moblie_BackEnd.Handle.Request.VoucherRequest;
using Van_Quyet_Moblie_BackEnd.Middleware;

namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class InputHelper
    {
        public static bool IsImage(IFormFile imageFile, int maxSizeInBytes = (2 * 1024 * 768))
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new Exception("Không có ảnh nào được chọn !");
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("File này không phải file có định dạng ảnh");
            }

            if (imageFile.Length > maxSizeInBytes)
            {
                throw new Exception("Kích thước file quá lớn");
            }

            var image = Image.Load<Rgba32>(imageFile.OpenReadStream());
            if (image.Width < 0 || image.Height < 0)
            {
                throw new Exception("Ảnh không phù hợp !");
            }

            return true;
        }

        public static bool RegisterValidate(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName)
               || string.IsNullOrWhiteSpace(request.Password)
               || string.IsNullOrWhiteSpace(request.FullName)
               || string.IsNullOrWhiteSpace(request.Email)
               || string.IsNullOrWhiteSpace(request.NumberPhone))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Bạn cần truyền vào đầy đủ thông tin !");
            }
            if (CheckLengthOfCharacters(request.FullName))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Họ và tên phải nhỏ hơn 20 ký tự !");
            }
            if (CheckWordCount(request.FullName))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Họ và tên phải có trên 2 từ !");
            }
            if (!RegexUserName(request.UserName))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Tên tài khoản không được chứa chữ hoa, dấu cách và ký tự đặc biệt !");
            }
            if (!RegexPassword(request.Password))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Mật khẩu phải có chữ hoa, chữ thường, chữ số và kí tự đặc biệt !");
            }
            if (!RegexEmail(request.Email))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Không đúng định dạng email !");
            }
            if (!RegexPhoneNumber(request.NumberPhone))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Không đúng định dạng số điện thoại !");
            }
            if (request.Gender != 1 && request.Gender != 2)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giới tính không phù hợp !");
            }
            return true;
        }

        public static bool VoucherValidate(VoucherRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code) || string.IsNullOrWhiteSpace(request.Title))
            {
                throw new Exception("Vui lòng nhập đủ thông tin !");
            }
            if (request.DiscountPercentage < 1)
            {
                throw new Exception("Số phần trăm không được nhỏ hơn 1 !");
            }
            if (request.MinimumPurchaseAmount < 0)
            {
                throw new Exception("Số tiền tối thiểu không phải lả số âm !");
            }
            return true;
        }

        public static bool ChangeInformationValidate(ChangeInformationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName)
               || string.IsNullOrWhiteSpace(request.Email)
               || string.IsNullOrWhiteSpace(request.NumberPhone)
               || string.IsNullOrWhiteSpace(request.Address))
            {
                throw new Exception("Bạn cần truyền vào đầy đủ thông tin !");
            }
            if (CheckLengthOfCharacters(request.FullName))
            {
                throw new Exception("Họ và tên phải nhỏ hơn 20 ký tự !");
            }
            if (CheckWordCount(request.FullName))
            {
                throw new Exception("Họ và tên phải có trên 2 từ !");
            }
            if (!RegexEmail(request.Email))
            {
                throw new Exception("Không đúng định dạng email !");
            }
            if (!RegexPhoneNumber(request.NumberPhone))
            {
                throw new Exception("Không đúng định dạng số điện thoại !");
            }
            if (request.Gender != 1 || request.Gender != 2)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Giới tính không phù hợp !");
            }
            return true;
        }


        public static bool CheckOutValidate(CheckOutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName)
               || string.IsNullOrWhiteSpace(request.Email)
               || string.IsNullOrWhiteSpace(request.Phone)
               || string.IsNullOrWhiteSpace(request.Address))
            {
                throw new Exception("Bạn cần truyền vào đầy đủ thông tin !");
            }
            if (CheckLengthOfCharacters(request.FullName))
            {
                throw new Exception("Họ và tên phải nhỏ hơn 20 ký tự !");
            }
            if (CheckWordCount(request.FullName))
            {
                throw new Exception("Họ và tên phải có trên 2 từ !");
            }
            if (!RegexEmail(request.Email))
            {
                throw new Exception("Không đúng định dạng email !");
            }
            if (!RegexPhoneNumber(request.Phone))
            {
                throw new Exception("Không đúng định dạng số điện thoại !");
            }
            return true;
        }

        public static bool UpdateOrderValidate(UpdateOrderRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName)
               || string.IsNullOrWhiteSpace(request.Email)
               || string.IsNullOrWhiteSpace(request.Phone)
               || string.IsNullOrWhiteSpace(request.Address))
            {
                throw new Exception("Bạn cần truyền vào đầy đủ thông tin !");
            }
            if (CheckLengthOfCharacters(request.FullName))
            {
                throw new Exception("Họ và tên phải nhỏ hơn 20 ký tự !");
            }
            if (CheckWordCount(request.FullName))
            {
                throw new Exception("Họ và tên phải có trên 2 từ !");
            }
            if (!RegexEmail(request.Email))
            {
                throw new Exception("Không đúng định dạng email !");
            }
            if (!RegexPhoneNumber(request.Phone))
            {
                throw new Exception("Không đúng định dạng số điện thoại !");
            }
            return true;
        }


        public static bool CreateProductValidate(CreateProductRequest request)
        {
            if (request.Height < 1 || request.Width < 1 || request.Length < 1 || request.Weight < 1)
            {
                throw new Exception("Chiều cao, rộng, dài và cân nặng phải lớn hơn 0 !");
            }
            if (string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.Title))
            {
                throw new Exception("Vui lòng nhập đầy đủ thông tin !");
            }
            if (request.Price < 0)
            {
                throw new Exception("Vui lòng nhập giá tiền lớn hơn !");
            }
            if (request.Discount != null && request.Discount < 0)
            {
                throw new Exception("Vui lòng nhập giảm giá lớn hơn !");
            }

            return true;
        }

        public static bool UpdateProductValidate(UpdateProductRequest request)
        {
            if (request.Height < 1 || request.Width < 1 || request.Length < 1 || request.Weight < 1)
            {
                throw new Exception("Chiều cao, rộng, dài và cân nặng phải lớn hơn 0 !");
            }
            if (string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.Title))
            {
                throw new Exception("Vui lòng nhập đầy đủ thông tin !");
            }
            if (request.Price < 0)
            {
                throw new Exception("Vui lòng nhập giá tiền lớn hơn !");
            }
            if (request.Discount != null && request.Discount < 0)
            {
                throw new Exception("Vui lòng nhập giảm giá lớn hơn !");
            }

            return true;
        }

        public static bool CheckLengthOfCharacters(string fullName)
        {
            return fullName.Length > 20;
        }
        public static bool CheckWordCount(string fullName)
        {
            return fullName.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length < 2;
        }
        public static string NormalizeName(string fullName)
        {
            fullName = fullName.Trim();

            fullName = Regex.Replace(fullName, @"\p{P}", " ").Trim();
            fullName = Regex.Replace(fullName, @"\s+", " ");

            TextInfo textInfo = new CultureInfo("vi-VN", false).TextInfo;
            return textInfo.ToTitleCase(fullName.ToLower());
        }
        public static string CreateSlug(string name)
        {
            return string.Join("-", name.Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToLower();
        }


        public static bool RegexUserName(string userName)
        {
            string pattern = @"^[a-z0-9_]+$";

            return Regex.IsMatch(userName, pattern);
        }

        public static bool RegexPassword(string password)
        {
            string pattern = @"^(?=.*\d)(?=.*[\W_])[A-Za-z\d\W_]+$";

            return Regex.IsMatch(password, pattern);
        }

        public static bool RegexEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            return Regex.IsMatch(email, pattern);
        }

        public static bool RegexPhoneNumber(string phoneNumber)
        {
            string pattern = @"^(?:\+84|0)(?:3[2-9]|5[689]|7[06-9]|8[1-9]|9[0-9])[0-9]{7}$";

            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
