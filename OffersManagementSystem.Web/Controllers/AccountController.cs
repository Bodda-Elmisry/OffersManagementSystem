using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using OffersManagementSystem.Application.IServices;
using OffersManagementSystem.Domain.DTOs;
using OffersManagementSystem.Infrastructure.Identity;
using OffersManagementSystem.Infrastructure.Identity.Interfaces;
using OffersManagementSystem.Web.DTOs;

namespace OffersManagementSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly SignInManager<AppIdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AccountController(
            UserManager<AppIdentityUser> userManager,
            SignInManager<AppIdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Navigate to Login view with default values
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            LoginDTO loginDTO = new LoginDTO();
            return View(loginDTO);
        }

        /// <summary>
        /// Login user with username and password, if successful redirect to Offers page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password");
                return View(model);
            }

            return RedirectToAction("Offers", "Offer", new OffersFilterDTO());
        }

        /// <summary>
        /// Navigate to Register view with default values
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            RegisterDTO registerDTO = new RegisterDTO();
            return View(registerDTO);
        }

        /// <summary>
        /// Register a new user with username and password, if successful sign in the user and redirect to Offers page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return View(model); 
            
            var existingUser = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null)
            {
                ModelState.AddModelError("UserName", "Username already exists");
                return View(model);
            }

            var user = new AppIdentityUser
            {
                UserName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }

            //If the user is an admin, set the Admin role
            if (model.IsAdmin)
            {
                await SetAdminRole(user);
            }

            // Sign in the user after registration
            await _signInManager.SignInAsync(user, isPersistent: false);

            return RedirectToAction("Offers", "Offer", new OffersFilterDTO());
        }

        /// <summary>
        /// Set the Admin role for the user if it doesn't exist, and add the user to the Admin role
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task SetAdminRole(AppIdentityUser user)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }

        /// <summary>
        /// Logout the user and redirect to Login page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
