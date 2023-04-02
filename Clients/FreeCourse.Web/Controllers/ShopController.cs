using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers;

public class ShopController : Controller
{
    private readonly ICatalogService _catalogService;

    public ShopController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        ViewBag.Page = page;
        var courseVMs = await _catalogService.GetAllCourseAsync();

        ViewBag.TotalPages = (int)Math.Ceiling(courseVMs.Count() / 10d);

        if (courseVMs.Count > 0)
        {
            if (page < 1 || page > (int)Math.Ceiling(courseVMs.Count() / 10d))
                return NotFound();
        }


        return View(courseVMs.Skip((page - 1) * 10).Take(10).ToList());
    }
}
