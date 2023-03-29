using FreeCourse.Web.Models.Baskets;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers;
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

    public async Task<IActionResult> AddBasketItem(string courseId)
    {
        var course = await _catalogService.GetByCourseId(courseId);

        var basketItemVM = new BasketItemViewModel
        {
            CourseId = course.Id,
            CourseName = course.Name,
            Price = course.Price,
        };

        await _basketService.AddBasketItem(basketItemVM);

        return RedirectToAction("Index", "Basket");
    }

    public async Task<IActionResult> RemoveBasketItem(string courseId)
    {
        await _basketService.RemoveBasketItem(courseId);
        return RedirectToAction("Index", "Basket");
    }
}
