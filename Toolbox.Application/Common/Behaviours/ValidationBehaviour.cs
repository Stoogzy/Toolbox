using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace Toolbox.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            ValidationContext<TRequest> validationContext = new(request);

            ValidationResult[] validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(validationContext, cancellationToken)));

            List<ValidationFailure> validationFailures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (validationFailures.Any())
            {
                throw new ValidationException(validationFailures);
            }
        }

        return await next();
    }
}
