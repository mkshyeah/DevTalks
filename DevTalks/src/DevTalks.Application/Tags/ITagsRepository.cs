namespace DevTalks.Application.Tags;

public interface ITagsRepository
{
    public Task<IReadOnlyList<string>> GetTagsAsync(IEnumerable<Guid> tagIds, CancellationToken cancellationToken);
}