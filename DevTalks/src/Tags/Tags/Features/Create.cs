using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tags.Contracts.Dtos;
using Tags.DataBase;
using Tags.Domain;

namespace Tags.Features;

public class Create
{
    public record CreateTagCommand(CreateTagDto CreateTagDto);
    
    public sealed class Handler
    {
        private readonly TagsDbContext _tagsDbContext;

        public Handler(TagsDbContext tagsDbContext)
        {
            _tagsDbContext = tagsDbContext;
        }

        public async Task<IResult> Handle(
            CreateTagCommand command)
        {
            var tag = new Tag
            {
                Name = command.CreateTagDto.Name,
            };
        
            await _tagsDbContext.AddAsync(tag);
            await _tagsDbContext.SaveChangesAsync();
            
            return Results.Ok(tag.Id);
        }
    }
    
    public sealed class EndPoint : IEndPoint
    {
        public void MapEndPoint(IEndpointRouteBuilder app)
        {
            app.MapPost("tags", async (
                CreateTagDto dto,
                Handler handler) =>
            {
                return await handler.Handle(new CreateTagCommand(dto));
            });
        }
    }
}