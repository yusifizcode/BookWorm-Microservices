using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FreeCourse.Web.Controllers;

[Authorize]
public class CoursesController : Controller
{
    private readonly ICatalogService _catalogService;
    private readonly ISharedIdentityService _sharedIdentityService;
    private readonly IPhotoStockService _photoStockService;

    public CoursesController(ICatalogService catalogService,
                             ISharedIdentityService sharedIdentityService,
                             IPhotoStockService photoStockService)
    {
        _catalogService = catalogService;
        _sharedIdentityService = sharedIdentityService;
        _photoStockService = photoStockService;
    }

    public async Task<IActionResult> Index()
        => View(await _catalogService.GetAllCourseByUserIdAsync(_sharedIdentityService.GetUserId));

    public async Task<IActionResult> Create()
    {
        var categories = await _catalogService.GetAllCategoryAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateInput courseCreateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

        if (resultPhotoService != null)
            courseCreateInput.Picture = resultPhotoService.Url;


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
