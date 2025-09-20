namespace Questions.Contracts.Dto;

public record GetQuestionDto(string Search, List<Guid> TagIds, int Page, int PageSize);