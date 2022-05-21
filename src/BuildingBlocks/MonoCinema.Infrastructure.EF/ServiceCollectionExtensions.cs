using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonoCinema.Infrastructure.EF.Extensions;

namespace MonoCinema.Infrastructure.EF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services,
                                                                  string migrationAssembly,
                                                                  string connectionString,
                                                                  EfDatabaseType databaseType)
        where TContext : DbContext
    {
        services.AddDbContext<TContext>(opt => opt.ApplyOptions(databaseType, connectionString, migrationAssembly));
        return services;
    }

    public static IApplicationBuilder UseDatabaseAutoMigration(this IApplicationBuilder builder, EfDatabaseType databaseType)
    {
        var contextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface && x != typeof(DbContext));

        using var scope = builder.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbContext>>();
        try
        {
            foreach (var contextType in contextTypes)
            {
                logger.LogInformation("Running migrations for db context: {0}", contextType.Name);
                if (scope.ServiceProvider.GetRequiredService(contextType) is not DbContext context)
                {
                    logger.LogWarning("Not found database context: {0}", contextType.FullName);
                    continue;
                }
                context.Database.Migrate(databaseType);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured during running migrations");
        }

        return builder;
    }
}
