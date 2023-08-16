using Client.MVC.Models.Catalog;
using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SharedLibrary.Services;

namespace Client.MVC.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ISharedIdentityService _sharedIdentityService;

    public CoursesController(ICatalogService catalogService,
                             ISharedIdentityService sharedIdentityService)
    {
        _catalogService = catalogService;
        _sharedIdentityService = sharedIdentityService;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        ViewBag.Page = page;
        var courseVMs = await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId);

        ViewBag.TotalPages = (int)Math.Ceiling(courseVMs.Count() / 5d);

        if (courseVMs.Count > 0)
        {
            if (page < 1 || page > (int)Math.Ceiling(courseVMs.Count() / 5d))
                return NotFound();
        }

        return View(courseVMs.Skip((page - 1) * 5).Take(5).ToList());
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        if (!ModelState.IsValid) return View();

        courseCreateInput.UserId = _sharedIdentityService.GetUserId;
        await _catalogService.CreateCourseAsync(courseCreateInput);

        return RedirectToAction("Index", "Courses");
    }

    public async Task<IActionResult> Update(string id)
    {
        var course = await _catalogService.GetByCourseId(id);
        var categories = await _catalogService.GetAllCategoryAsync();

        if (course == null) return RedirectToAction(nameof(Index));

        ViewBag.Categories = new SelectList(categories, "Id", "Name", course.Id);
        CourseUpdateInput courseUpdateInput = new()
        {
            Id = course.Id,
            Name = course.Name,
            Price = course.Price,
            UserId = course.UserId,
            Feature = course.Feature,
            Picture = course.Picture,
            CategoryId = course.CategoryId,
            Description = course.Description
        };

        return View(courseUpdateInput);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);

        if (!ModelState.IsValid)
            return View();

        await _catalogService.UpdateCourseAsync(courseUpdateInput);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        await _catalogService.DeleteCourseAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
