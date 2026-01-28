using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Toolbox.Application.Common.Interfaces;
using Toolbox.Infrastructure.Persistence;
using Toolbox.Infrastructure.Persistence.Interceptors;

namespace Toolbox.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register Interceptors (for any Audit Fields).
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        // Set up SQL Server with EF Core.
        string? connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            // Get the interceptors from the service provider.
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        // Register the Interface for the Application Layer.
        services.AddScoped<IApplicationDbContext>(provider => 
            provider.GetRequiredService<ApplicationDbContext>());

        // Configure Azure KeyVault & Managed Identity.
        // KeyVault is usually added to builder.Configuration in Program.cs, but the clients for it are registered here for Dependency Injection.
        string? keyVaultUri = configuration["Azure:KeyVaultUri"];
        if (!string.IsNullOrEmpty(keyVaultUri))
        {
            // This allows other services to inject SecretClient
            // services.AddSingleton(new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential()));
        }

        // Set up Azure Service Bus.
        ConfigureMessaging(services, configuration);

        // Add Database Seeding.
        services.AddScoped<ApplicationDbContextInitialiser>();

        return services;
    }

    private static void ConfigureMessaging(IServiceCollection services, IConfiguration configuration)
    {
        bool useServiceBus = configuration.GetValue<bool>("Features:UseAzureServiceBus");

        if (useServiceBus)
        {
            services.AddSingleton(_ =>
                new ServiceBusClient(
                    configuration.GetConnectionString("ServiceBus"),
                    new DefaultAzureCredential()));
        }
    }
}