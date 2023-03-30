using FreeCourse.Web.Models.Orders;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers;

public class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly IBasketService _basketService;

    public OrderController(IOrderService orderService,
                           IBasketService basketService)
    {
        _orderService = orderService;
        _basketService = basketService;
    }

    public async Task<IActionResult> Checkout()
    {
        var basket = await _basketService.Get();
        ViewBag.Basket = basket;

        return View(new CheckoutInfoInput());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
    {
        // 1. Sync connection 
        // var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);

        // 2. Async connection
        var orderSuspend = await _orderService.SuspendOrder(checkoutInfoInput);

        if (!orderSuspend.IsSuccessful)
        {
            var basket = await _basketService.Get();
            ViewBag.Basket = basket;
            ViewBag.Error = orderSuspend.Error;

            return RedirectToAction(nameof(Checkout));
        }

        // 1. Sync connection
        // return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderSuspend.OrderId });

        // 2. Async connection
        return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = new Random().Next(1, 1000) });
    }

    public async Task<IActionResult> SuccessfullCheckout(int orderId)
    {
        ViewBag.OrderId = orderId;
        return View();
    }
}
