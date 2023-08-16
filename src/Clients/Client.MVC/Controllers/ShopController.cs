using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Client.MVC.Controllers;

public class ShopController : Controller
{
    private readonly ICatalogService _catalogService;

    public ShopController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index(string title, int page = 1)
    {
        ViewBag.Page = page;
        var productVMs = await _catalogService.GetAllProductAsync();

        ViewBag.TotalPages = (int)Math.Ceiling(productVMs.Count() / 10d);

        if (productVMs.Count > 0)
        {
            if (page < 1 || page > (int)Math.Ceiling(productVMs.Count() / 10d))
                return NotFound();
        }

        if (title != null)
            productVMs = productVMs.Where(x => x.Name.ToLower().Contains(title.ToLower())).ToList();

        return View(productVMs.Skip((page - 1) * 10).Take(10).ToList());
    }
}
