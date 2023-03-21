using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Basket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasketsController : CustomBaseController
{
    private readonly IBasketService _basketService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public BasketsController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
    {
        _basketService = basketService;
        _sharedIdentityService = sharedIdentityService;
    }


    [HttpGet]
    public async Task<IActionResult> GetBasket()
        => CreateActionResultInstance(await _basketService.GetBasket(_sharedIdentityService.GetUserId));

    [HttpPost]
    public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        => CreateActionResultInstance(await _basketService.SaveOrUpdate(basketDto));

    [HttpDelete]
    public async Task<IActionResult> DeleteBasket()
        => CreateActionResultInstance(await _basketService.Delete(_sharedIdentityService.GetUserId));
}
