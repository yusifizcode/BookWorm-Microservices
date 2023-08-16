using Client.MVC.Models.Baskets;
using Client.MVC.Services.Interfaces;
using SharedLibrary.DTOs;

namespace Client.MVC.Services;

public class BasketService : IBasketService
{
    private readonly HttpClient _httpClient;
    private readonly IDiscountService _discountService;

    public BasketService(HttpClient httpClient,
                         IDiscountService discountService)
    {
        _httpClient = httpClient;
        _discountService = discountService;
    }

    public async Task AddBasketItem(BasketItemViewModel basketItemViewModel)
    {
        BasketViewModel basket = await Get();

        if (basket != null)
        {
            if (!basket.BasketItems.Any(x => x.ProductId == basketItemViewModel.ProductId))
                basket.BasketItems.Add(basketItemViewModel);
        }
        else
        {
            basket = new BasketViewModel();
            basket.BasketItems.Add(basketItemViewModel);
        }

        await SaveOrUpdate(basket);
    }

    public async Task<bool> ApplyDiscount(string discountCode)
    {
        await CancelApplyDiscount();

        var basket = await Get();
        if (basket == null) return false;

        var hasDiscount = await _discountService.GetDiscount(discountCode);
        if (hasDiscount == null) return false;

        basket.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);
        await SaveOrUpdate(basket);
        return true;
    }

    public async Task<bool> CancelApplyDiscount()
    {
        var basket = await Get();
        if (basket == null || basket.DiscountCode == null) return false;

        basket.CancelDiscount();
        await SaveOrUpdate(basket);
        return true;
    }

    public async Task<bool> Delete()
    {
        var result = await _httpClient.DeleteAsync("baskets");
        return result.IsSuccessStatusCode;
    }

    public async Task<BasketViewModel> Get()
    {
        var response = await _httpClient.GetAsync("baskets");

        if (!response.IsSuccessStatusCode) return null;

        var basketVM = await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
        return basketVM.Data;

    }

    public async Task<bool> RemoveBasketItem(string productId)
    {
        var basket = await Get();
        if (basket == null) return false;

        var deleteBasketItem = basket.BasketItems.FirstOrDefault(x => x.ProductId == productId);
        if (deleteBasketItem == null) return false;

        var deleteResult = basket.BasketItems.Remove(deleteBasketItem);
        if (!deleteResult) return false;

        if (!basket.BasketItems.Any()) basket.DiscountCode = null;

        return await SaveOrUpdate(basket);
    }

    public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
    {
        var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);

        return response.IsSuccessStatusCode;
    }
}
