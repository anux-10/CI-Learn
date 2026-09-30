namespace OrderServiceApi.Requests;

public class PayOrderRequest
{
    public string PaymentMethod { get; set; } = "CreditCard";
    public string? CardNumber { get; set; }
    public string? CardHolderName { get; set; }
}
