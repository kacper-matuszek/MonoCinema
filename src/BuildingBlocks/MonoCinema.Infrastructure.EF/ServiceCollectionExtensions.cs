﻿using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MonoCinema.Infrastructure.EF.Exceptions;
using MonoCinema.Infrastructure.EF.Extensions;
using MonoCinema.Infrastructure.Options;

namespace MonoCinema.Infrastructure.EF;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseContext<TContext>(this IServiceCollection services, IConfiguration configuration, 
        string migrationAssembly)
        where TContext : DbContext
    {
        var connectionString = configuration[$"{MainDbOptions.SectionName}:{nameof(MainDbOptions.ConnectionString)}"];
        var databaseTypeString = configuration[$"{MainDbOptions.SectionName}:{nameof(MainDbOptions.DatabaseType)}"];
        if (!Enum.TryParse(databaseTypeString, out DatabaseType databaseType))
        {
            throw new DatabaseTypeNotFoundException(databaseTypeString);
        }
        services.AddDbContext<TContext>(opt => opt.ApplyOptions(databaseType, connectionString, migrationAssembly));
        services.Configure<MainDbOptions>(configuration.GetSection(MainDbOptions.SectionName));
        return services;
    }

    public static IApplicationBuilder UseDatabaseAutoMigration(this IApplicationBuilder builder)
    {
        var contextTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(DbContext).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface && x != typeof(DbContext));

        using var scope = builder.ApplicationServices.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbContext>>();
        var dbOptions = scope.ServiceProvider.GetRequiredService<IOptions<MainDbOptions>>().Value;
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
                context.Database.Migrate(dbOptions.DatabaseType);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occured during running migrations");
        }

        return builder;
    }
}
