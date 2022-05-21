using Microsoft.EntityFrameworkCore;
using MonoCinema.Infrastructure.Options;

namespace MonoCinema.Infrastructure.EF.Extensions;

internal static class DbContextOptionsExtensions
{
    internal static DbContextOptionsBuilder ApplyOptions(this DbContextOptionsBuilder optionsBuilder, DatabaseType databaseType, string connectionString, string migrationAssembly)
        => databaseType switch
        {
            DatabaseType.Postgres => Postgres.Appliers.DbContextOptionsApplier.ApplyOptions(optionsBuilder, connectionString, migrationAssembly),
            DatabaseType.SqlServer => MsSql.Appliers.DbContextOptionsApplier.ApplyOptions(optionsBuilder, connectionString, migrationAssembly),
            _ => throw new NotImplementedException(),
        };
}
