using System.Globalization;
using System.Text;
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
                throw new CustomException(StatusCodes.Status400BadRequest, "Không có ảnh nào được chọn !");
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "File này không phải file có định dạng ảnh");
            }

            if (imageFile.Length > maxSizeInBytes)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Kích thước file quá lớn");
            }

            var image = Image.Load<Rgba32>(imageFile.OpenReadStream());
            if (image.Width < 0 || image.Height < 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Ảnh không phù hợp !");
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
                throw new CustomException(StatusCodes.Status400BadRequest, "Chiều cao, rộng, dài và cân nặng phải lớn hơn 0 !");
            }
            if (string.IsNullOrWhiteSpace(request.Name)
                || string.IsNullOrWhiteSpace(request.Description))
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Vui lòng nhập đầy đủ thông tin !");
            }
            if (request.Price < 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Vui lòng nhập giá tiền lớn hơn !");
            }
            if (request.Discount != null && request.Discount < 0)
            {
                throw new CustomException(StatusCodes.Status400BadRequest, "Vui lòng nhập giảm giá lớn hơn !");
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
                || string.IsNullOrWhiteSpace(request.Description))
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



        public static string CreateSlug(string name)
        {
            // Chuyển đổi các ký tự có dấu sang các ký tự không dấu
            name = RemoveAccents(name);
            // Thay thế các ký tự không mong muốn bằng dấu gạch ngang
            string slug = Regex.Replace(name, @"[^\w\d\s-]", "").Trim();
            // Thay thế khoảng trắng bằng dấu gạch ngang và chuyển đổi chuỗi thành chữ thường
            slug = Regex.Replace(slug, @"\s+", "-").ToLower();
            return slug;
        }

        public static string RemoveAccents(string input)
        {
            // Tạo một mảng các ký tự có dấu và ký tự không dấu tương ứng
            char[] accents = "áàảãạâấầẩẫậăắằẳẵặéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵđÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴĐ".ToCharArray();
            char[] noAccents = "aaaaaaaaaaaaaaaaaeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyydAAAAAAAAAAAAAAAAAEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYYD".ToCharArray();

            // Tạo một StringBuilder để lưu trữ chuỗi đã được chuyển đổi
            StringBuilder stringBuilder = new StringBuilder(input);

            // Duyệt qua từng ký tự trong chuỗi và thực hiện chuyển đổi nếu cần
            for (int i = 0; i < stringBuilder.Length; i++)
            {
                char c = stringBuilder[i];
                int index = Array.IndexOf(accents, c);
                if (index != -1)
                {
                    stringBuilder[i] = noAccents[index];
                }
            }

            // Trả về chuỗi đã được chuyển đổi
            return stringBuilder.ToString();
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

        public static bool RegexColor(string color)
        {
            string pattern = @"^(#([0-9a-fA-F]{6}|[0-9a-fA-F]{3})|rgba?\(\s*(\d{1,3}\s*,\s*\d{1,3}\s*,\s*\d{1,3}\s*(,\s*(0(\.\d+)?|1(\.0+)?)\s*)?)?\)|hsla?\(\s*(\d+(\.\d+)?\s*,\s*\d+(\.\d+)?%\s*,\s*\d+(\.\d+)?%\s*(,\s*(0(\.\d+)?|1(\.0+)?)\s*)?)?\))$";
            return Regex.IsMatch(color, pattern);
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
