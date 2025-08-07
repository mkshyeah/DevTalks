using DevTalks.Domain.Questions;

namespace DevTalks.Application.Questions;

public interface IQuestionsRepository
{
    Task<Guid> AddAsync(Question question, CancellationToken cancellationToken);
    
    Task<Guid> SaveAsync(Question question, CancellationToken cancellationToken);
    
    Task<Guid> DeleteAsync(Guid questionId, CancellationToken cancellationToken);
    
    Task<Question> GetByGuidAsync(Guid questionId, CancellationToken cancellationToken);
    
    Task<int> GetOpenQuestionsAsync(Guid userId, CancellationToken cancellationToken);
}