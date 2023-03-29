using FreeCourse.Web.Models.Discounts;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class DiscountService : IDiscountService
{
    public Task<DiscountViewModel> GetDiscount(string discountCode)
    {
        throw new NotImplementedException();
    }
}
