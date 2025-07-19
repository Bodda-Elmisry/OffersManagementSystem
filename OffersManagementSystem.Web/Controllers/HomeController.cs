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

        /// <summary>
        /// If user is authenticated redirect to Offers page, otherwise redirect to Login page
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Offers", "Offer", new OffersFilterDTO());
            }
            return RedirectToAction("Login", "Account");
        }

    }
}
