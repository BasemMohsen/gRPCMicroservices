//using PaymentService.Services;
using Microsoft.EntityFrameworkCore;
using PaymentService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcClient<OrderService.Contracts.OrderService.OrderServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5077"); // Address of the OrderService
});

builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PaymentDb")));


builder.Services.AddScoped<PaymentProcessor>();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var processor = scope.ServiceProvider
        .GetRequiredService<PaymentProcessor>();

    await processor.ProcessPaymentAsync();
}
//var processor = app.Services.GetRequiredService<PaymentProcessor>();
//await processor.ProcessPaymentAsync();

// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
