using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WarehouseManager.Application.Features.Products.Commands.CreateProduct;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;

namespace WarehouseManager.UnitTests;

public class TemplateMethodTests
{
    private readonly CreateProductCommandHandler _handler;
    private readonly Mock<IUnitOfWork> _uowMock;

    public TemplateMethodTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        var loggerMock = new Mock<ILogger<CreateProductCommandHandler>>();
        _handler = new CreateProductCommandHandler(_uowMock.Object, loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenSkuExists_ShouldFailAtValidationStep()
    {
        var command = new CreateProductCommand("Test", null, "EXISTING-SKU", 10m, Guid.NewGuid(), null, 5);

        _uowMock.Setup(u => u.Products.GetBySkuAsync("EXISTING-SKU", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Sku = "EXISTING-SKU" });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("SKU already exists");
    }

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ShouldFailAtValidationStep()
    {
        var command = new CreateProductCommand("Test", null, "NEW-SKU", 10m, Guid.NewGuid(), null, 5);

        _uowMock.Setup(u => u.Products.GetBySkuAsync("NEW-SKU", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        _uowMock.Setup(u => u.Categories.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Category?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Category not found");
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldExecuteAndReturnId()
    {
        var categoryId = Guid.NewGuid();
        var command = new CreateProductCommand("Widget", "A widget", "WDG-001", 25m, categoryId, null, 10);

        _uowMock.Setup(u => u.Products.GetBySkuAsync("WDG-001", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        _uowMock.Setup(u => u.Categories.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = categoryId, Name = "Test Category" });

        _uowMock.Setup(u => u.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product p, CancellationToken _) => p);

        _uowMock.Setup(u => u.Stock.AddAsync(It.IsAny<Stock>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Stock s, CancellationToken _) => s);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_WhenValid_ShouldCallSaveChanges()
    {
        var categoryId = Guid.NewGuid();
        var command = new CreateProductCommand("Widget", null, "WDG-002", 15m, categoryId, null, 0);

        _uowMock.Setup(u => u.Products.GetBySkuAsync("WDG-002", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        _uowMock.Setup(u => u.Categories.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Category { Id = categoryId, Name = "Cat" });

        _uowMock.Setup(u => u.Products.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product p, CancellationToken _) => p);

        _uowMock.Setup(u => u.Stock.AddAsync(It.IsAny<Stock>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Stock s, CancellationToken _) => s);

        await _handler.Handle(command, CancellationToken.None);

        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldNotCallSaveChanges()
    {
        var command = new CreateProductCommand("Test", null, "DUP", 10m, Guid.NewGuid(), null, 0);

        _uowMock.Setup(u => u.Products.GetBySkuAsync("DUP", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product { Sku = "DUP" });

        await _handler.Handle(command, CancellationToken.None);

        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}