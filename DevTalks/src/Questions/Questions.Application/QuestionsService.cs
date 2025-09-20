using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Questions.Application.Fails;
using Questions.Contracts.Dto;
using Questions.Domain;
using Shared;
using Shared.DataBase;
using Shared.Extensions;
// using DevTalks.Application.Communication;

namespace Questions.Application;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly IValidator<CreateQuestionDto> _createQuestionDtoValidator;
    private readonly IValidator<AddAnswerDto> _addAnswerDtoValidator;
    private readonly ITransactionManager _transactionManager;
    // private readonly IUserCommunicationService _userCommunicationService;

    public QuestionsService(IQuestionsRepository questionsRepository, ILogger<QuestionsService> logger, IValidator<CreateQuestionDto> createQuestionDtoValidator, IValidator<AddAnswerDto> addAnswerDtoValidator, ITransactionManager transactionManager)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _addAnswerDtoValidator = addAnswerDtoValidator;
        _transactionManager = transactionManager;
        // _userCommunicationService = userCommunicationService;
        _createQuestionDtoValidator = createQuestionDtoValidator;
    }
    
    public async Task<Result<Guid, Failure>> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // Валидация

        // Валидация входных данных
        var validationResult = await _createQuestionDtoValidator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }

        // Валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            return Errors.Questions.TooManyQuestions().ToFailure();
        }

        // Создание сущности Question
        var questionId = Guid.NewGuid();

        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds);

        // Сохранение сущности Question в БД
        await _questionsRepository.AddAsync(question, cancellationToken);

        // Логирование об успешном или неудачном создании 
        _logger.LogInformation("Question created with id: {QuestionId}", questionId);

        return questionId;
    }

    public async Task<Result<Guid, Failure>> AddAnswer(
         Guid questionId,
         AddAnswerDto addAnswerDto,
         CancellationToken cancellationToken)
    {
        // Валидация
        
        // Валидация входных данных
        var validationResult = await _addAnswerDtoValidator.ValidateAsync(addAnswerDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return validationResult.ToErrors();
        }
        
        // var userRatingResult = await _userCommunicationService.GetUserRating(addAnswerDto.UserId, cancellationToken);
        //
        // // Валидация бизнес логики(Проверка рейтинга)
        // if (userRatingResult.IsFailure)
        // {
        //     return userRatingResult.Error;
        // }
        
        // if (userRatingResult.Value <= 0)
        // {
        //     _logger.LogError("User with id: {UserId} has not enough rating", addAnswerDto.UserId);
        //     return Errors.Questions.NotEnoughRating();
        // }
        
        // Валидация бизнес логики(Проверка наличия вопроса)
        var transaction = await _transactionManager.BeginTransactionAsync(cancellationToken); // Создание транзакции
        
        (_, bool isFailure, Question? question, Failure? error ) = await _questionsRepository.GetByIdAsync(questionId, cancellationToken);
        if (isFailure)
        {
            return error;
        }
        
        // Создание сущности Answer
        var answer = new Answer(Guid.NewGuid(), addAnswerDto.UserId, addAnswerDto.Text, questionId);
        
        // Сохранение сущности Answer в БД
        question.Answers.Add(answer);
        
        await _questionsRepository.SaveAsync(question, cancellationToken);
        
        transaction.Commit(); // Подтверждение транзакции
        
        // Логирование об успешном или неудачном создании
        _logger.LogInformation("Answer created with id: {AnswerId} to question with id: {QuestionId}", answer.Id, questionId);

        return answer.Id;
    }
}