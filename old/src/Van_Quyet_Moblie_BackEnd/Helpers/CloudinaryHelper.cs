using CloudinaryDotNet.Actions;
using CloudinaryDotNet;

namespace Van_Quyet_Moblie_BackEnd.Helpers
{
    public class CloudinaryHelper
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryHelper(IConfiguration configuration)
        {
            var account = new Account(
                configuration.GetSection("AppSettings:CloudinarySettings:CloudName").Value,
                configuration.GetSection("AppSettings:CloudinarySettings:ApiKey").Value,
                configuration.GetSection("AppSettings:CloudinarySettings:ApiSecret").Value);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImage(IFormFile file, string folderPath, string fileName)
        {
            // Tách đường dẫn thư mục thành các phần
            var folders = folderPath.Split('/');

            // Bắt đầu tạo thư mục từ thư mục gốc
            CreateFolderRecursively(folders, 0);

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = fileName + "-" + "image" + "-" + DateTime.Now.Ticks,
                Folder = folderPath
                //Transformation = new Transformation().Width(300).Height(400).Crop("fill")
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }
            string imageUrl = uploadResult.SecureUrl.ToString();
            return imageUrl;
        }

        private void CreateFolderRecursively(string[] folders, int currentIndex)
        {
            if (currentIndex >= folders.Length)
            {
                // Đã kiểm tra và tạo các thư mục con
                return;
            }

            var currentFolder = folders[currentIndex];

            // Kiểm tra xem thư mục hiện tại đã tồn tại hay chưa
            if (!CheckFolderExists(currentFolder))
            {
                // Nếu thư mục chưa tồn tại, tạo mới
                CreateFolder(currentFolder);
            }

            // Tiếp tục kiểm tra và tạo các thư mục con
            CreateFolderRecursively(folders, currentIndex + 1);
        }

        private bool CheckFolderExists(string folderName)
        {
            try
            {
                // Thử lấy thông tin về thư mục
                var getResourceParams = new GetResourceParams($"folder:{folderName}");
                _cloudinary.GetResource(getResourceParams);
                return true; // Thư mục đã tồn tại
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); // Thư mục chưa tồn tại hoặc có lỗi
            }
        }

        private void CreateFolder(string folderName)
        {
            try
            {
                // Tạo thư mục mới
                _cloudinary.CreateFolder(folderName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);// Xử lý lỗi khi không thể tạo thư mục.
            }
        }

    public async Task<bool> DeleteImageByUrl(string imageUrl)
        {
            try
            {
                // Phân tích đường dẫn URL để trích xuất public ID
                var uri = new System.Uri(imageUrl);
                var segments = uri.Segments;
                string publicId;
                if (segments.Length == 6) {
                    publicId = segments[segments.Length - 1].Split('.')[0];
                }
                else
                {
                    publicId = string.Join("", segments.Skip(5)).Split('.')[0];
                }

                // Thực hiện xóa ảnh bằng public ID
                var deleteParams = new DeletionParams(publicId);
                var result = await _cloudinary.DestroyAsync(deleteParams);

                // Kiểm tra xem việc xóa thành công hay không
                if (result.Result == "ok" && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true; // Xóa thành công
                } else
                {
                    throw new Exception("Lỗi xóa ảnh:" + result.Result);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                throw new Exception($"Lỗi xóa ảnh: {ex.Message}");
            }
        }
    }
}
