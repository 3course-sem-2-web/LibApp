using FluentValidation;
using task2.DTOs;

namespace task2.BLL.Validations;

public class SaveANewBookDtoValidation
    : AbstractValidator<SaveANewBookDto>
{
    public SaveANewBookDtoValidation()
    {
        RuleFor(prop => prop.Title)
            .NotEmpty() // checking for nullability as well
            .MaximumLength(100);
        RuleFor(prop => prop.Content)
            .NotEmpty();
        RuleFor(prop => prop.Genre)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(prop => prop.Author)
            .NotEmpty()
            .MaximumLength(100);
        
    }
}