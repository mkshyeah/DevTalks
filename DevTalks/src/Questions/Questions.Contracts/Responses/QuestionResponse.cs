using Questions.Contracts.Dto;

namespace Questions.Contracts.Responses;

public record QuestionResponse(IEnumerable<QuestionDto> Questions, long TotalCount);