using CSharpFunctionalExtensions;
using Questions.Contracts.Dto;
using Shared;
using Shared.Abstractions;

namespace Questions.Application.Features.AddAnswerCommand;

public record AddAnswerCommand(Guid questionId, AddAnswerDto addAnswerDto) : ICommand;