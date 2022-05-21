using MonoCinema.Infrastructure.EF;
using MonoCinema.Infrastructure.Options;

namespace MonoCinema.Infrastructure.Utils;

internal static class DatabaseTypeUtils
{
    internal static EfDatabaseType ToEfDatabseType(DatabaseType databaseType)
        => databaseType switch
        {
            DatabaseType.Postgres => EfDatabaseType.Postgres,
            DatabaseType.SqlServer => EfDatabaseType.SqlServer,
            _ => throw new NotImplementedException(),
        };
}
