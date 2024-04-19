using System.Text;
using System.Text.Json.Serialization;
using WebApi.Orders;
using Newtonsoft.Json;

class OrderService
{
    private readonly ILogger<OrderService> _logger;

    public OrderService(ILogger<OrderService> logger)
    {
        _logger = logger;
    }
    private void LogOrder(Order order)
    {
        _logger.LogInformation("Order Received for {Item}", order);
    }

    public void ReceiveOrder(Order order)
    {
        LogOrder(order);

        var factory = new RabbitMQ.Client.ConnectionFactory(){
            HostName = "localhost",
            Port = 5672
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        var json = JsonConvert.SerializeObject(order);
        var bytes = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange: "", routingKey: "orders", body: bytes, mandatory: false, basicProperties: null);
    }
}