using FluentValidation;
using Questions.Contracts.Dto;

namespace Questions.Application.Features.CreateQuestionCommand;

public class CreateQuestionValidator: AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Текст вопроса не может быть пустым").
            MaximumLength(500).WithMessage("Текст вопроса не может быть больше 500 символов");
        
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Id пользователя не может быть пустым");
    }
}