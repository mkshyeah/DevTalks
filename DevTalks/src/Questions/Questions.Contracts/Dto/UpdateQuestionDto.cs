namespace Questions.Contracts.Dto;

public record UpdateQuestionDto(string Title, string Body, List<Guid> TagIds);