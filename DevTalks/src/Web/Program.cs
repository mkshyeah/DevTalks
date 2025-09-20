using Framework;
using Tags;
using Web;
using Web.MiddleWares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProgramDependencies();

builder.Services.AddEndpoints(TagsAssembly.Assembly);

var app = builder.Build();

app.UseExceptionMiddleWare();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "DevTalks"));
}

app.MapControllers();

app.MapEndpoints();

app.Run();