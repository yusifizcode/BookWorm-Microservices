using FreeCourse.Web.Models.Baskets;

namespace FreeCourse.Web.Services.Interfaces;

public interface IBasketService
{
    Task<bool> Delete();
    Task<BasketViewModel> Get();
    Task<bool> CancelApplyDiscount();
    Task<bool> RemoveBasketItem(string courseId);
    Task<bool> ApplyDiscount(string discountCode);
    Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
    Task AddBasketItem(BasketItemViewModel basketItemViewModel);
}
