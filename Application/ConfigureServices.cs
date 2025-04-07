using Application.Common.Behaviours;
using Application.Common.Interfaces.Logger;
using Application.Common.Interfaces.Services;
using Application.Common.Logger;
using Application.Common.Methodes;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Common.Behaviours.CheckPermissionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Common.Behaviours.ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserSessionBehaviour<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddScoped<ILoggerPointer, LoggerPointer>();
            services.AddScoped<ICheckPermissionPointer, CheckPermissionPointer>();


            return services;
        }
    }
}
