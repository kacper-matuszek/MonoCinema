using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonoCinema.Infrastructure.EF;
using MonoCinema.Infrastructure.Options;
using MonoCinema.Infrastructure.Utils;

namespace MonoCinema.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MainDbOptions>(configuration.GetSection(MainDbOptions.SectionName));
        return services;
    }

    public static IServiceCollection AddMainContext<T>(this IServiceCollection services, IConfiguration configuration, 
        string migrationAssembly)
        where T : DbContext
    {
        var connectionString = configuration[$"{MainDbOptions.SectionName}:{nameof(MainDbOptions.ConnectionString)}"];
        var dbTypeString = configuration[$"{MainDbOptions.SectionName}:{nameof(MainDbOptions.DatabaseType)}"];
        Enum.TryParse(dbTypeString, out DatabaseType dbType);
        return services.AddDatabaseContext<T>(migrationAssembly, connectionString, DatabaseTypeUtils.ToEfDatabseType(dbType));
    }

    public static IApplicationBuilder UseInfrastructureFramework(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var dbType = scope.ServiceProvider.GetRequiredService<IOptions<MainDbOptions>>().Value;
        return builder.UseDatabaseAutoMigration(DatabaseTypeUtils.ToEfDatabseType(dbType.DatabaseType));
    }
}
