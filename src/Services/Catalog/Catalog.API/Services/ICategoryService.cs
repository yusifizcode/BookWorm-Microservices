using Catalog.API.DTOs;
using SharedLibrary.DTOs;

namespace Catalog.API.Services;

public interface ICategoryService
{
    Task<Response<List<CategoryDto>>> GetAllAsync();
    Task<Response<CategoryDto>> GetByIdAsync(string id);
    Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);
}
