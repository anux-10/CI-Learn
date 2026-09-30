using System.Globalization;
using System.Net.Http.Json;
using OrderServiceApi.Models;
using OrderServiceApi.Repositories;
using OrderServiceApi.Requests;
using OrderServiceApi.Responses;

namespace OrderServiceApi.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly HttpClient _paymentHttpClient;

    public OrderService(IOrderRepository repository, HttpClient paymentHttpClient)
    {
        _repository = repository;
        _paymentHttpClient = paymentHttpClient;
    }

    public async Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Items.Count == 0)
            throw new ArgumentException("Order must have at least one item");

        var items = request.Items
            .Select(i => new OrderItem(i.ProductId, i.Quantity, i.UnitPrice))
            .ToList();

        var order = new Order(request.CustomerId, items);

        await _repository.AddAsync(order, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return OrderResponse.FromModel(order);
    }

    public async Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(id, cancellationToken);
        return order is null ? null : OrderResponse.FromModel(order);
    }

    public async Task<List<OrderResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _repository.GetAllAsync(cancellationToken);
        return orders.Select(OrderResponse.FromModel).ToList();
    }

    public async Task<OrderResponse> PayAsync(Guid orderId, PayOrderRequest request, CancellationToken cancellationToken = default)
    {
        var order = await _repository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException($"Order {orderId} not found.");

        if (order.PaidAt.HasValue)
            throw new InvalidOperationException("Order has already been paid.");

        var payload = new Dictionary<string, string>()
        {
            { "orderId", order.Id.ToString() },
            { "amount", order.TotalAmount.ToString(CultureInfo.InvariantCulture) },
            { "paymentMethod", request.PaymentMethod },
            { "cardNumber", request.CardNumber ?? string.Empty },
            { "cardNameHolder", request.CardHolderName ?? string.Empty },
            { "Currency", "USD" }
        };

        var response = await _paymentHttpClient.PostAsJsonAsync("Payment/process", payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Payment failed with status: {(int)response.StatusCode} {response.ReasonPhrase}. {body}");
        }

        var paymentResult = await response.Content.ReadFromJsonAsync<PaymentResponse>(cancellationToken);

        if (paymentResult is null)
        {
            var raw = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"Deserialization of payment response failed. Raw: {raw}");
        }

        if (!paymentResult.Success)
            throw new InvalidOperationException("Payment was declined");

        order.PaidAt = DateTime.UtcNow;
        order.PaymentReference = paymentResult.TransactionId;

        await _repository.UpdateAsync(order, cancellationToken);

        return OrderResponse.FromModel(order);
    }
}
