using FreeCourse.Web.Models.Payments;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services;

public class PaymentService : IPaymentService
{
    public Task<bool> RecievePayment(PaymentInfoInput paymentInfoInput)
    {
        throw new NotImplementedException();
    }
}
