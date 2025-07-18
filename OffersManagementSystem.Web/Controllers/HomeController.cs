using Microsoft.AspNetCore.Mvc;
using OffersManagementSystem.Web.DTOs;
using OffersManagementSystem.Web.Models;
using System.Diagnostics;

namespace OffersManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Offers", "Offer", new OffersFilterDTO());
            }
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
