namespace MonoCinema.Infrastructure.Options;

public class MainDbOptions : IMainDbOptions
{
    internal const string SectionName = "postgres";
    public string ConnectionString { get; set; }
}
