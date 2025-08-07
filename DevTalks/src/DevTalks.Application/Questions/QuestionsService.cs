using DevTalks.Contracts.Questions;
using DevTalks.Domain.Questions;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DevTalks.Application.Questions;

public class QuestionsService : IQuestionsService
{
    private readonly IQuestionsRepository _questionsRepository;
    private readonly ILogger<QuestionsService> _logger;
    private readonly CreateQuestionValidator _validator;

    public QuestionsService(IQuestionsRepository questionsRepository, ILogger<QuestionsService> logger, CreateQuestionValidator validator)
    {
        _questionsRepository = questionsRepository;
        _logger = logger;
        _validator = new CreateQuestionValidator();
    }

    public async Task<Guid> Create(CreateQuestionDto questionDto, CancellationToken cancellationToken)
    {
        // Валидация
        
        // Валидация входных данных
        var validationResult = await _validator.ValidateAsync(questionDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        // Валидация бизнес логики
        int openUserQuestionsCount = await _questionsRepository
            .GetOpenQuestionsAsync(questionDto.UserId, cancellationToken);

        if (openUserQuestionsCount > 3)
        {
            throw new Exception("User has too many open questions");
        }
        
        // Создание сущности Question
        var questionId = Guid.NewGuid();
        
        var question = new Question(
            questionId,
            questionDto.Title,
            questionDto.Text,
            questionDto.UserId,
            null,
            questionDto.TagIds );
        
        // Сохранение сущности Question в БД
        await _questionsRepository.AddAsync(question, cancellationToken);
        
        // Логирование об успешном или неудачном создании 
        _logger.LogInformation("Question created with id: {QuestionId}", questionId);
        
        return questionId;
    }
}