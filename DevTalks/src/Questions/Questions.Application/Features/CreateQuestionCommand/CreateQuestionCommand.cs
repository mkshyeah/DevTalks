using CSharpFunctionalExtensions;
using Questions.Contracts.Dto;
using Shared;
using Shared.Abstractions;

namespace Questions.Application.Features.CreateQuestionCommand;

public record CreateQuestionCommand(CreateQuestionDto QuestionDto) : ICommand;