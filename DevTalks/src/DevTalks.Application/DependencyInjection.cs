﻿using DevTalks.Application.Abstractions;
using DevTalks.Application.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevTalks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.Scan(scan => scan.FromAssemblies([assembly])
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        services.Scan(scan => scan.FromAssemblies([assembly])
            .AddClasses(classes => classes
                .AssignableToAny(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}