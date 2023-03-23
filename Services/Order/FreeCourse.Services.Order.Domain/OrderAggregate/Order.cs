using FreeCourse.Services.Order.Domain.Core;

namespace FreeCourse.Services.Order.Domain.OrderAggregate;

public class Order : Entity, IAggregateRoot
{
    public DateTime CreatedTime { get; private set; }
    public Address Address { get; private set; }
    public string BuyerId { get; private set; }
    // EF Core Feature -> Backing Field (Field vasitesile set edilmenin qarshisini alib, ancaq oxumaq icazesi veririk)
    private readonly List<OrderItem> _orderItems;

    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public Order(string buyerId, Address address)
    {
        _orderItems = new List<OrderItem>();
        CreatedTime = DateTime.Now;
        BuyerId = buyerId;
        Address = address;
    }

    public void AddOrderItem(string productId, string productName, decimal price, string pictureUrl)
    {
        var existProduct = _orderItems.Any(x => x.Id == productId);

        if (!existProduct)
        {
            var newOrderItem = new OrderItem(productId, productName, pictureUrl, price);
            _orderItems.Add(newOrderItem);
        }
    }

    public decimal GetTotalPrice => _orderItems.Sum(x => x.Price);
}
