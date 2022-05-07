using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonoCinema.Infrastructure.Options;

namespace MonoCinema.Infrastructure;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddInfrastructureFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MainDbOptions>(configuration.GetSection(MainDbOptions.SectionName));
        return services;
    }

    public static IApplicationBuilder UseInfrastructureFramework(this IApplicationBuilder builder) 
        => builder.UseMainDbAutoMigration();

    public static IServiceCollection AddMainContext<T>(this IServiceCollection services, IConfiguration configuration)
        where T : DbContext
    {
        var connectionString = configuration[$"{MainDbOptions.SectionName}:{nameof(MainDbOptions.ConnectionString)}"];
        services.AddDbContext<T>(x => x.UseNpgsql(connectionString));

        return services;
    }

    internal static IApplicationBuilder UseMainDbAutoMigration(this IApplicationBuilder builder)
    {
        var contextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface && x != typeof(DbContext));

        using var scope = builder.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbContext>>();
        try
        {
            foreach(var contextType in contextTypes)
            {
                logger.LogInformation("Running migrations for db context: {0}", contextType.Name);
                var context = scope.ServiceProvider.GetRequiredService(contextType) as DbContext;
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured during running migrations");
        }

        return builder;
    }
}
