namespace MonoCinema.SharedKernel;

public abstract class AggregateRoot<TId>
{
    private bool _versionIncremented = false;

    public TId Id { get; protected set; } = default!;
    public int Version { get; protected set; } = 1;

    protected void IncrementVersion()
    {
        if (_versionIncremented)
        {
            return;
        }

        Version++;
        _versionIncremented = true;
    }
}
