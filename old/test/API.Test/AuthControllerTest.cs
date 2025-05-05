using Moq;
using API.Controllers;
using Business.Interface;
using Requests.Auth;
using Responses;
using DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace API.Test
{
    public class AuthControllerTest
    {
        private readonly AuthController _controller;
        private readonly Mock<IAuthService> _mockAuthService;

        public AuthControllerTest()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task Register_ReturnsOkResult_WhenRegistrationIsSuccessful()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "Password123",
                Gender = 0
            };

            _mockAuthService.Setup(service => service.Register(It.IsAny<RegisterRequest>()))
                .ReturnsAsync(ResponseText.ResponseSuccess("Đăng ký thành công !"));

            // Act
            var result = await _controller.Register(registerRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseText>(okResult.Value);
            Assert.Equal("Đăng ký thành công !", response.Message);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@example.com",
                Password = "Password123",
                ReCaptchaToken = "fake-recaptcha-token"
            };

            var authDto = new AuthDTO
            {
                AccessToken = "fake-jwt-token"
            };

            _mockAuthService.Setup(service => service.Login(It.IsAny<LoginRequest>()))
                .ReturnsAsync(ResponseObject<AuthDTO>.ResponseSuccess("Đăng nhập thành công !", authDto));

            // Act
            var result = await _controller.Login(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseObject<AuthDTO>>(okResult.Value);
            Assert.Equal("Đăng nhập thành công !", response.Message);
            Assert.Equal("fake-jwt-token", response.Data!.AccessToken);
        }
    }
}

