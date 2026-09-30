using PaymentServiceApi.Models;

namespace PaymentServiceApi.Responses;

public class PaymentResultResponse
{
    public Guid Id { get; set; }
    public string OrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string? TransactionId { get; set; }
    public bool Success { get; set; }
    public string Status { get; set; } = string.Empty;

    public static PaymentResultResponse FromModel(Payment payment)
    {
        return new PaymentResultResponse
        {
            Id = payment.Id,
            OrderId = payment.OrderId,
            Amount = payment.Amount,
            TransactionId = payment.TransactionId,
            Success = payment.Status == Enums.PaymentStatus.Succeeded,
            Status = payment.Status.ToString()
        };
    }
}
