using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MonoCinema.Application.CQRS.Queries;
using MonoCinema.Application.CQRS.Events;
using MonoCinema.Application.CQRS.Commands;

namespace MonoCinema.Application.CQRS;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddCQRS(this IServiceCollection services)
    {
        var appAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToArray();

        return services.AddCommands(appAssemblies)
                       .AddEvents(appAssemblies)
                       .AddQueries(appAssemblies);
    }

    private static IServiceCollection AddCommands(this IServiceCollection services, Assembly[] assemblies)
    {
        var commandHandlerType = typeof(ICommandHandler<>);
        var commandHandlerTypes = GetGenericTypes(assemblies, commandHandlerType);

        RegisterGenericTypes(services, commandHandlerType, commandHandlerTypes);
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        return services;
    }

    private static IServiceCollection AddEvents(this IServiceCollection services, Assembly[] assemblies)
    {
        var eventSubscriberType = typeof(ISubscriber<>);
        var eventHandlerTypes = GetGenericTypes(assemblies, eventSubscriberType);

        RegisterGenericTypes(services, eventSubscriberType, eventHandlerTypes);
        services.AddSingleton<IEventDispatcher, EventDispatcher>();

        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services, Assembly[] assemblies)
    {
        var queryHandlerType = typeof(IQueryHandler<,>);
        var queryHandlerTypes = GetGenericTypes(assemblies, queryHandlerType);

        RegisterGenericTypes(services, queryHandlerType, queryHandlerTypes);
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        return services;
    }

    private static IEnumerable<TypeInfo> GetTypes(Assembly[] assemblies, Type interfaceType)
    {
        return assemblies.SelectMany(a => a.DefinedTypes.Where(ti => !ti.IsAbstract && !ti.IsInterface && ti.GetInterfaces().Any(i => i.IsInterface && i.GetType() == interfaceType)));
    }

    private static IEnumerable<TypeInfo> GetGenericTypes(Assembly[] assemblies, Type genericInterfaceType)
    {
        return assemblies.SelectMany(a => a.DefinedTypes.Where(ti => !ti.IsAbstract && !ti.IsInterface && ti.GetInterfaces().Any(i => i.IsInterface && i.IsGenericType && i.GetGenericTypeDefinition() == genericInterfaceType)));
    }

    private static void RegisterGenericTypes(IServiceCollection services, Type genericType, IEnumerable<TypeInfo> types, ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        foreach (var type in types)
        {
            var genType = genericType;
            var genericArguments = genericType.GetGenericArguments();
            if (genericArguments.Any())
            {
                var genericInterfaces = type.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);
                if (!genericInterfaces.Any())
                {
                    continue;
                }
                var specifiedArguments = genericInterfaces.First().GetGenericArguments();
                genType = genericType.MakeGenericType(specifiedArguments);
            }
            services.Add(new ServiceDescriptor(genType, type, lifetime));
        }
    }
}
