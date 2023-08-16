using Client.MVC.Models.Catalog;
using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedLibrary.Services;

namespace Client.MVC.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public ProductsController(ICatalogService catalogService,
                             ISharedIdentityService sharedIdentityService)
    {
        _catalogService = catalogService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        ViewBag.Page = page;
        var productVMs = await _catalogService.GetAllProductByUserIdAsync(_sharedIdentityService.GetUserId);

        ViewBag.TotalPages = (int)Math.Ceiling(productVMs.Count() / 5d);

        if (productVMs.Count > 0)
        {
            if (page < 1 || page > (int)Math.Ceiling(productVMs.Count() / 5d))
                return NotFound();
        }

        return View(productVMs.Skip((page - 1) * 5).Take(5).ToList());
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateInput productCreateInput)
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        if (!ModelState.IsValid) return View();

        productCreateInput.UserId = _sharedIdentityService.GetUserId;
        await _catalogService.CreateProductAsync(productCreateInput);

        return RedirectToAction("Index", "Products");
    }

    public async Task<IActionResult> Update(string id)
    {
        var product = await _catalogService.GetByProductIdAsync(id);
        var categories = await _catalogService.GetAllCategoryAsync();

        if (product == null) return RedirectToAction(nameof(Index));

        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.Id);
        ProductUpdateInput productUpdateInput = new()
        {
            Id = product.Id,
            Name = product.Name,
            Count = product.Count,
            Price = product.Price,
            UserId = product.UserId,
            Picture = product.Picture,
            CategoryId = product.CategoryId,
            Description = product.Description
        };

        return View(productUpdateInput);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProductUpdateInput productUpdateInput)
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", productUpdateInput.Id);

        if (!ModelState.IsValid)
            return View();

        await _catalogService.UpdateProductAsync(productUpdateInput);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        await _catalogService.DeleteProductAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
