using Client.MVC.Models.Discounts;

namespace Client.MVC.Services.Interfaces;

public interface IDiscountService
{
    Task<DiscountViewModel> GetDiscount(string discountCode);
}
