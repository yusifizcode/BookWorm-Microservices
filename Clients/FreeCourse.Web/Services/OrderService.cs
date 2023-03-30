using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Models.Payments;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private readonly IBasketService _basketService;
    private readonly IPaymentService _paymentService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public OrderService(HttpClient httpClient,
                        IBasketService basketService,
                        IPaymentService paymentService,
                        ISharedIdentityService sharedIdentityService)
    {
        _httpClient = httpClient;
        _basketService = basketService;
        _paymentService = paymentService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
    {
        var basket = await _basketService.Get();
        var payment = new PaymentInfoInput()
        {
            CardName = checkoutInfoInput.CardName,
            CardNumber = checkoutInfoInput.CardNumber,
            Expiration = checkoutInfoInput.Expiration,
            CVV = checkoutInfoInput.CVV,
            TotalPrice = basket.TotalPrice,
        };

        var responsePayment = await _paymentService.RecievePayment(payment);
        if (!responsePayment)
            return new OrderCreatedViewModel() { Error = "Payment could not be received!", IsSuccessful = false };

        var orderCreateInput = new OrderCreateInput()
        {
            BuyerId = _sharedIdentityService.GetUserId,
            Address = new AddressCreateInput()
            {
                Province = checkoutInfoInput.Province,
                District = checkoutInfoInput.District,
                Street = checkoutInfoInput.Street,
                Line = checkoutInfoInput.Line,
                ZipCode = checkoutInfoInput.ZipCode
            },
        };

        basket.BasketItems.ForEach(x =>
        {
            var orderItem = new OrderItemCreateInput()
            {
                ProductId = x.CourseId,
                Price = x.GetCurrentPrice,
                PictureUrl = "",
                ProductName = x.CourseName
            };
            orderCreateInput.OrderItems.Add(orderItem);
        });

        var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

        if (!response.IsSuccessStatusCode)
            return new OrderCreatedViewModel() { Error = "Order could not be created!", IsSuccessful = false };

        var orderCreatedVM = await response.Content.ReadFromJsonAsync<OrderCreatedViewModel>();
        orderCreatedVM.IsSuccessful = true;

        return orderCreatedVM;
    }

    public async Task<List<OrderViewModel>> GetOrder()
    {
        var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
        return response.Data;
    }

    public Task SuspendOrder(CheckoutInfoInput checkoutInfoInput)
    {
        throw new NotImplementedException();
    }
}
