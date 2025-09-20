using DevTalks.Application.Questions;
using DevTalks.Infrastructure.Postgres.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevTalks.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddDbContext<QuestionsReadDb>()
    }
}