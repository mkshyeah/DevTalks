namespace DevTalks.Contracts;

public record UpdateQuestionDto(string Title, string Body, List<Guid> TagIds);