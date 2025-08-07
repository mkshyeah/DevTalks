using DevTalks.Application.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevTalks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<IQuestionsService, QuestionsService>();
        
        return services;
    }
}