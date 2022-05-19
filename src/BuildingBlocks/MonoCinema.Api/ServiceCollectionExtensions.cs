using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonoCinema.Application;
using MonoCinema.Infrastructure;

namespace MonoCinema.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationFramework();
        //services.AddInfrastructureFramework(configuration);
        return services;
    }

    public static IApplicationBuilder UseFramework(this IApplicationBuilder builder)
    {
        //builder.UseInfrastructureFramework();
        return builder;
    }
}
