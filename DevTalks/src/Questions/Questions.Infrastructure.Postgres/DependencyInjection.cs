using Microsoft.Extensions.DependencyInjection;
using Questions.Application;
using Shared.DataBase;

namespace Questions.Infrastructure.Postgres;

public static class DependencyInjection
{
    public static IServiceCollection AddPostgresInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<QuestionsReadDbContext>();
        
        // Expose DbContext via read interface for query handlers
        services.AddScoped<IQuestionsReadDbContext>(sp => sp.GetRequiredService<QuestionsReadDbContext>());
        
        // Register repository implementation(s)
        // You can switch between EFCore and Dapper implementations by changing the mapping below
        services.AddScoped<IQuestionsRepository, QuestionsEFCoreRepository>();
        
        // Transaction manager for command handlers/services
        services.AddScoped<ITransactionManager, QuestionsTransactionManager>();
        
        return services;
    }
}