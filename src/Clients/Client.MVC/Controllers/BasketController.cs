using Client.MVC.Models.Baskets;
using Client.MVC.Models.Discounts;
using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.MVC.Controllers;
[Authorize]
public class BasketController : Controller
{
    private readonly IBasketService _basketService;
    private readonly ICatalogService _catalogService;

    public BasketController(IBasketService basketService,
                            ICatalogService catalogService)
    {
        _basketService = basketService;
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index()
        => View(await _basketService.Get());

    public async Task<IActionResult> AddBasketItem(string productId)
    {
        var product = await _catalogService.GetByProductIdAsync(productId);

        var basketItemVM = new BasketItemViewModel
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Price = product.Price,
        };

        await _basketService.AddBasketItem(basketItemVM);

        return RedirectToAction("Index", "Basket");
    }

    public async Task<IActionResult> RemoveBasketItem(string productId)
    {
        await _basketService.RemoveBasketItem(productId);
        return RedirectToAction("Index", "Basket");
    }

    public async Task<IActionResult> ApplyDiscount(DiscountApplyInput discountApplyInput)
    {
        if (!ModelState.IsValid)
        {
            TempData["discountError"] = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).First();
            return RedirectToAction(nameof(Index));
        }

        var discountStatus = await _basketService.ApplyDiscount(discountApplyInput.Code);
        TempData["discountStatus"] = discountStatus;

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> CancelApplyDiscount()
    {
        await _basketService.CancelApplyDiscount();
        return RedirectToAction(nameof(Index));
    }
}
