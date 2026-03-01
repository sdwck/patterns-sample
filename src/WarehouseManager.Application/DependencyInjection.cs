using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WarehouseManager.Application.Common.Behaviours;
using WarehouseManager.Application.Services;

namespace WarehouseManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));

        services.AddScoped<Facade>();

        return services;
    }
}