using Client.MVC.Models.Payments;
using Client.MVC.Services.Interfaces;

namespace Client.MVC.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient _httpClient;

    public PaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> RecievePayment(PaymentInfoInput paymentInfoInput)
    {
        var response = await _httpClient.PostAsJsonAsync<PaymentInfoInput>("payments", paymentInfoInput);
        return response.IsSuccessStatusCode;
    }
}
