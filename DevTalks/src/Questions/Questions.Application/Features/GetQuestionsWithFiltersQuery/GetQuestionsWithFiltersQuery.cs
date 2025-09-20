using Questions.Contracts.Dto;
using Shared.Abstractions;

namespace Questions.Application.Features.GetQuestionsWithFiltersQuery;

public record GetQuestionsWithFiltersQuery(GetQuestionDto GetQuestionDto) : IQuery;