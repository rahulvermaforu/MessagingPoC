using WebApi.Orders;

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