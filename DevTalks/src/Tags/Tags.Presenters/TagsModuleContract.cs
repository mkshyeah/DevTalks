using Shared.Abstractions;
using Tags.Contracts;
using Tags.Contracts.Dtos;
using Tags.DataBase;
using Tags.Features;

namespace Tags.Presenters;

public class TagsModuleContract : ITagsContract
{
    private readonly TagsDbContext _tagsDbContext;
    private readonly IQueryHandler<IReadOnlyList<TagDto>, GetByIds.GetByIdsQuery> _handler;

    public TagsModuleContract(TagsDbContext tagsDbContext, IQueryHandler<IReadOnlyList<TagDto>, GetByIds.GetByIdsQuery> handler)
    {
        _tagsDbContext = tagsDbContext;
        _handler = handler;
    }
    
    public async Task CreateTag(CreateTagDto dto)
    {
        var handler = new Create.Handler(_tagsDbContext);
        await handler.Handle(new Create.CreateTagCommand(dto));
    }

    public async Task<IReadOnlyList<TagDto>> GetByIds(GetByIdsDto dto)
    {
        return await _handler.Handle(new GetByIds.GetByIdsQuery(dto));
    }
    
}