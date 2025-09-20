using CSharpFunctionalExtensions;

namespace Shared.Communication;

public interface IUserCommunicationService
{
    Task<Result<long, Failure>> GetUserRating(Guid userId, CancellationToken cancellationToken);
}