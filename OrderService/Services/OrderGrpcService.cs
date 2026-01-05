using Grpc.Core;
using OrderService.Contracts;

namespace OrderService.Services;

public class OrderGrpcService : OrderService.Contracts.OrderService.OrderServiceBase
{
    private static readonly Dictionary<string, (string Status, double Amount)> Orders = new();

    public override Task<CreateOrderResponse> CreateOrder(
        CreateOrderRequest request,
        ServerCallContext context)
    {
        if (request.Amount <= 0)
        {
            throw new RpcException(
                new Status(StatusCode.InvalidArgument, "Amount must be greater than zero"));
        }

        var orderId = Guid.NewGuid().ToString();

        Orders[orderId] = ("Created", request.Amount);

        return Task.FromResult(new CreateOrderResponse
        {
            OrderId = orderId,
            Status = "Created"
        });
    }

    public override Task<GetOrderResponse> GetOrder(
        GetOrderRequest request,
        ServerCallContext context)
    {
        if (!Orders.TryGetValue(request.OrderId, out var order))
        {
            throw new RpcException(
                new Status(StatusCode.NotFound, "Order not found"));
        }

        return Task.FromResult(new GetOrderResponse
        {
            OrderId = request.OrderId,
            Status = order.Status,
            Amount = order.Amount
        });
    }
}
