using FreeCourse.Web.Models.Orders;

namespace FreeCourse.Web.Services.Interfaces;

public interface IOrderService
{
    /// <summary>
    /// Sync connection - Will be made directly request to the Order microservice
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns>
    Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);
    /// <summary>
    /// Async connection - Order informations send to RabbitMQ
    /// </summary>
    /// <param name="checkoutInfoInput"></param>
    /// <returns></returns>
    Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);
    Task<List<OrderViewModel>> GetOrder();
}
