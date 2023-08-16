using MassTransit;
using Order.Infrastructure;
using SharedLibrary.Messages;

namespace Order.Application.Consumers;

public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
{
    private readonly OrderDbContext _orderDbContext;

    public CreateOrderMessageCommandConsumer(OrderDbContext dbContext)
    {
        _orderDbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
    {
        var newAddress = new Domain.OrderAggregate.Address(context.Message.Province,
                                                           context.Message.District,
                                                           context.Message.Street,
                                                           context.Message.ZipCode,
                                                           context.Message.Line);

        Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(context.Message.BuyerId, newAddress);
        context.Message.OrderItems.ForEach(x =>
        {
            order.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
        });

        await _orderDbContext.Orders.AddAsync(order);
        await _orderDbContext.SaveChangesAsync();
    }
}
