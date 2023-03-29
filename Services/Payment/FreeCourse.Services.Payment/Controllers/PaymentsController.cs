using FreeCourse.Services.Payment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Payment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : CustomBaseController
{
    [HttpPost]
    public IActionResult RecievePayment(PaymentDto paymentDto)
        => CreateActionResultInstance(Response<NoContent>.Success(200));
}
