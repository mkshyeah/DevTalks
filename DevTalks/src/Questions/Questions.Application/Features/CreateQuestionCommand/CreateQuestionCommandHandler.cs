using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Questions.Application.Fails;
using Questions.Contracts.Dto;
using Questions.Domain;
using Shared;
using Shared.Abstractions;
using Shared.Extensions;

namespace Questions.Application.Features.CreateQuestionCommand;

public class CreateQuestionCommandHandler : ICommandHandler<Guid, CreateQuestionCommand>
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<CreateQuestionCommandHandler> _logger;
    private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;
    
    public CreateQuestionCommandHandler(IQuestionsRepository questionsRepository, ILogger<CreateQuestionCommandHandler> logger, IValidator<CreateQuestionDto> createQuestionDtoValidator)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _createQuestionDtoValidator = createQuestionDtoValidator;
    }
    
    public async Task<Result<Guid, Failure>> Handle(CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        // Валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenQuestionsAsync(command.QuestionDto.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            return Errors.Questions.TooManyQuestions().ToFailure();
        }

        // Создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            command.QuestionDto.Title,
            command.QuestionDto.Text,
            command.QuestionDto.UserId,
            null,
            command.QuestionDto.TagIds);

        // Сохранение сущности Question в БД
        await _questionsRepository.AddAsync(question, cancellationToken);

        // Логирование об успешном или неудачном создании 
        _logger.LogInformation("Question created with id: {QuestionId}", questionId);

        return questionId;
    }
}