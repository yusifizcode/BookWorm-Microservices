using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Services;
using FreeCourse.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : CustomBaseController
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
        => _courseService = courseService;


    public async Task<IActionResult> GetAll()
    {
        var response = await _courseService.GetAllAsync();

        return CreateActionResultInstance(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _courseService.GetByIdAsync(id);

        return CreateActionResultInstance(response);
    }

    [Route("/api/[controller]/GetAllByUserId/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var response = await _courseService.GetAllByUserIdAsync(userId);

        return CreateActionResultInstance(response);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateDto createDto)
    {
        var response = await _courseService.CreateAsync(createDto);

        return CreateActionResultInstance(response);
    }

    [HttpPut]
    public async Task<IActionResult> Update(CourseUpdateDto updateDto)
    {
        var response = await _courseService.UpdateAsync(updateDto);

        return CreateActionResultInstance(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _courseService.DeleteAsync(id);

        return CreateActionResultInstance(response);
    }
}
