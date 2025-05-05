using Moq;
using Business.Implement;
using Requests.Auth;
using DTOs.Auth;
using Libs;
using Contexts;
using AutoMapper;
using System.Data;
using Dapper;
using Middlewares.ErrorHandling;
using Models;
using Enums;
using Xunit;
using System.Threading.Tasks;

namespace Business.Test
{
    public class AuthServiceTest
    {
        private readonly Mock<IDbConnection> _dbConnectionMock;
        private readonly Mock<IConfigurationUtils> _configurationUtilsMock;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<IJwtUtils> _jwtUtilsMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IDapperContext> _mockDapperContext;
        private readonly AuthService _authService;

        public AuthServiceTest()
        {
            _dbConnectionMock = new Mock<IDbConnection>();
            _configurationUtilsMock = new Mock<IConfigurationUtils>();
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _jwtUtilsMock = new Mock<IJwtUtils>();
            _mapperMock = new Mock<IMapper>();
            _mockDapperContext = new Mock<IDapperContext>();

            _authService = new AuthService(
                _configurationUtilsMock.Object,
                _httpClientFactoryMock.Object,
                _jwtUtilsMock.Object,
                _mockDapperContext.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Register_ThrowsException_WhenEmailExists()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "existing@example.com",
                Password = "Password123",
                FullName = "Test User",
                Gender = 0
            };

            //var mockConnection = new Mock<IDbConnection>();
            //_mockDapperContext.Setup(d => d.CreateConnection()).Returns(mockConnection.Object);

            // Simulate email already exists
            //_dbConnectionMock.Setup(c => c.QueryFirstOrDefaultAsync<int>(
            //    It.IsAny<string>(),
            //    It.IsAny<object>(),
            //    null,
            //    null,
            //    null
            //)).ReturnsAsync(1); // Email already exists, returning 1

            // Act & Assert
            await Assert.ThrowsAsync<CustomException>(() => _authService.Register(registerRequest));
        }

        [Fact]
        public async Task Login_ReturnsSuccess_WhenValidCredentials()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Password123",
                ReCaptchaToken = "fake-token"
            };

            var user = new Users
            {
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                //RoleId = 1,
                //Status = ()Status.Active
            };

            var mockConnection = new Mock<IDbConnection>();
            _mockDapperContext.Setup(d => d.CreateConnection()).Returns(mockConnection.Object);

            //_jwtUtilsMock.Setup(j => j.CreateAccessToken(It.IsAny<Users>())).Returns("fake-jwt-token");

            // Simulate successful user retrieval
            mockConnection.Setup(c => c.QueryFirstOrDefaultAsync<Users>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                null,
                null,
                null
            )).ReturnsAsync(user);

            _mapperMock.Setup(m => m.Map<AuthDTO>(It.IsAny<Users>()))
                .Returns(new AuthDTO { AccessToken = "fake-jwt-token" });

            // Act
            var result = await _authService.Login(loginRequest);

            // Assert
            Assert.Equal("Đăng nhập thành công !", result.Message);
            Assert.Equal("fake-jwt-token", result.Data!.AccessToken);
        }
    }
}
