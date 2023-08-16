using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;
using SharedLibrary.Messages;

namespace Order.Application.Consumers;

public class ProductNameChangedEventConsumer : IConsumer<ProductNameChangedEvent>
{
    private readonly OrderDbContext _orderDbContext;

    public ProductNameChangedEventConsumer(OrderDbContext orderDbContext)
    {
        _orderDbContext = orderDbContext;
    }

    public async Task Consume(ConsumeContext<ProductNameChangedEvent> context)
    {
        var orderItems = await _orderDbContext.OrderItems.Where(x => x.ProductId == context.Message.ProductId).ToListAsync();

        orderItems.ForEach(x =>
        {
            x.UpdateOrderItem(context.Message.UpdatedName, x.PictureUrl, x.Price);
        });

        await _orderDbContext.SaveChangesAsync();
    }
}
