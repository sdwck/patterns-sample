using WarehouseManager.Application.Common.Interfaces;

namespace WarehouseManager.Infrastructure.ExternalServices;

public class PaymentServiceAdapter : IPaymentService
{
    private readonly ExternalPaymentGateway _gateway;

    public PaymentServiceAdapter(ExternalPaymentGateway gateway)
    {
        _gateway = gateway;
    }

    public Task<PaymentResult> ChargeAsync(Guid orderId, decimal amount, CancellationToken ct = default)
    {
        var response = _gateway.ProcessPayment("****", (double)amount);

        var result = new PaymentResult(
            response.Succeeded,
            response.TransactionRef,
            response.ErrorMessage);

        return Task.FromResult(result);
    }
}