using Client.MVC.Models.Payments;

namespace Client.MVC.Services.Interfaces;

public interface IPaymentService
{
    Task<bool> RecievePayment(PaymentInfoInput paymentInfoInput);
}
