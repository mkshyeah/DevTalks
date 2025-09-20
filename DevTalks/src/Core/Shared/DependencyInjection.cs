using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions;

namespace Shared;

public static class DependencyInjection
{
    /// <summary>
    /// Registers shared infrastructure and cross-cutting concerns.
    /// This method is intentionally minimal and only wires up Shared-level services (no application modules).
    /// </summary>
    public static IServiceCollection AddSharedDependencies(this IServiceCollection services)
    {
        // Nothing in Shared assembly needs scanning for handlers; keep shared light-weight.
        // If there are shared validators, register them; otherwise harmless.
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }

    /// <summary>
    /// Registers validators and handler implementations from the provided assemblies.
    /// Use this from module composition roots to avoid copy-paste scanning code.
    /// </summary>
    public static IServiceCollection AddModulePipeline(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
            return services;

        foreach (var asm in assemblies)
        {
            // Validators
            services.AddValidatorsFromAssembly(asm);

            // Command handlers (ICommandHandler<,> and ICommandHandler<>)
            services.Scan(scan => scan.FromAssemblies(new[] { asm })
                .AddClasses(classes => classes.AssignableToAny(typeof(ICommandHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());

            // Query handlers (IQueryHandler<,>)
            services.Scan(scan => scan.FromAssemblies(new[] { asm })
                .AddClasses(classes => classes.AssignableToAny(typeof(IQueryHandler<,>)))
                .AsSelfWithInterfaces()
                .WithScopedLifetime());
        }

        return services;
    }
}