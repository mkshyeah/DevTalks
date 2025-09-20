using Questions.Application;
using Questions.Presenters;
using Tags.Presenters;
using Shared; // bring in AddSharedDependencies
using Shared.FilesStorage;
using Infrastructure.S3;

namespace Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddSharedDependencies(); // Shared DI
        services.AddQuestionsModule();
        services.AddTagsModule(); // DI for Tags module: DbContext, handlers, and ITagsContract

        // Files storage provider registration
        services.AddSingleton<IFilesProvider, S3Provider>();
        
        // Web-specific required services (controllers, OpenAPI)
        services.AddWebDependencies();
        
        return services;
    }

    private static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        
        return services;
    }
}