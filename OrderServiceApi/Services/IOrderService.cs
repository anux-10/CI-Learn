using OrderServiceApi.Requests;
using OrderServiceApi.Responses;

namespace OrderServiceApi.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OrderResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OrderResponse> PayAsync(Guid orderId, PayOrderRequest request, CancellationToken cancellationToken = default);
}
