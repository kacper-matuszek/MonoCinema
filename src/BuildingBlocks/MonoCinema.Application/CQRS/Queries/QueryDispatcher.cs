using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MonoCinema.Application.CQRS.Queries
{
    internal sealed class QueryDispatcher : IQueryDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public QueryDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            if (query == null)
            {
                throw new ArgumentNullException($"{nameof(query)} cannot be null.");
            }

            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            var handler = _serviceProvider.GetRequiredService(queryHandlerType);
            return await (Task<TResult>)handler.GetType()
                                               .GetMethod(nameof(IQueryHandler<IQuery<TResult>, TResult>.HandleAsync))
                                               .Invoke(handler, new object[] { query, cancellationToken });
        }
    }
}
