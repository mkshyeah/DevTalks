using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Questions.Application.Fails;
using Questions.Contracts.Dto;
using Questions.Domain;
using Shared;
using Shared.Abstractions;
using Shared.DataBase;
using Shared.Extensions;

namespace Questions.Application.Features.AddAnswerCommand;

public class AddAnswerCommandHandler : ICommandHandler<Guid, AddAnswerCommand>
{
    private readonly IValidator<AddAnswerDto> _addAnswerDtoValidator;
    //private readonly IUserCommunicationService _userCommunicationService;
    private readonly ILogger<AddAnswerCommandHandler> _logger;
    private readonly ITransactionManager _transactionManager;
    private readonly IQuestionsRepository _questionsRepository;
        
    public AddAnswerCommandHandler(IValidator<AddAnswerDto> addAnswerDtoValidator, ILogger<AddAnswerCommandHandler> logger, ITransactionManager transactionManager, IQuestionsRepository questionsRepository)
    {
        _addAnswerDtoValidator = addAnswerDtoValidator;
        //_userCommunicationService = userCommunicationService;
        _logger = logger;
        _transactionManager = transactionManager;
        _questionsRepository = questionsRepository;
    }
    
    public async Task<Result<Guid, Failure>> Handle(AddAnswerCommand command, CancellationToken cancellationToken)
    {
        
        //var userRatingResult = await _userCommunicationService.GetUserRating(command.addAnswerDto.UserId, cancellationToken);

        // Валидация бизнес логики(Проверка рейтинга)
        // if (userRatingResult.IsFailure)
        // {
        //     return userRatingResult.Error;
        // }
        //
        // if (userRatingResult.Value <= 0)
        {
            _logger.LogError("User with id: {UserId} has not enough rating", command.addAnswerDto.UserId);
            return Errors.Questions.NotEnoughRating();
        }
        
        // Валидация бизнес логики(Проверка наличия вопроса)
        var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken); // Создание транзакции
        
        (_, bool isFailure, Question? question, Failure? error ) = await _questionsRepository.GetByIdAsync(command.questionId, cancellationToken);
        if (isFailure)
        {
            return error;
        }
        
        // Создание сущности Answer
        var answer = new Answer(Guid.NewGuid(), command.addAnswerDto.UserId, command.addAnswerDto.Text, command.questionId);
        
        // Сохранение сущности Answer в БД
        question.Answers.Add(answer);
        
        await _questionsRepository.SaveAsync(question, cancellationToken);
        
        transaction.Commit(); // Подтверждение транзакции
        
        // Логирование об успешном или неудачном создании
        _logger.LogInformation("Answer created with id: {AnswerId} to question with id: {QuestionId}", answer.Id, command.questionId);

        return answer.Id;
    }
}