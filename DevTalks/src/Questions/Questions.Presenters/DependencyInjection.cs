using Microsoft.Extensions.DependencyInjection;
using Questions.Application;
using Questions.Application.Features.AddAnswerCommand;
using Questions.Infrastructure.Postgres;

namespace Questions.Presenters;

public static class DependencyInjection
{
    public static IServiceCollection AddQuestionsModule(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddApplication();
        services.AddPostgresInfrastructure();
        
        return services;
    }
}