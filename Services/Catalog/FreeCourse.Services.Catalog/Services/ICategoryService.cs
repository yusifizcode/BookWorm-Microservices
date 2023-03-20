using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Catalog.Services;

public interface ICategoryService
{
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> GetByIdAsync(string id);
    Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
}
