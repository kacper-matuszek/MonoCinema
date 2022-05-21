using Microsoft.EntityFrameworkCore;

namespace MonoCinema.Infrastructure.EF.Postgres.Appliers;

internal static class DbContextOptionsApplier
{
    public static DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string migrationAssembly)
        => optionsBuilder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationAssembly))
                         .UseLowerCaseNamingConvention();
}
