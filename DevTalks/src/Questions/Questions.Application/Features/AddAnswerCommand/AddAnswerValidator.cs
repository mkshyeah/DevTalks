using FluentValidation;
using Questions.Contracts.Dto;

namespace Questions.Application.Features.AddAnswerCommand;

public class AddAnswerValidator: AbstractValidator<AddAnswerDto>
{
    public AddAnswerValidator()
    {
        
        RuleFor(x => x.Text).NotEmpty().WithMessage("Текст ответа не может быть пустым").
            MaximumLength(5000).WithMessage("Текст ответа не может быть больше 5000 символов");
        
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Id пользователя не может быть пустым");
    }
}