using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Toolbox.Application.Common.Exceptions;

namespace Toolbox.Api;

public class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Handling Validation Errors: 400 Response.
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            Dictionary<string, string[]> errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            await httpContext.Response.WriteAsJsonAsync(new ValidationProblemDetails(errors), cancellationToken);

            return true;
        }

        // Handling Not Found Errors: 404 Response.
        if (exception is NotFoundException notFoundException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "The specified resource was not found.",
                Detail = notFoundException.Message
            }, cancellationToken);

            return true;
        }

        // Handling something we don't recognise: 500 Response.
        return false;
    }
}
