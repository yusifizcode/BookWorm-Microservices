using FreeCourse.Web.Exceptions;
using FreeCourse.Web.Models;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FreeCourse.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICatalogService _catalogService;
        public HomeController(ILogger<HomeController> logger,
                              ICatalogService catalogService)
        {
            _logger = logger;
            _catalogService = catalogService;
        }

        public IActionResult Index()
        {
            ViewBag.Featured = _catalogService.GetAllCourseAsync().Result.Skip(2).Take(5).ToList();
            ViewBag.Biographies = _catalogService.GetAllCourseAsync().Result.Skip(3).Take(3).ToList();

            return View(_catalogService.GetAllCourseAsync().Result.Take(5).ToList());
        }

        public async Task<IActionResult> Detail(string id)
            => View(await _catalogService.GetByCourseId(id));

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var errorFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();

            if (errorFeature != null && errorFeature.Error is UnAuthorizeException)
                return RedirectToAction("Logout", "Auth");

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}