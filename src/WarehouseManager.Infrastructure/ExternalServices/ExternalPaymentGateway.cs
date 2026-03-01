namespace WarehouseManager.Infrastructure.ExternalServices;

public class ExternalPaymentGateway
{
    public ExternalPaymentGatewayResponse ProcessPayment(string cardNumber, double amountUsd)
    {
        return new ExternalPaymentGatewayResponse
        {
            TransactionRef = Guid.NewGuid().ToString(),
            Succeeded = true
        };
    }
}

public class ExternalPaymentGatewayResponse
{
    public string TransactionRef { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public string? ErrorMessage { get; set; }
}