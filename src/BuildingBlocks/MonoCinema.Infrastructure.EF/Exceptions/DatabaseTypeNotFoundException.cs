namespace MonoCinema.Infrastructure.EF.Exceptions;

public class DatabaseTypeNotFoundException : Exception
{
    internal DatabaseTypeNotFoundException(string databaseTypeConfiguration)
        : base ($"Not found database type called: {databaseTypeConfiguration}")
    { }
}
