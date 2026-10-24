using System.Reflection;
using GestionClinicaNutricional.Application.UseCase.DomainEventHandler;
using GestionClinicaNutricional.Domain.Events;
using Joseco.Outbox.Contracts.Model;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace GestionClinicaNutricional.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //services.AddTransient<INotificationHandler<OutboxMessage<PacienteCreated>>, PublishIntegrationEventWhenPacienteCreated>();
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });
            
            return services;
        }
        
        public static IServiceCollection AddIntegrationMessaging(this IServiceCollection services)
        {
            services.AddTransient<INotificationHandler<OutboxMessage<PacienteCreated>>, PublishIntegrationEventWhenPacienteCreated>();

            return services;
        }
    }
}