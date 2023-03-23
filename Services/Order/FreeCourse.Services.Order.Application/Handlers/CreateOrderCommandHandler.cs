﻿using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;

namespace FreeCourse.Services.Order.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
{
    private readonly OrderDbContext _context;
    public CreateOrderCommandHandler(OrderDbContext context)
        => _context = context;

    public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var newAddress = new Address(request.Address.Province,
                                     request.Address.District,
                                     request.Address.Street,
                                     request.Address.ZipCode,
                                     request.Address.Line);

        Domain.OrderAggregate.Order newOrder = new(request.BuyerId, newAddress);
        request.OrderItems.ForEach(x =>
        {
            newOrder.AddOrderItem(x.ProductId, x.ProductName, x.Price, x.PictureUrl);
        });

        await _context.Orders.AddAsync(newOrder);
        await _context.SaveChangesAsync();

        return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
    }
}
