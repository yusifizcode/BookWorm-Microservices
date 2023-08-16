using Basket.API.DTOs;
using SharedLibrary.DTOs;

namespace Basket.API.Services;

public interface IBasketService
{
    Task<Response<BasketDto>> GetBasket(string userId);
    Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);
    Task<Response<bool>> Delete(string userId);
}
