using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Questions.Application;
using Questions.Application.Fails;
using Questions.Domain;
using Shared;

namespace Questions.Infrastructure.Postgres;

public class QuestionsEFCoreRepository : IQuestionsRepository
{
    private readonly QuestionsReadDbContext _readDbContext;

    public QuestionsEFCoreRepository(QuestionsReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    
    public async Task<Guid> AddAsync(Question question, CancellationToken cancellationToken)
    { 
        await _readDbContext.Questions.AddAsync(question, cancellationToken);
        
        await _readDbContext.SaveChangesAsync(cancellationToken);
        
        return question.Id;
    }

    public async Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken)
    {
        _readDbContext.Questions.Attach(question);
        await _readDbContext.SaveChangesAsync(cancellationToken);
        
        return question.Id;
    }

    public async Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Question, Failure>> GetByIdAsync(Guid questionId, CancellationToken cancellationToken)
    {
        var question = await _readDbContext.Questions
            .Include(q => q.Answers)
            .Include(q => q.Solution)
            .FirstOrDefaultAsync(q => q.Id == questionId, cancellationToken);

        if (question is null)
        {
            return Errors.General.NotFoundRecord(questionId).ToFailure();
        }

        return question;
    }

    public async Task<int> GetOpenQuestionsAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Count questions for the user where status is OPEN (i.e., not resolved)
        return await _readDbContext.Questions
            .Where(q => q.UserId == userId && q.Status == QuestionStatus.OPEN)
            .CountAsync(cancellationToken);
    }
}

