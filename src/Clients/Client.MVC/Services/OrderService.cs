using Client.MVC.Models.Orders;
using Client.MVC.Models.Payments;
using Client.MVC.Services.Interfaces;
using SharedLibrary.DTOs;
using SharedLibrary.Services;

namespace Client.MVC.Services;

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

        var orderCreatedVM = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();
        orderCreatedVM.Data.IsSuccessful = true;

        await _basketService.Delete();
        return orderCreatedVM.Data;
    }

    public async Task<List<OrderViewModel>> GetOrder()
    {
        var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");
        return response.Data;
    }

    public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
    {
        var basket = await _basketService.Get();

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

        var payment = new PaymentInfoInput()
        {
            CardName = checkoutInfoInput.CardName,
            CardNumber = checkoutInfoInput.CardNumber,
            Expiration = checkoutInfoInput.Expiration,
            CVV = checkoutInfoInput.CVV,
            TotalPrice = basket.TotalPrice,
            Order = orderCreateInput,
        };

        var responsePayment = await _paymentService.RecievePayment(payment);
        if (!responsePayment)
            return new OrderSuspendViewModel() { Error = "Payment could not be received!", IsSuccessful = false };

        await _basketService.Delete();
        return new OrderSuspendViewModel() { IsSuccessful = true };
    }
}
