using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using WarehouseManager.Domain.Entities;
using WarehouseManager.Domain.Interfaces;
using WarehouseManager.Infrastructure.Persistence.Decorators;

namespace WarehouseManager.UnitTests;

public class DecoratorTests
{
    private readonly LoggingProductRepositoryDecorator _decorator;
    private readonly Mock<IProductRepository> _innerMock;
    private readonly Mock<ILogger<LoggingProductRepositoryDecorator>> _loggerMock;

    public DecoratorTests()
    {
        _innerMock = new Mock<IProductRepository>();
        _loggerMock = new Mock<ILogger<LoggingProductRepositoryDecorator>>();
        _decorator = new LoggingProductRepositoryDecorator(_innerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Decorator_ShouldImplementSameInterface()
    {
        _decorator.Should().BeAssignableTo<IProductRepository>();
    }

    [Fact]
    public async Task GetByIdAsync_ShouldDelegateToInner()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Name = "Test" };
        _innerMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var result = await _decorator.GetByIdAsync(id);

        result.Should().Be(product);
        _innerMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldDelegateToInner()
    {
        var products = new List<Product> { new() { Name = "A" }, new() { Name = "B" } };
        _innerMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var result = await _decorator.GetAllAsync();

        result.Should().HaveCount(2);
        _innerMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_ShouldDelegateToInner()
    {
        var product = new Product { Name = "New", Sku = "NEW-001" };
        _innerMock.Setup(r => r.AddAsync(product, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var result = await _decorator.AddAsync(product);

        result.Should().Be(product);
        _innerMock.Verify(r => r.AddAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Update_ShouldDelegateToInner()
    {
        var product = new Product { Name = "Updated" };

        _decorator.Update(product);

        _innerMock.Verify(r => r.Update(product), Times.Once);
    }

    [Fact]
    public void Delete_ShouldDelegateToInner()
    {
        var product = new Product { Name = "ToDelete" };

        _decorator.Delete(product);

        _innerMock.Verify(r => r.Delete(product), Times.Once);
    }

    [Fact]
    public async Task GetBySkuAsync_ShouldDelegateToInner()
    {
        var product = new Product { Sku = "SKU-001" };
        _innerMock.Setup(r => r.GetBySkuAsync("SKU-001", It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var result = await _decorator.GetBySkuAsync("SKU-001");

        result.Should().Be(product);
    }

    [Fact]
    public async Task GetByCategoryIdAsync_ShouldDelegateToInner()
    {
        var categoryId = Guid.NewGuid();
        var products = new List<Product> { new() { Name = "P1" } };
        _innerMock.Setup(r => r.GetByCategoryIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        var result = await _decorator.GetByCategoryIdAsync(categoryId);

        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task Decorator_ShouldNotAlterReturnValues()
    {
        _innerMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var result = await _decorator.GetByIdAsync(Guid.NewGuid());

        result.Should().BeNull();
    }
}