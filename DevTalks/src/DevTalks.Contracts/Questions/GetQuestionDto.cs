namespace DevTalks.Contracts.Questions;

public record GetQuestionDto(string Search, List<Guid> TagIds, int Page, int PageSize);