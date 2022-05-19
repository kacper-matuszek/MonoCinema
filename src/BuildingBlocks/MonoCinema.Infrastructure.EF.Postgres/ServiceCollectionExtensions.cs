using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonoCinema.Infrastructure.EF.Postgres.Appliers;

namespace MonoCinema.Infrastructure.EF.Postgres;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreDbContext<TContext>(this IServiceCollection services, IConfiguration configuration,
        string migrationAssemblyName)
        where TContext : DbContext
        => services.AddDatabaseContext<TContext>(configuration, migrationAssemblyName, new DbContextOptionsApplier());

    public static IApplicationBuilder UseDbAutoMigration(this IApplicationBuilder builder)
        => builder.UseDatabaseAutoMigration((db) => db.Migrate());

}
