using FluentValidation;

namespace Toolbox.Application.Employees.Command.Create;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(e => e.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(e => e.LastName)
            .NotEmpty()
            .MaximumLength(50);

        // Date of Birth must be in the past
        RuleFor(v => v.DateOfBirth)
            .NotEmpty()
            .LessThan(DateTime.Today)
            .WithMessage("Date of Birth cannot be today or in the future.");

        // New Employee must be at least 16 years old.
        RuleFor(v => v.DateOfBirth)
            .Must(dob => dob <= DateTime.Today.AddYears(-16))
            .WithMessage("Employee must be at least 16 years old.");

        // Using a Regex to enforce a UK Format NI Number.
        RuleFor(v => v.NationalInsuranceNumber)
            .Matches(@"^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D]{1}$")
            .WithMessage("Invalid National Insurance number format.");

        RuleFor(v => v.Salary)
            .GreaterThan(0);

        // Start Date must be today or in the future.
        RuleFor(v => v.StartDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start Date cannot be in the past.");
    }
}
