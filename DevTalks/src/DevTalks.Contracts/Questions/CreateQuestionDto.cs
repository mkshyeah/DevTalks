namespace DevTalks.Contracts.Questions;

public record CreateQuestionDto(string Title, string Text, Guid UserId, List<Guid> TagIds);