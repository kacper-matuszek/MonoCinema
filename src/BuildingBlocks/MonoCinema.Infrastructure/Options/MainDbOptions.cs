namespace MonoCinema.Infrastructure.Options;

public class MainDbOptions : IMainDbOptions
{
    internal const string SectionName = "main-database";
    public string ConnectionString { get; set; }
}
