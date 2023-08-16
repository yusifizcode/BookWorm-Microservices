using Microsoft.AspNetCore.Mvc;
using SharedLibrary.DTOs;

namespace SharedLibrary.ControllerBases
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
