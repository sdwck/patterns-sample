using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Domain.Common;
using WarehouseManager.Domain.Events;
using WarehouseManager.Domain.Interfaces;
using WarehouseManager.Infrastructure.Auth;
using WarehouseManager.Infrastructure.Events;
using WarehouseManager.Infrastructure.ExternalServices;
using WarehouseManager.Infrastructure.Notifications;
using WarehouseManager.Infrastructure.Persistence;
using WarehouseManager.Infrastructure.Persistence.Decorators;
using WarehouseManager.Infrastructure.Persistence.Repositories;

namespace WarehouseManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            // options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            options.UseInMemoryDatabase("WarehouseManagerDb"));

        services.AddScoped<ProductRepository>();
        services.AddScoped<IProductRepository>(sp =>
            new LoggingProductRepositoryDecorator(
                sp.GetRequiredService<ProductRepository>(),
                sp.GetRequiredService<ILogger<LoggingProductRepositoryDecorator>>()));
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IStockRepository, StockRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();

        services.AddScoped<IEventDispatcher, EventDispatcher>();
        services.AddScoped<IDomainEventHandler<OrderCreatedEvent>, OrderCreatedEventHandler>();
        services.AddScoped<IDomainEventHandler<LowStockEvent>, LowStockEventHandler>();
        services.AddScoped<IDomainEventHandler<OrderStatusChangedEvent>, OrderStatusChangedEventHandler>();

        services.AddSingleton<INotificationFactory, LogNotificationFactory>();

        services.AddSingleton<ExternalPaymentGateway>();
        services.AddScoped<IPaymentService, PaymentServiceAdapter>();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });

        services.AddAuthorization();

        return services;
    }
}