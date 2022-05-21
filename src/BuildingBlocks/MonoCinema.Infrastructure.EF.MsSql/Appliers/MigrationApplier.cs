using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MonoCinema.Infrastructure.EF.MsSql.Appliers;

internal static class MigrationApplier
{
    internal static DatabaseFacade UseDbMigration(DatabaseFacade database)
    {
        database.Migrate();
        return database;
    }
}
