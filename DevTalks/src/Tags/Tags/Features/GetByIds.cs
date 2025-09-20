using Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions;
using Shared.FullTextSearch;
using Tags.Contracts.Dtos;
using Tags.DataBase;

namespace Tags.Features;

public class GetByIds
{
    public record GetByIdsQuery(GetByIdsDto GetByIdsDto) : IQuery;
    
    public sealed class Handler : IQueryHandler<IReadOnlyList<TagDto>, GetByIdsQuery>
    {
        private readonly TagsDbContext _tagsDbContext;

        public Handler(TagsDbContext tagsDbContext)
        {
            _tagsDbContext = tagsDbContext;
        }
        
        public async Task<IReadOnlyList<TagDto>> Handle(
            GetByIdsQuery query,
            CancellationToken cancellationToken)
        {
            var tags = await _tagsDbContext.Tags
                .Where(t => query.GetByIdsDto.Ids.Contains(t.Id))
                .ToListAsync(cancellationToken);
            
            return tags.Select(t => new TagDto(t.Id, t.Name)).ToList();
        }
    }
    
    public sealed class EndPoint : IEndPoint
    {
        public void MapEndPoint(IEndpointRouteBuilder app)
        {
            app.MapPost("tags/{id:guid}", async (
                GetByIdsDto getByIdsDto,
                IQueryHandler<IReadOnlyList<TagDto>, GetByIdsQuery> handler,
                CancellationToken cancellationToken) =>
            {
                var result = await handler.Handle(new GetByIdsQuery(getByIdsDto), cancellationToken);

                return Results.Ok(result);
            });
        }
    }
    
}