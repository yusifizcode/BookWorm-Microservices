using FreeCourse.Services.Payment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace FreeCourse.Services.Payment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : CustomBaseController
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public PaymentsController(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    [HttpPost]
    public async Task<IActionResult> RecievePayment(PaymentDto paymentDto)
    {
        var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

        var createOrderMessageCommand = new CreateOrderMessageCommand();

        createOrderMessageCommand.BuyerId = paymentDto.Order.BuyerId;
        createOrderMessageCommand.Line = paymentDto.Order.Address.Line;
        createOrderMessageCommand.Street = paymentDto.Order.Address.Street;
        createOrderMessageCommand.ZipCode = paymentDto.Order.Address.ZipCode;
        createOrderMessageCommand.District = paymentDto.Order.Address.District;
        createOrderMessageCommand.Province = paymentDto.Order.Address.Province;

        paymentDto.Order.OrderItems.ForEach(x =>
        {
            createOrderMessageCommand.OrderItems.Add(new OrderItem
            {
                Price = x.Price,
                ProductId = x.ProductId,
                PictureUrl = x.PictureUrl,
                ProductName = x.ProductName
            });
        });

        await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);

        try
        {
            StripeConfiguration.ApiKey = "sk_test_51MsRiLGVwWWuLvrTdJpuEObgwZOdlfB6mCymd4Nro1JqeJMbSNphwHY85eHbaq54jGKS2i8fVBWJiJwjRoOQie6B00sTkyhmz1";

            var optionsToken = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = paymentDto.CardName,
                    Number = paymentDto.CardNumber,
                    ExpYear = paymentDto.Expiration.Trim().ToLower().Split("/")[1],
                    ExpMonth = paymentDto.Expiration.Trim().ToLower().Split("/")[0],
                    Cvc = paymentDto.CVV
                }
            };

            var serviceToken = new TokenService();
            Token stripeToken = await serviceToken.CreateAsync(optionsToken);

            var options = new ChargeCreateOptions
            {
                Amount = (long)paymentDto.TotalPrice * 100,
                Currency = "usd",
                Description = "test",
                Source = stripeToken.Id
            };

            var service = new ChargeService();
            Charge charge = await service.CreateAsync(options);

            if (charge.Paid)
            {
                return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
            }
            else
            {
                return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Fail("An error ocurred while payment!", 500));
            }
        }
        catch (System.Exception e)
        {

            return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Fail("An error ocurred while payment!", 500));
        }

        //return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
    }

}
