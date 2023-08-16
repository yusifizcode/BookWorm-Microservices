using Client.MVC.Helpers;
using Client.MVC.Models.Catalog;
using Client.MVC.Services.Interfaces;
using SharedLibrary.DTOs;

namespace Client.MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient _client;
    private readonly IPhotoStockService _photoStockService;
    private readonly PhotoHelper _photoHelper;
    public CatalogService(HttpClient client,
                          PhotoHelper photoHelper,
                          IPhotoStockService photoStockService)
    {
        _client = client;
        _photoHelper = photoHelper;
        _photoStockService = photoStockService;
    }

    public async Task<bool> CreateProductAsync(ProductCreateInput courseCreateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);

        if (resultPhotoService != null)
            courseCreateInput.Picture = resultPhotoService.Url;

        var response = await _client.PostAsJsonAsync<ProductCreateInput>("Products", courseCreateInput);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteProductAsync(string productId)
    {
        var response = await _client.DeleteAsync($"Products/{productId}");

        return response.IsSuccessStatusCode;
    }

    public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
    {
        // http://localhost:5000/services/catalog/categories
        var response = await _client.GetAsync("Categories");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
        return responseSuccess.Data;
    }

    public async Task<List<ProductViewModel>> GetAllProductAsync()
    {
        // http://localhost:5000/services/catalog/products
        var response = await _client.GetAsync("Products");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();

        responseSuccess.Data.ForEach(x =>
        {
            x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
        });

        return responseSuccess.Data;
    }

    public async Task<List<ProductViewModel>> GetAllProductByUserIdAsync(string productId)
    {
        // http://localhost:5000/services/catalog/products/GetAllByUserId/{productId}
        var response = await _client.GetAsync($"Products/GetAllByUserId/{productId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<ProductViewModel>>>();

        responseSuccess.Data.ForEach(x =>
        {
            x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
        });

        return responseSuccess.Data;
    }

    public async Task<ProductViewModel> GetByProductIdAsync(string productId)
    {
        // http://localhost:5000/services/catalog/Products/{productId}
        var response = await _client.GetAsync($"Products/{productId}");

        if (!response.IsSuccessStatusCode)
            return null;

        var responseSuccess = await response.Content.ReadFromJsonAsync<Response<ProductViewModel>>();

        responseSuccess.Data.StockPictureUrl = _photoHelper.GetPhotoStockUrl(responseSuccess.Data.Picture);

        return responseSuccess.Data;
    }

    public async Task<bool> UpdateProductAsync(ProductUpdateInput productUpdateInput)
    {
        var resultPhotoService = await _photoStockService.UploadPhoto(productUpdateInput.PhotoFormFile);

        if (resultPhotoService != null)
        {
            await _photoStockService.DeletePhoto(productUpdateInput.Picture);
            productUpdateInput.Picture = resultPhotoService.Url;
        }

        var response = await _client.PutAsJsonAsync<ProductUpdateInput>("Products", productUpdateInput);

        return response.IsSuccessStatusCode;
    }
}