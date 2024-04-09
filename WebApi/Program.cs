using Microsoft.AspNetCore.Mvc;

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

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapPost("/order/create",async (Order order, [FromServices] OrderService orderService, [FromServices] ILogger logger) =>
{
    app.Logger.LogInformation("Order Received for {Item}", order);
    logger.LogInformation("Order Received for {Item}", order);
    orderService.LogOrder(order);
    
    return order;
});

app.Run();

internal record Order(string Name, decimal Price)
{
    public override string ToString() => $"{Name} ({Price:C})";
}

class OrderService
{
    private readonly ILogger<OrderService> _logger;

    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
    }
    public void LogOrder(Order order)
    {
        _logger.LogInformation("Order Received for {Item}", order);
    }
}


internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
