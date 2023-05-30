using FluentValidation;
using task2.DTOs;

namespace task2.BLL.Validations;

public class RateTheBookDtoValidation
    : AbstractValidator<RateBookDto>
{
    public RateTheBookDtoValidation()
    {
        RuleFor(prop => prop.Score)
            .InclusiveBetween(0, 10);
    }
}