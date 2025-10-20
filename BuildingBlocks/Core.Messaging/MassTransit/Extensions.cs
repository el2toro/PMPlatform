using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Core.Messaging.MassTransit;

public static class Extensions
{
    public static IServiceCollection
        AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly[]? assemblies = null)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            if (assemblies is not null && assemblies.Any())
            {
                config.AddConsumers(assemblies);
            }

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
