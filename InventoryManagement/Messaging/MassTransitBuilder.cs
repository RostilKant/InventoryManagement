using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Entities.Settings;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InventoryManagement.Messaging
{
    public class MassTransitBuilder
    {
        private readonly IServiceCollection _services;

        private readonly List<Action<IBusRegistrationConfigurator>> _consumerSetup;

        public MassTransitBuilder(IServiceCollection services)
        {
            _services = services;
            _consumerSetup = new List<Action<IBusRegistrationConfigurator>>();
        }

        public MassTransitBuilder AddConsumer<T>() where T : class, IConsumer
        {
            _consumerSetup.Add(cfg => cfg.AddConsumer<T>());
            return this;
        }

        public void Build()
        {
            _services.AddMassTransit(cfg =>
            {
                cfg.AddBus(RabbitMQBusFactory);

                // Adds consumers
                foreach (var setupAction in _consumerSetup)
                {
                    setupAction.Invoke(cfg);
                }
            });
        }

        private static IBusControl RabbitMQBusFactory(IServiceProvider provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var settings = provider.GetRequiredService<IOptions<RabbitMQConnectionSettings>>().Value;

                cfg.Host(settings.Host, host =>
                {
                    host.Username(settings.Username);
                    host.Password(settings.Password);

                    // host.UseSsl(s =>
                    // {
                    //     s.Protocol = SslProtocols.Tls12;
                    // });
                });

                cfg.ConfigureJsonSerializerOptions(opt =>
                {
                    opt.ReferenceHandler = ReferenceHandler.Preserve;
                    return opt;
                });

                var context = provider.GetRequiredService<IBusRegistrationContext>();

                // Configures the endpoints by convention
                cfg.ConfigureEndpoints(context);
            });
        }
    }
}