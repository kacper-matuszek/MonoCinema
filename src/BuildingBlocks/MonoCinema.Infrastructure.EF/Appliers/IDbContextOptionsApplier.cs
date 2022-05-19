using Microsoft.EntityFrameworkCore;

namespace MonoCinema.Infrastructure.EF.Appliers;

public interface IDbContextOptionsApplier
{
    DbContextOptionsBuilder ApplyOptions(DbContextOptionsBuilder optionsBuilder, string connectionString, string migrationAssembly);
}
