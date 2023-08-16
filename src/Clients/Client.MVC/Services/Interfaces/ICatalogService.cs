using Client.MVC.Models.Catalog;

namespace Client.MVC.Services.Interfaces;

public interface ICatalogService
{
    Task<List<ProductViewModel>> GetAllProductAsync();
    Task<List<CategoryViewModel>> GetAllCategoryAsync();
    Task<ProductViewModel> GetByProductIdAsync(string productId);
    Task<List<ProductViewModel>> GetAllProductByUserIdAsync(string userId);

    Task<bool> DeleteProductAsync(string courseId);
    Task<bool> CreateProductAsync(ProductCreateInput courseCreateInput);
    Task<bool> UpdateProductAsync(ProductUpdateInput courseUpdateInput);
}
