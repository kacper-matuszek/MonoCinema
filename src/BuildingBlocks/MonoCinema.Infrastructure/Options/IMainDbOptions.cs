namespace MonoCinema.Infrastructure.Options;

public interface IMainDbOptions
{
    string ConnectionString { get; set; }
    DatabaseType DatabaseType { get; set; }
}
