using Microsoft.EntityFrameworkCore.Infrastructure;
using MonoCinema.Infrastructure.Options;

namespace MonoCinema.Infrastructure.EF.Extensions;

internal static class DatabaseFacadeExtensions
{
    internal static DatabaseFacade Migrate(this DatabaseFacade database, DatabaseType databaseType)
        => databaseType switch
        {
            DatabaseType.Postgres => Postgres.Appliers.MigrationApplier.UseDbMigration(database),
            DatabaseType.SqlServer => MsSql.Appliers.MigrationApplier.UseDbMigration(database),
            _ => throw new NotImplementedException(),
        };
}
