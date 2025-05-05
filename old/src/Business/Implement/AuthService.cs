using Models;
using Business.Interface;
using System.Text.Json;
using Requests.Auth;
using Middlewares.ErrorHandling;
using Microsoft.AspNetCore.Http;
using Libs;
using Responses;
using DTOs.Auth;
using Contexts;
using Dapper;
using System.Data;
using Enums;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using DTOs.Role;

namespace Business.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IConfigurationUtils _configurationUtils;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJwtUtils _jwtUtils;
        private readonly IDapperContext _dapperContext;
        private readonly IMapper _mapper;

        public AuthService(IConfigurationUtils configurationUtils,
            IHttpClientFactory httpContextFactory,
            IJwtUtils jwtUtils,
            IDapperContext dapperContext,
            IMapper mapper)
        {
            _configurationUtils = configurationUtils;
            _httpClientFactory = httpContextFactory;
            _jwtUtils = jwtUtils;
            _dapperContext = dapperContext;
            _mapper = mapper;
        }

        
        public async Task<ResponseObject<AuthDTO>> Login(LoginRequest loginRequest)
        {
            // await ValidateRecaptcha(loginRequest.ReCaptchaToken!);

            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();

                var hashedPassword = FuncUtils.HashPassword(loginRequest.Password, _configurationUtils.GetConfiguration("AppSettings:SecretKey"));
                loginRequest.Password = hashedPassword;

                var stdName = "Auth_Login";
                var parameters = new DynamicParameters();
                parameters.Add("@Email", loginRequest.Email);
                parameters.Add("@Password", loginRequest.Password);

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
                    throw new CustomException(StatusCodes.Status400BadRequest, "Email hoặc mật khẩu không chính xác !");
                }

                if (userData.Status == (int)Status.InActive)
                {
                    throw new CustomException(StatusCodes.Status400BadRequest, "Tài khoản đã bị khóa vui lòng liên hệ quản trị viên!");
                }

                var listRoleTokenDTO = ToListRoleToken(userData);
                string accessToken = _jwtUtils.CreateAccessToken(userData, listRoleTokenDTO);
                var refreshToken = GenerateRefreshToken(userData.Id);


                var myRefreshToken = await connection.QueryFirstOrDefaultAsync<RefreshTokens>("RefreshToken_GetByUserId", new { UserId = userData.Id }, commandType: CommandType.StoredProcedure);

                if (myRefreshToken == null)
                {
                    var createParams = new DynamicParameters();
                    createParams.Add("@RefreshTokenData", JsonSerializer.Serialize(refreshToken));
                    await connection.ExecuteAsync("RefreshToken_Create", createParams, commandType: CommandType.StoredProcedure);
                }
                else
                {
                    myRefreshToken.Token = refreshToken.Token;
                    myRefreshToken.ExpiredTime = refreshToken.ExpiredTime;

                    var updateParams = new DynamicParameters();
                    updateParams.Add("@RefreshTokenData", JsonSerializer.Serialize(myRefreshToken));
                    await connection.ExecuteAsync("RefreshToken_Update", updateParams, commandType: CommandType.StoredProcedure);
                }


                var authDto = _mapper.Map<AuthDTO>(userData);
                authDto.AccessToken = accessToken;
                authDto.RefreshToken = refreshToken.Token;

                return ResponseObject<AuthDTO>.ResponseSuccess("Đăng nhập thành công !", authDto);
            }
        }


        public async Task<ResponseObject<AuthDTO>> ReNewToken(string refreshToken)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                var existingRefreshToken = await connection.QueryFirstOrDefaultAsync<RefreshTokens>("RefreshToken_GetByToken", new { Token = refreshToken }, commandType: CommandType.StoredProcedure)
                ?? throw new CustomException(StatusCodes.Status404NotFound, "RefreshToken không tồn tại");

                if (existingRefreshToken.ExpiredTime < DateTime.Now)
                {
                    throw new CustomException(StatusCodes.Status419AuthenticationTimeout, "Phiên đăng nhập đã hết hạn");
                }

                var userDictionary = new Dictionary<int, Users>();

                var users = (await connection.QueryAsync<Users, RoleUser, Roles, Users>(
                    "User_GetFullDataById",
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
                    new { UserId = existingRefreshToken.UserId },
                    commandType: CommandType.StoredProcedure,
                    splitOn: "Id"
                )).Distinct().ToList();

                var userData = userDictionary.Values.FirstOrDefault();
                if (userData == null)
                {
                    throw new CustomException(StatusCodes.Status404NotFound, "Người dùng không tồn tại");
                }


                var listRoleTokenDTO = ToListRoleToken(userData);

                var newAccessToken = _jwtUtils.CreateAccessToken(userData, listRoleTokenDTO);
                var newRefreshToken = GenerateRefreshToken(userData.Id);

                existingRefreshToken.Token = newRefreshToken.Token;
                existingRefreshToken.ExpiredTime = newRefreshToken.ExpiredTime;

                var updateParams = new DynamicParameters();
                updateParams.Add("@RefreshTokenData", JsonSerializer.Serialize(existingRefreshToken));

                await connection.ExecuteAsync("RefreshToken_Update", updateParams, commandType: CommandType.StoredProcedure);

                var authDto = _mapper.Map<AuthDTO>(userData);
                authDto.AccessToken = newAccessToken;
                authDto.RefreshToken = newRefreshToken.Token;

                return ResponseObject<AuthDTO>.ResponseSuccess("Làm mới token thành công !", authDto);
            }
        }

        public async Task<ResponseText> Register(RegisterRequest registerRequest)
        {
            using (var connection = _dapperContext.CreateConnection())
            {
                connection.Open();
                registerRequest.UserName ??= ExtractUserNameFromEmail(registerRequest.Email);
                var hashedPassword = FuncUtils.HashPassword(registerRequest.Password, _configurationUtils.GetConfiguration("AppSettings:SecretKey"));
                registerRequest.Password = hashedPassword;

                string defaultAvatarMale = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127887/root/nam_pjwipr.jpg";
                string defaultAvatarFamele = "https://res.cloudinary.com/dbdozmkxv/image/upload/v1700127885/root/nu_uqiotb.jpg";
                registerRequest.Avatar = registerRequest.Gender == 0 ? defaultAvatarMale : defaultAvatarFamele;


                var stdName = "Auth_Register";
                var userJson = JsonSerializer.Serialize(registerRequest);
                var parameters = new DynamicParameters();
                parameters.Add("@UserData", userJson);

                await connection.ExecuteAsync(stdName, parameters, commandType: CommandType.StoredProcedure);

                return ResponseText.ResponseSuccess("Đăng ký thành công !");
            }
        }

        private List<RoleTokenDTO> ToListRoleToken(Users users)
        {
            var listRoleTokenDTO = new List<RoleTokenDTO>();
            foreach (var roleItem in users.ListRoleUser!)
            {
                listRoleTokenDTO.Add(_mapper.Map<RoleTokenDTO>(roleItem.Role));
            }

            return listRoleTokenDTO;
        }

        private static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        private static RefreshTokens GenerateRefreshToken(int userId)
        {
            var refreshToken = new RefreshTokens
            {
                Token = CreateRandomToken(),
                UserId = userId,
                CreatedAt = DateTime.Now,
                ExpiredTime = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        private async Task<bool> ValidateRecaptcha(string token)
        {
            var secretKey = _configurationUtils.GetConfiguration("AppSettings:GoogleRecaptcha:SecretKey");
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}"
            );

            using (var document = JsonDocument.Parse(response))
            {
                var root = document.RootElement;
                bool success = root.GetProperty("success").GetBoolean();

                if (!success)
                {
                    if (root.TryGetProperty("error-codes", out JsonElement errorCodesElement))
                    {
                        var errorCodes = errorCodesElement.EnumerateArray()
                            .Select(error => error.GetString())
                            .ToList();

                        string errorString = string.Join(", ", errorCodes);

                        throw new CustomException(StatusCodes.Status400BadRequest, $"reCAPTCHA Error: {errorString}");
                    }
                }
                return success;
            }
        }

        private static string ExtractUserNameFromEmail(string email, int maxLength = 30)
        {
            var userName = email.Split('@')[0];
            return userName.Length > maxLength ? userName.Substring(0, maxLength) : userName;
        }

    }
}
