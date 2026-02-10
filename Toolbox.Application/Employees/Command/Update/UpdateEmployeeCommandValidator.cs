using FluentValidation;

namespace Toolbox.Application.Employees.Command.Update;

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("An Employee Id must be provided.");

        RuleFor(x => x.FirstName)
            .MaximumLength(50)
            .NotEmpty()
            .When(x => x.FirstName != null)
            .WithMessage("First Name exceeds maximum length of 50 characters.");

        RuleFor(x => x.LastName)
            .MaximumLength(50)
            .NotEmpty()
            .When(x => x.LastName != null)
            .WithMessage("Last Name exceeds maximum length of 50 characters.");

        RuleFor(x => x.Salary)
            .GreaterThan(0)
            .When(x => x.Salary.HasValue)
            .WithMessage("When trying to update an employee's salary, please provide a positive decimal.");
    }
}
