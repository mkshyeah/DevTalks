namespace DevTalks.Contracts.Questions;

public record UpdateQuestionDto(string Title, string Body, List<Guid> TagIds);