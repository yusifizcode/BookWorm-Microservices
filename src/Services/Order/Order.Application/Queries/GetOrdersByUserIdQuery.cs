using MediatR;
using Order.Application.DTOs;
using SharedLibrary.DTOs;

namespace Order.Application.Queries;

public class GetOrdersByUserIdQuery : IRequest<Response<List<OrderDto>>>
{
    public string UserId { get; set; }
}
