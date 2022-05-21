using Microsoft.EntityFrameworkCore;

namespace MonoCinema.Infrastructure.EF.Postgres.Appliers;

internal static class DbContextOptionsApplier
{
    public static DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string migrationAssembly)
    {
        //EF Core issue related to https://github.com/npgsql/efcore.pg/issues/2000
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        return optionsBuilder.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(migrationAssembly))
                             .UseLowerCaseNamingConvention();
    }
}
