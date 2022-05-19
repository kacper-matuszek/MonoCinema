using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonoCinema.Infrastructure.EF.Appliers;
using MonoCinema.Infrastructure.Options;

namespace MonoCinema.Infrastructure.EF;

public static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, IConfiguration configuration, 
        string migrationAssembly, IDbContextOptionsApplier optionApplier)
        where TContext : DbContext
    {
        var connectionString = configuration[$"{MainDbOptions.SectionName}:{nameof(MainDbOptions.ConnectionString)}"];
        services.AddDbContext<TContext>(opt => optionApplier.ApplyOptions(opt, connectionString, migrationAssembly));
        return services;
    }

    internal static IApplicationBuilder UseDatabaseAutoMigration(this IApplicationBuilder builder, Action<DatabaseFacade> migrationAction)
    {
        if (migrationAction == null)
        {
            throw new ArgumentNullException($"Migration action must be applied.");
        }

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
                var context = scope.ServiceProvider.GetRequiredService(contextType) as DbContext;
                if (context is null)
                {
                    logger.LogWarning("Not found database context: {0}", contextType.FullName);
                    continue;
                }
                migrationAction.Invoke(context.Database);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured during running migrations");
        }

        return builder;
    }
}
