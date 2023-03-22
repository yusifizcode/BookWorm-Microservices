using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Discount.Services;

public interface IDiscountService
{
    Task<Response<List<Entities.Discount>>> GetAllAsync();
    Task<Response<Entities.Discount>> GetByIdAsync(int id);
    Task<Response<NoContent>> Save(Entities.Discount discount);
    Task<Response<NoContent>> Update(Entities.Discount discount);
    Task<Response<NoContent>> Delete(int id);

    Task<Response<Entities.Discount>> GetByCodeAndUserId(string code, string userId);
}
