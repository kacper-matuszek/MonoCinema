using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonoCinema.Infrastructure.EF.MsSql.Appliers;

namespace MonoCinema.Infrastructure.EF.MsSql;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMsSqlDbContext<TContext>(this IServiceCollection services, IConfiguration configuration,
        string migrationAssemblyName)
        where TContext : DbContext
        => services.AddDatabaseContext<TContext>(configuration, migrationAssemblyName, new DbContextOptionsApplier());

    public static IApplicationBuilder UseDatabaseAutoMigration(this IApplicationBuilder builder)
        => builder.UseDatabaseAutoMigration((db) => db.Migrate());

}
