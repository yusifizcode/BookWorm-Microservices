using FreeCourse.Services.Payment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

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

        return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
    }

}
