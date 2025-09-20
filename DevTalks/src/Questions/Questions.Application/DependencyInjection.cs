using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Questions.Application.Pipelines;
using Shared;
using Shared.Abstractions;

namespace Questions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        // Reuse shared helper to keep DI consistent
        services.AddModulePipeline(assembly);
        
        // services.AddMediatR(c =>
        // {
        //     c.RegisterServicesFromAssembly(assembly);
        //     
        //     c.AddOpenBehavior(typeof(ValidationBehavior<,>));
        // });
        return services;
    }
}