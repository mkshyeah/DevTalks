using Shared.Exceptions;

namespace Questions.Application.Fails.Exceptions;

public class TooManyQuestionsException : BadRequestException
{
    public TooManyQuestionsException() 
        : base([Errors.Questions.TooManyQuestions()])
    {
    }
}