using Microsoft.EntityFrameworkCore;
using Questions.Contracts.Dto;
using Questions.Contracts.Responses;
using Questions.Domain;
using Shared.Abstractions;
using Shared.FilesStorage;
using Tags.Contracts;
using Tags.Contracts.Dtos;

namespace Questions.Application.Features.GetQuestionsWithFiltersQuery;

public class GetQuestionsWithFiltersHandler : IQueryHandler<QuestionResponse, GetQuestionsWithFiltersQuery>
{
    private readonly IFilesProvider _filesProvider;
    private readonly IQuestionsReadDbContext _questionsReadDbContext;
    private readonly ITagsContract _tagsContract;
    
    
    public GetQuestionsWithFiltersHandler( IFilesProvider filesProvider, IQuestionsReadDbContext questionsReadDbContext, ITagsContract tagsContract)
    {
        _filesProvider = filesProvider;
        _questionsReadDbContext = questionsReadDbContext;
        _tagsContract = tagsContract;
    }

    public async Task<QuestionResponse> Handle(
        GetQuestionsWithFiltersQuery query,
        CancellationToken cancellationToken)
    {
        var questions = await _questionsReadDbContext.ReadQuestions
            .Skip(query.GetQuestionDto.Page * query.GetQuestionDto.PageSize)
            .Take(query.GetQuestionDto.PageSize)
            .ToListAsync(cancellationToken);

        long count = await _questionsReadDbContext.ReadQuestions.LongCountAsync(cancellationToken);

        var screenshotIdsList = new List<Guid>();
        foreach (var q in questions)
        {
            if (q.ScreenshotId.HasValue)
            {
                screenshotIdsList.Add(q.ScreenshotId.Value);
            }
        }

        var filesDict = await _filesProvider.GetUrlsByIdsAsync(screenshotIdsList, cancellationToken);

        var tagIdsList = new List<Guid>();
        foreach (var q in questions)
        {
            if (q.Tags != null && q.Tags.Count > 0)
            {
                tagIdsList.AddRange(q.Tags);
            }
        }

        var tags = await _tagsContract.GetByIds(new GetByIdsDto(tagIdsList.ToArray()));

        var questionsDto = questions.Select(q => new QuestionDto(
            q.Id,
            q.Title,
            q.Text,
            q.UserId,
            (q.ScreenshotId.HasValue && filesDict.TryGetValue(q.ScreenshotId.Value, out var url)) ? url : string.Empty,
            q.Solution?.Id, 
            tags.Select(t => t.Name),
            q.Status.ToRussianString()));
        
        return new QuestionResponse(questionsDto, count);
    }
}