using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Chỉ dùng để tham chiếu Assembly của Infrastructure.
/// </summary>
public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {

        return services;
    }
}
