using Microsoft.EntityFrameworkCore;
using MonoCinema.Infrastructure.EF.Appliers;

namespace MonoCinema.Infrastructure.EF.MsSql.Appliers;

internal sealed class DbContextOptionsApplier : IDbContextOptionsApplier
{
    public DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string migrationAssembly) 
        => optionsBuilder.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(migrationAssembly));

}
