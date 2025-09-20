using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework;

public static class EndpointsExtensions
{
    public static IServiceCollection AddEndpoints(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        // Use provided assemblies, or fall back to the executing assembly if none were passed.
        var toScan = (assemblies is { Length: > 0 }) ? assemblies : new[] { Assembly.GetExecutingAssembly() };
        services.AddEndpointsWithAssemblies(toScan);
        
        return services;
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndPoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndPoint(builder);
        }
        
        return app;
    }

    private static IServiceCollection AddEndpointsWithAssemblies(
        this IServiceCollection services,
        params Assembly[] assembly)
    {
        var servicesDescription = assembly
            .SelectMany(a => a.DefinedTypes
                .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndPoint)))
                .Select(type => ServiceDescriptor.Transient(typeof(IEndPoint), type))
                .ToArray());
        
        services.TryAddEnumerable(servicesDescription);
        
        return services;
    }
}