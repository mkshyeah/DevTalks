namespace DevTalks.Contracts;

public record GetQuestionDto(string Search, List<Guid> TagIds, int Page, int PageSize);