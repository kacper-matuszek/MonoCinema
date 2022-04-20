
namespace MonoCinema.Application.CQRS.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
    }
}