using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Infrastructure.Notifications;

namespace WarehouseManager.UnitTests;

public class AbstractFactoryTests
{
    [Fact]
    public void LogFactory_CreateEmailService_ShouldReturnLogEmailService()
    {
        var factory = CreateLogFactory();

        var emailService = factory.CreateEmailService();

        emailService.Should().NotBeNull();
        emailService.Should().BeOfType<LogEmailService>();
    }

    [Fact]
    public void LogFactory_CreateSmsService_ShouldReturnLogSmsService()
    {
        var factory = CreateLogFactory();

        var smsService = factory.CreateSmsService();

        smsService.Should().NotBeNull();
        smsService.Should().BeOfType<LogSmsService>();
    }

    [Fact]
    public void SmtpFactory_CreateEmailService_ShouldReturnSmtpEmailService()
    {
        var factory = CreateSmtpFactory();

        var emailService = factory.CreateEmailService();

        emailService.Should().NotBeNull();
        emailService.Should().BeOfType<SmtpEmailService>();
    }

    [Fact]
    public void SmtpFactory_CreateSmsService_ShouldReturnTwilioSmsService()
    {
        var factory = CreateSmtpFactory();

        var smsService = factory.CreateSmsService();

        smsService.Should().NotBeNull();
        smsService.Should().BeOfType<TwilioSmsService>();
    }

    [Fact]
    public void BothFactories_ShouldImplementSameInterface()
    {
        var logFactory = CreateLogFactory();
        var smtpFactory = CreateSmtpFactory();

        logFactory.Should().BeAssignableTo<INotificationFactory>();
        smtpFactory.Should().BeAssignableTo<INotificationFactory>();
    }

    [Fact]
    public void LogFactory_Products_ShouldImplementCorrectInterfaces()
    {
        var factory = CreateLogFactory();

        factory.CreateEmailService().Should().BeAssignableTo<IEmailNotificationService>();
        factory.CreateSmsService().Should().BeAssignableTo<ISmsNotificationService>();
    }

    [Fact]
    public void SmtpFactory_Products_ShouldImplementCorrectInterfaces()
    {
        var factory = CreateSmtpFactory();

        factory.CreateEmailService().Should().BeAssignableTo<IEmailNotificationService>();
        factory.CreateSmsService().Should().BeAssignableTo<ISmsNotificationService>();
    }

    [Fact]
    public async Task LogEmailService_SendAsync_ShouldNotThrow()
    {
        var factory = CreateLogFactory();
        var emailService = factory.CreateEmailService();

        var act = () => emailService.SendAsync("test@test.com", "Subject", "Body");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task LogSmsService_SendAsync_ShouldNotThrow()
    {
        var factory = CreateLogFactory();
        var smsService = factory.CreateSmsService();

        var act = () => smsService.SendAsync("+1234567890", "Hello");

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public void EachFactory_ShouldCreateNewInstancesPerCall()
    {
        var factory = CreateLogFactory();

        var email1 = factory.CreateEmailService();
        var email2 = factory.CreateEmailService();

        email1.Should().NotBeSameAs(email2);
    }

    private static ILoggerFactory CreateMockLoggerFactory()
    {
        var loggerFactory = new Mock<ILoggerFactory>();
        loggerFactory
            .Setup(f => f.CreateLogger(It.IsAny<string>()))
            .Returns(new Mock<ILogger>().Object);
        return loggerFactory.Object;
    }

    private static LogNotificationFactory CreateLogFactory()
    {
        return new LogNotificationFactory(CreateMockLoggerFactory());
    }

    private static SmtpNotificationFactory CreateSmtpFactory()
    {
        return new SmtpNotificationFactory(CreateMockLoggerFactory());
    }
}