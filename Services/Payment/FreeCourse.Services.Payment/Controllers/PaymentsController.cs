using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.Payment.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : CustomBaseController
{
    [HttpPost]
    public IActionResult RecievePayment()
        => CreateActionResultInstance(Response<NoContent>.Success(200));
}
