using OrderService.Contracts;

namespace PaymentService.Services;

public class PaymentProcessor
{
    private readonly OrderService.Contracts.OrderService.OrderServiceClient _orderClient;

    public PaymentProcessor(OrderService.Contracts.OrderService.OrderServiceClient orderClient)
    {
        _orderClient = orderClient;
    }

    public async Task ProcessPaymentAsync()
    {
        var response = await _orderClient.CreateOrderAsync(
            new CreateOrderRequest
            {
                CustomerId = "customer-123",
                Amount = 150
            });

        Console.WriteLine($"Order created: {response.OrderId}");
    }
}
