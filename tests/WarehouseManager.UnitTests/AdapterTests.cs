using FluentAssertions;
using WarehouseManager.Application.Common.Interfaces;
using WarehouseManager.Infrastructure.ExternalServices;

namespace WarehouseManager.UnitTests;

public class AdapterTests
{
    [Fact]
    public void PaymentServiceAdapter_ShouldImplementIPaymentService()
    {
        var adapter = CreateAdapter();

        adapter.Should().BeAssignableTo<IPaymentService>();
    }

    [Fact]
    public async Task ChargeAsync_ShouldAdaptExternalGatewayResponse()
    {
        var adapter = CreateAdapter();

        var result = await adapter.ChargeAsync(Guid.NewGuid(), 100.50m);

        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.TransactionId.Should().NotBeNullOrEmpty();
        result.Error.Should().BeNull();
    }

    [Fact]
    public async Task ChargeAsync_ShouldAcceptDecimalAmount()
    {
        var adapter = CreateAdapter();

        var result = await adapter.ChargeAsync(Guid.NewGuid(), 99999.99m);

        result.Success.Should().BeTrue();
    }

    [Fact]
    public async Task ChargeAsync_ShouldAcceptGuidOrderId()
    {
        var adapter = CreateAdapter();
        var orderId = Guid.NewGuid();

        var act = () => adapter.ChargeAsync(orderId, 50m);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public void ExternalGateway_UsesDoubleAndString_AdapterConvertsToDecimalAndGuid()
    {
        var gateway = new ExternalPaymentGateway();

        var response = gateway.ProcessPayment("4111111111111111", 100.0);

        response.Succeeded.Should().BeTrue();
        response.TransactionRef.Should().NotBeNullOrEmpty();
    }

    private static PaymentServiceAdapter CreateAdapter()
    {
        return new PaymentServiceAdapter(new ExternalPaymentGateway());
    }
}