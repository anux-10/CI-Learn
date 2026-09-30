using Microsoft.EntityFrameworkCore;
using PaymentServiceApi.Data;
using PaymentServiceApi.Models;

namespace PaymentServiceApi.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _db;

    public PaymentRepository(PaymentDbContext db)
    {
        _db = db;
    }

    public async Task<Payment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _db.Payments.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<List<Payment>> GetByOrderIdAsync(string orderId, CancellationToken cancellationToken = default)
    {
        return await _db.Payments
            .Where(p => p.OrderId == orderId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Payments
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        await _db.Payments.AddAsync(payment, cancellationToken);
    }

    public Task UpdateAsync(Payment payment, CancellationToken cancellationToken = default)
    {
        _db.Payments.Update(payment);
        return _db.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _db.SaveChangesAsync(cancellationToken);
    }
}
