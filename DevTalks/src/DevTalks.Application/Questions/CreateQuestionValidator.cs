using DevTalks.Contracts.Questions;
using FluentValidation;

namespace DevTalks.Application.Questions;

public class CreateQuestionValidator: AbstractValidator<CreateQuestionDto>
{
    public CreateQuestionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500).WithMessage("Title is invalid");
        
        RuleFor(x => x.Text).NotEmpty().MaximumLength(5000).WithMessage("Text is invalid");
        
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is invalid");
        
        RuleForEach(x => x.TagIds).NotEmpty().WithMessage("Tag is invalid");
    }
}