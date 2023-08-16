using Client.MVC.Exceptions;
using Client.MVC.Models;
using Client.MVC.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.MVC.Controllers
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
            ViewBag.Featured = _catalogService.GetAllProductAsync().Result.Skip(2).Take(5).ToList();
            ViewBag.Biographies = _catalogService.GetAllProductAsync().Result.Skip(3).Take(3).ToList();

            return View(_catalogService.GetAllProductAsync().Result.Take(5).ToList());
        }

        public async Task<IActionResult> Detail(string id)
            => View(await _catalogService.GetByProductIdAsync(id));

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