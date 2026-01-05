using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDb")));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<OrderGrpcService>();
app.MapGet("/", () => "OrderService is running");

app.Run();
