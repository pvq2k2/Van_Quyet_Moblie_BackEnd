using Application.Auth;
using Application.Common.DeviceInfoProvider;
using Application.Common.Email;
using Application.Common.JwtToken;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
/// <summary>
/// Chỉ dùng để tham chiếu Assembly của Application.
/// </summary>
public static class ApplicationModule
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IDeviceInfoProvider, DeviceInfoProvider>();

        services.AddTransient<IEmailService, EmailService>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}
