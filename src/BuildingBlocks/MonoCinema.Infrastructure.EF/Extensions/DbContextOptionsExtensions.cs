using Microsoft.EntityFrameworkCore;

namespace MonoCinema.Infrastructure.EF.Extensions;

internal static class DbContextOptionsExtensions
{
    internal static DbContextOptionsBuilder ApplyOptions(this DbContextOptionsBuilder optionsBuilder, EfDatabaseType databaseType, string connectionString, string migrationAssembly)
        => databaseType switch
        {
            EfDatabaseType.Postgres => Postgres.Appliers.DbContextOptionsApplier.ApplyOptions(optionsBuilder, connectionString, migrationAssembly),
            EfDatabaseType.SqlServer => MsSql.Appliers.DbContextOptionsApplier.ApplyOptions(optionsBuilder, connectionString, migrationAssembly),
            _ => throw new NotImplementedException(),
        };
}
