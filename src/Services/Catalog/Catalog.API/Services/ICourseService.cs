using Catalog.API.DTOs;
using SharedLibrary.DTOs;

namespace Catalog.API.Services;

public interface ICourseService
{
    Task<Response<List<CourseDto>>> GetAllAsync();
    Task<Response<CourseDto>> GetByIdAsync(string id);
    Task<Response<List<CourseDto>>> GetAllByUserIdAsync(string userId);
    Task<Response<CourseDto>> CreateAsync(CourseCreateDto createDto);
    Task<Response<NoContent>> UpdateAsync(CourseUpdateDto updateDto);
    Task<Response<NoContent>> DeleteAsync(string id);
}
