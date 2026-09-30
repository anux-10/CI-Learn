using PaymentServiceApi.Enums;

namespace PaymentServiceApi.Models;

public class Payment
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string PaymentMethod { get; set; } = "CreditCard";
    public string? CardNumber { get; set; }
    public string? CardNameHolder { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }

    private Payment() { }

    public Payment(string orderId, decimal amount, string currency, string paymentMethod, string? cardNumber, string? cardNameHolder)
    {
        if (string.IsNullOrWhiteSpace(orderId)) throw new ArgumentException("OrderId is required.");
        if (amount <= 0) throw new ArgumentException("Amount must be > 0.");

        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Currency = string.IsNullOrWhiteSpace(currency) ? "USD" : currency;
        PaymentMethod = string.IsNullOrWhiteSpace(paymentMethod) ? "CreditCard" : paymentMethod;
        CardNumber = cardNumber;
        CardNameHolder = cardNameHolder;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsSucceeded(string transactionId)
    {
        Status = PaymentStatus.Succeeded;
        TransactionId = transactionId;
        ProcessedAt = DateTime.UtcNow;
    }

    public void MarkAsFailed()
    {
        Status = PaymentStatus.Failed;
        ProcessedAt = DateTime.UtcNow;
    }
}
