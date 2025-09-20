using Shared;

namespace Questions.Application.Fails;

public partial class Errors
{
    public static class General
    {
        public static Error NotFoundRecord(Guid id) =>
            Error.NotFound("record.not.found", $"Запись {id} не найдена", id);
    }
    
    public static class Questions
    {
        public static Error TooManyQuestions() => 
            Error.Failure("questions.too.many", "Пользователь не может создать более 3 вопросов");
        
        public static Failure NotEnoughRating() =>
            Error.Failure("not.enough.rating", "Недостаточно рейтинга");
    }
}