using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LeaveManagement.Application
{
    public static class ConfigureApplicationRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
