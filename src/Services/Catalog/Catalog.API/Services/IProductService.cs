using Catalog.API.DTOs;
using SharedLibrary.DTOs;

namespace Catalog.API.Services;

public interface IProductService
{
    Task<Response<List<ProductDto>>> GetAllAsync();
    Task<Response<ProductDto>> GetByIdAsync(string id);
    Task<Response<List<ProductDto>>> GetAllByUserIdAsync(string userId);
    Task<Response<ProductDto>> CreateAsync(ProductCreateDto createDto);
    Task<Response<NoContent>> UpdateAsync(ProductUpdateDto updateDto);
    Task<Response<NoContent>> DeleteAsync(string id);
}
