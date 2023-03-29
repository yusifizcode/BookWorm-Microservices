namespace FreeCourse.Web.Models.Orders;

public class OrderViewModel
{
    public int Id { get; set; }
    public DateTime CreatedTime { get; set; }

    // Odeme gecmishinde adres alanina ihtiyac olmadigindan dolayi alinmadi
    //public AddressDto Address { get; private set; }
    public string BuyerId { get; set; }
    public List<OrderItemViewModel> OrderItems { get; set; }
}
