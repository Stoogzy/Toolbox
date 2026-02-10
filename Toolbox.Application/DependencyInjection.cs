using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Toolbox.Application.Common.Behaviours;

namespace Toolbox.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR.
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);

            // Hook Validation behaviour up to MediatR.
            config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        // Register FluentValidation validators.
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
