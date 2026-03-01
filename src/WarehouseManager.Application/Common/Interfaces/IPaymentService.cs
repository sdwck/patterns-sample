namespace WarehouseManager.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<PaymentResult> ChargeAsync(Guid orderId, decimal amount, CancellationToken ct = default);
}

public record PaymentResult(bool Success, string TransactionId, string? Error);