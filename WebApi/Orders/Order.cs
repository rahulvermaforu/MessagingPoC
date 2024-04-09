namespace WebApi.Orders;

internal record Order(string Name, decimal Price)
{
    public override string ToString() => $"{Name} ({Price:C})";
}