using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate;

public class OrderItem : Entity
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string PictureUrl { get; private set; }
    public decimal Price { get; private set; }
    // EF Core Feature -> Shadow Property (OrderId olmasa da elave edir)
    public OrderItem(string productId, string productName, string pictureUrl, decimal price)
    {
        Id = productId;
        Name = productName;
        PictureUrl = pictureUrl;
        Price = price;
    }

    public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
    {
        Name = productName;
        PictureUrl = pictureUrl;
        Price = price;
    }
}
