using PaymentServiceApi.Models;

namespace PaymentServiceApi.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Payment>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default);
    Task<List<Payment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Payment payment, CancellationToken cancellationToken = default);
    Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
