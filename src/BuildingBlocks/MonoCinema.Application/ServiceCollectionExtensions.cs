using Microsoft.Extensions.DependencyInjection;
using MonoCinema.Application.CQRS;

namespace MonoCinema.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationFramework(this IServiceCollection services)
    {
        services.AddCQRS();

        return services;
    }
}
