using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MonoCinema.Infrastructure.EF.Extensions;

internal static class DatabaseFacadeExtensions
{
    internal static DatabaseFacade Migrate(this DatabaseFacade database, EfDatabaseType databaseType)
        => databaseType switch
        {
            EfDatabaseType.Postgres => Postgres.Appliers.MigrationApplier.UseDbMigration(database),
            EfDatabaseType.SqlServer => MsSql.Appliers.MigrationApplier.UseDbMigration(database),
            _ => throw new NotImplementedException(),
        };
}
