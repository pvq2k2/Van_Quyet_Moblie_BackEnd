using AutoMapper;
using Business.Interface;
using Contexts;
using Dapper;
using DTOs.User;
using Libs;
using Microsoft.AspNetCore.Http;
using Middlewares.ErrorHandling;
using Models;
using Requests;
using Requests.User;
using Responses;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Data;
using System.Text.Json;

namespace Business.Implement
{
    public class UserService : IUserService
    {
        private readonly IDapperContext _dapperContext;
        private readonly IMapper _mapper;
        private readonly ICloudinaryUtils _cloudinaryUtils;
        private readonly IConfigurationUtils _configurationUtils;

        public UserService(IDapperContext dapperContext, IMapper mapper, ICloudinaryUtils cloudinaryUtils, IConfigurationUtils configurationUtils)
        {
            _dapperContext = dapperContext;
            _mapper = mapper;
            _cloudinaryUtils = cloudinaryUtils;
            _configurationUtils = configurationUtils;
        }


        private class UserWithTotalRecords : Users
        {
            public int TotalRecords { get; set; }
        }

        public static bool IsImage(IFormFile imageFile, int maxSizeInBytes = (2 * 1024 * 1024))
        {
            //if (imageFile == null || imageFile.Length == 0)
            //{
            //    throw new CustomException(StatusCodes.Status400BadRequest, "Không có ảnh nào được chọn !");
            //}
            if (imageFile != null || imageFile?.Length > 0)
            {
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
            }
            return true;
        }

