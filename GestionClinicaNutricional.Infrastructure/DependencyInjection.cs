using System.Reflection;
using GestionClinicaNutricional.Application;
using GestionClinicaNutricional.Domain.Repositories;
using GestionClinicaNutricional.Infrastructure.Repositories;
using Joseco.DDD.Core.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionClinicaNutricional.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddApplication();
            
            services.AddDbContext<DatabaseContext>(
                context =>
                    context.UseSqlServer(
                        "Server=localhost\\SQLEXPRESS;Database=ClinicaNutricional;MultipleActiveResultSets=True;Trusted_Connection=True;TrustServerCertificate=True"));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IConsultaInicialRepository, ConsultaInicialRepository>();
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IPlanAlimenticioRepository, PlanAlimenticioRepository>();
            
            services.AddMediatR(config =>
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
            );
            
            return services;
        }
    }
}