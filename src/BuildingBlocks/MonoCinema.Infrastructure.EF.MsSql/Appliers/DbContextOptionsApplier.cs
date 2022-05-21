using Microsoft.EntityFrameworkCore;

namespace MonoCinema.Infrastructure.EF.MsSql.Appliers;

internal static class DbContextOptionsApplier 
{
    public static DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string migrationAssembly) 
        => optionsBuilder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationAssembly));

}
