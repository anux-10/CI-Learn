namespace OrderServiceApi.Models;

public class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentReference { get; set; }
    public decimal TotalAmount => Items.Sum(i => i.Total);
    public List<OrderItem> Items { get; set; } = [];

    private Order() { }

    public Order(Guid customerId, IEnumerable<OrderItem> items)
    {
        if (!items.Any()) throw new ArgumentException("Order must have at least one item");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        CreatedAt = DateTime.UtcNow;
        Items = items.ToList();
    }

    public void MarkAsPaid(string paymentReference)
    {
        if (PaidAt.HasValue) throw new InvalidOperationException("Order is already paid.");

        PaidAt = DateTime.UtcNow;
        PaymentReference = paymentReference;
    }
}
