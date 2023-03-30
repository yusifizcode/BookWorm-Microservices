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
        var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);
        if (!orderStatus.IsSuccessful)
        {
            var basket = await _basketService.Get();
            ViewBag.Basket = basket;
            ViewBag.Error = orderStatus.Error;

            return RedirectToAction(nameof(Checkout));
        }

        return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = orderStatus.OrderId });
    }

    public async Task<IActionResult> SuccessfullCheckout(int orderId)
    {
        ViewBag.OrderId = orderId;
        return View();
    }
}
