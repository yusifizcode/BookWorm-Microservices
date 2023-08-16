using Client.MVC.Models.Orders;
using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Client.MVC.Controllers;

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
        return RedirectToAction(nameof(SuccessfullCheckout), new { orderId = new Random().Next(1, 1000), checkoutInfoInput = checkoutInfoInput });
    }

    public async Task<IActionResult> SuccessfullCheckout(int orderId)
    {
        ViewBag.OrderId = orderId;
        return View();
    }

    public async Task<IActionResult> CheckoutHistory(int page = 1)
    {
        ViewBag.Page = page;
        var orderVMs = await _orderService.GetOrder();

        ViewBag.TotalPages = (int)Math.Ceiling(orderVMs.Count() / 5d);

        if (orderVMs.Count > 0)
        {
            if (page < 1 || page > (int)Math.Ceiling(orderVMs.Count() / 5d))
                return NotFound();
        }

        return View(orderVMs.Skip((page - 1) * 5).Take(5).ToList());
    }
}
