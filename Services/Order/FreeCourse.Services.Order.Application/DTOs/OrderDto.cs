namespace FreeCourse.Services.Order.Application.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedTime { get; private set; }
    public AddressDto Address { get; private set; }
    public string BuyerId { get; private set; }
    public List<OrderItemDto> OrderItems { get; set; }
}
