namespace Questions.Contracts.Dto;

public record CreateQuestionDto(string Title, string Text, Guid UserId, List<Guid> TagIds);