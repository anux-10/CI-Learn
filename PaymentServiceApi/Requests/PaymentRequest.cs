namespace PaymentServiceApi.Requests;

public class PaymentRequest
{
    public string? OrderId { get; set; }
    public string? Amount { get; set; }
    public string? PaymentMethod { get; set; }
    public string? CardNumber { get; set; }
    public string? CardNameHolder { get; set; }
    public string? Currency { get; set; } = "USD";
}
