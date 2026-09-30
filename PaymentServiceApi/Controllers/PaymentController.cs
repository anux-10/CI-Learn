using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PaymentServiceApi.Models;
using PaymentServiceApi.Repositories;
using PaymentServiceApi.Requests;
using PaymentServiceApi.Responses;

namespace PaymentServiceApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentController : ControllerBase
{
    private readonly IPaymentRepository _repository;

    public PaymentController(IPaymentRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentResultResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var payments = await _repository.GetAllAsync(cancellationToken);
        return Ok(payments.Select(PaymentResultResponse.FromModel).ToList());
    }

    [HttpGet("count")]
    public async Task<ActionResult<int>> Count(CancellationToken cancellationToken)
    {
        var payments = await _repository.GetAllAsync(cancellationToken);
        return Ok(payments.Count);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentResultResponse>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetByIdAsync(id, cancellationToken);
        if (payment is null) return NotFound();
        return Ok(PaymentResultResponse.FromModel(payment));
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request, CancellationToken cancellationToken)
    {
        if (!decimal.TryParse(request.Amount, NumberStyles.Any, CultureInfo.InvariantCulture, out var amount) || amount <= 0)
            return BadRequest(new { Message = "Amount must be > 0." });

        var payment = new Payment(
            request.OrderId ?? string.Empty,
            amount,
            request.Currency ?? "USD",
            request.PaymentMethod ?? "CreditCard",
            request.CardNumber,
            request.CardNameHolder);

        // Simulate processing delay
        await Task.Delay(Random.Shared.Next(100, 500), cancellationToken);

        // Mock success/failure (90% success)
        var success = Random.Shared.NextDouble() > 0.1;

        if (!success)
        {
            // Do not persist failed payments — only successful ones are saved.
            return StatusCode(502, new { Message = "Payment processing failed." });
        }

        payment.MarkAsSucceeded($"txn_{Guid.NewGuid():N}"[^8..]);
        await _repository.AddAsync(payment, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Ok(new
        {
            TransactionId = payment.TransactionId,
            Success = true
        });
    }
}