        private async Task<string> UploadAndValidateImage(IFormFile image, string folderPath, string fileName)
        {
            IsImage(image);
            return await _cloudinaryUtils.UploadImage(image, folderPath, fileName);
        }
        private static async Task<Users> IsUserExists(int userId, IDbConnection connection)
        {
            var stdName = "User_GetById";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", userId);

            var userExists = await connection.QueryFirstOrDefaultAsync<Users>(
                stdName,
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return userExists!;
        }

        private static string ExtractUserNameFromEmail(string email, int maxLength = 30)
        {
            var userName = email.Split('@')[0];
            return userName.Length > maxLength ? userName.Substring(0, maxLength) : userName;
        }

        public async Task<ResponseText> CreateUser(CreateUserRequest request)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();

                request.UserName ??= ExtractUserNameFromEmail(request.Email);

                var hashedPassword = FuncUtils.HashPassword(request.Password, _configurationUtils.GetConfiguration("AppSettings:SecretKey"));
                request.Password = hashedPassword;

                string image = "";
                if (request.Image == null || request.Image.Length == 0)
                {
                    string defaultAvatarMale = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127887/root/nam_pjwipr.jpg";
                    string defaultAvatarFamele = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127885/root/nu_uqiotb.jpg";
                    request.Avatar = request.Gender == 0 ? defaultAvatarMale : defaultAvatarFamele;
                }
                else
                {
                    image = await UploadAndValidateImage(request.Image!, $"msf/user", "user");
                    request.Avatar = image;
                }

                try
                {
                    var stdName = "User_Create";
                    var userJson = JsonSerializer.Serialize(request);
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserData", userJson);

                    await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);



                    return ResponseText.ResponseSuccess("Thêm tài khoản thành công !");
                }
                catch (Exception ex)
                {
                    if (!string.IsNullOrEmpty(image))
                    {
                        await _cloudinaryUtils.DeleteImageByUrl(image);
                    }
                    throw new Exception(ex.Message);
                }

            }
        }

        public async Task<ResponseText> DeleteUser(int userId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var user = await IsUserExists(userId, connection);

                if (user == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại !");
                }
                var stdName = "User_Delete";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                return ResponseText.ResponseSuccess("Xóa người dùng thành công !");
            }
        }

        public async Task<PageResult<UserDTO>> GetAllUser(FilterUserRequest filter)
        {
            if (filter.PageNumber < 1)
            {
                filter.PageNumber = 1;
            }

            if (filter.PageSize <= 0)
            {
                filter.PageSize = 10;
            }

            using (var connection = _dapperContext.CreateConnection())
            {
                var stdName = "User_GetAll";
                var parameters = new DynamicParameters();
                parameters.Add("@FilterUserData", JsonSerializer.Serialize(filter));
                parameters.Add("@PageNumber", filter.PageNumber);
                parameters.Add("@PageSize", filter.PageSize);

                var userDictionary = new Dictionary<int, UserWithTotalRecords>();

                var users = (await connection.QueryAsync<UserWithTotalRecords, RoleUser, Roles, UserWithTotalRecords>(
                    stdName,
                    (user, roleUser, role) =>
                    {
                        if (!userDictionary.TryGetValue(user.Id, out var existingUser))
                        {
                            existingUser = user;
                            existingUser.ListRoleUser = [];
                            userDictionary.Add(user.Id, existingUser);
                        }

                        if (roleUser != null)
                        {
                            // Kiểm tra xem roleUser đã tồn tại trong danh sách chưa
                            if (!existingUser.ListRoleUser!.Any(pr => pr.Id == roleUser.Id))
                            {
                                roleUser.Role = role;
                                existingUser.ListRoleUser!.Add(roleUser);
                            }
                        }


                        return existingUser;
                    },
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Id"
                )).Distinct().ToList();

                var totalCount = users.FirstOrDefault()?.TotalRecords;

                // Tạo đối tượng Pagination
                var pagination = new Pagination()
                {
                    PageSize = filter.PageSize,
                    PageNumber = filter.PageNumber,
                    TotalCount = totalCount ?? 0
                };

                // Ánh xạ sang UserDTO
                var userDtoList = _mapper.Map<List<UserDTO>>(users);

                return new PageResult<UserDTO>(pagination, userDtoList);
            }
        }


        public async Task<ResponseObject<UserDTO>> GetUserById(int userId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var user = await IsUserExists(userId, connection);

                if (user == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại !");
                }
                var userDTO = _mapper.Map<UserDTO>(user);

                return ResponseObject<UserDTO>.ResponseSuccess("Thành công", userDTO);
            }
        }

        public async Task<ResponseObject<UserDTO>> GetFullDataUserById(int userId)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var stdName = "User_GetFullDataById";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var userDictionary = new Dictionary<int, Users>();

                var users = (await connection.QueryAsync<Users, RoleUser, Roles, Users>(
                    stdName,
                    (user, roleUser, role) =>
                    {
                        if (!userDictionary.TryGetValue(user.Id, out var existingUser))
                        {
                            existingUser = user;
                            existingUser.ListRoleUser = [];
                            userDictionary.Add(user.Id, existingUser);
                        }

                        if (roleUser != null)
                        {
                            // Kiểm tra xem roleUser đã tồn tại trong danh sách chưa
                            if (!existingUser.ListRoleUser!.Any(pr => pr.Id == roleUser.Id))
                            {
                                roleUser.Role = role;
                                existingUser.ListRoleUser!.Add(roleUser);
                            }
                        }


                        return existingUser;
                    },
                    parameters,
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Id"
                )).Distinct().ToList();

                var userData = userDictionary.Values.FirstOrDefault();
                if (userData == null)
                {
                    throw new CustomException(StatusCodes.Status400BadRequest, "Người dùng không tồn tại !");
                }

                var userDTO = _mapper.Map<UserDTO>(userData);

                return ResponseObject<UserDTO>.ResponseSuccess("Thành công", userDTO);
            }
        }

        public async Task<ResponseText> UpdateUser(int userId, UpdateUserRequest request)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var user = await IsUserExists(userId, connection);

                if (user == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại !");
                }

                string? newImage = null; // Biến để lưu ảnh mới nếu có
                string defaultAvatarMale = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127887/root/nam_pjwipr.jpg";
                string defaultAvatarFamele = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127885/root/nu_uqiotb.jpg";
                if (request.Image == null || request.Image.Length == 0)
                {
                    if (defaultAvatarMale.Contains(request.Avatar!) || defaultAvatarFamele.Contains(request.Avatar!))
                    {
                        request.Avatar = request.Gender == 0 ? defaultAvatarMale : defaultAvatarFamele;
                    } else
                    {
                        request.Avatar = user.Avatar;
                    }
                }
                else
                {
                    // Upload ảnh mới và lưu vào biến newImage
                    newImage = await UploadAndValidateImage(request.Image!, $"msf/user", "user");
                    request.Avatar = newImage;
                }


                try
                {
                    var stdName = "User_Update";
                    var userJson = JsonSerializer.Serialize(request);
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserData", userJson);
                    parameters.Add("@UserId", userId);

                    await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                    // Nếu có upload ảnh mới thành công, xóa ảnh cũ
                    if (!string.IsNullOrEmpty(newImage))
                    {
                        await _cloudinaryUtils.DeleteImageByUrl(user.Avatar!);
                    }

                    return ResponseText.ResponseSuccess("Cập nhật người dùng thành công !");
                }
                catch (Exception ex)
                {
                    // Nếu đã upload ảnh mới nhưng thất bại trong quá trình xử lý, xóa ảnh mới
                    if (!string.IsNullOrEmpty(newImage))
                    {
                        await _cloudinaryUtils.DeleteImageByUrl(newImage);
                    }

                    throw new Exception(ex.Message);
                }

            }
        }

    }
}
