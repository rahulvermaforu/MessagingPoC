using Microsoft.AspNetCore.Mvc;
using WebApi.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Adding own Logger.
builder.Services.AddTransient<ILogger>(provider =>
{
    var factory = provider.GetRequiredService<ILoggerFactory>();
    return factory.CreateLogger("MyLogger");
});

// 2. Adding service with it's own logger.
builder.Services.AddSingleton<OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/order/create", (Order order, [FromServices] OrderService orderService, [FromServices] ILogger logger) =>
{
    app.Logger.LogInformation("Order Received for {Item}", order);
    logger.LogInformation("Order Received for {Item}", order);
    orderService.LogOrder(order);

    return order;
});

app.Run();