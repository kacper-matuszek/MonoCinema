using Microsoft.EntityFrameworkCore;
using MonoCinema.Infrastructure.EF.Appliers;

namespace MonoCinema.Infrastructure.EF.Postgres.Appliers;

internal sealed class DbContextOptionsApplier : IDbContextOptionsApplier
{
    public DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string migrationAssembly)
        => optionsBuilder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationAssembly))
                         .UseLowerCaseNamingConvention();
}
