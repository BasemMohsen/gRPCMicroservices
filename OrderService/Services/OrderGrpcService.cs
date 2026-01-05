using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using OrderService.Contracts;
using OrderService.Domain;
using OrderService.Infrastructure;
using static OrderService.Contracts.OrderService;

public class OrderGrpcService : OrderServiceBase
{
    private readonly OrderDbContext _db;

    public OrderGrpcService(OrderDbContext db)
    {
        _db = db;
    }

    public override async Task<CreateOrderResponse> CreateOrder(
        CreateOrderRequest request,
        ServerCallContext context)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = request.CustomerId,
            Amount = request.Amount,
            Status = "PendingPayment",
            CreatedAt = DateTime.UtcNow
        };

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        return new CreateOrderResponse
        {
            OrderId = order.Id.ToString(),
            Status = order.Status
        };
    }

    public override async Task<Empty> MarkOrderAsPaid(
        MarkOrderAsPaidRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.OrderId, out var orderId))
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, "Invalid OrderId"));
        }

        var order = await _db.Orders.FindAsync(orderId);

        if (order == null)
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, "Order not found"));
        }

        if (order.Status == "Paid")
        {
            // idempotency-friendly behavior
            return new Empty();
        }

        order.Status = "Paid";
        await _db.SaveChangesAsync();

        return new Empty();
    }
}
