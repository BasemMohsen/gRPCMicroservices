using OrderService.Contracts;
using PaymentService.Domain;
using PaymentService.Infrastructure;
using static OrderService.Contracts.OrderService;

public class PaymentProcessor
{
    private readonly OrderServiceClient _orderClient;
    private readonly PaymentDbContext _db;

    public PaymentProcessor(
        OrderServiceClient orderClient,
        PaymentDbContext db)
    {
        _orderClient = orderClient;
        _db = db;
    }

    public async Task ProcessPaymentAsync()
    {
        // 1. Create order
        var orderResponse = await _orderClient.CreateOrderAsync(
            new CreateOrderRequest
            {
                CustomerId = "cust-1",
                Amount = 200
            });

        // 2. Save payment
        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = Guid.Parse(orderResponse.OrderId),
            Amount = 200,
            Status = "Completed",
            CreatedAt = DateTime.UtcNow
        };

        _db.Payments.Add(payment);
        await _db.SaveChangesAsync();

        // 3. Mark order as paid
        await _orderClient.MarkOrderAsPaidAsync(
            new MarkOrderAsPaidRequest
            {
                OrderId = orderResponse.OrderId
            });
    }
}
