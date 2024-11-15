using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.Controllers
{
    public class AccountController : Controller
    {
        private readonly PizzeriaContext _context;
        private readonly UserManager<User?> _userManager;
        private readonly SignInManager<User?> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(PizzeriaContext context, 
                                UserManager<User?> userManager, 
                                SignInManager<User?> signInManager,
                                IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult AccessDenied(string returnUrl) 
          => RedirectToAction("Index", "Home");

        public async Task<IActionResult> Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            User? user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "Home");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = new User()
                {
                    Email = model.Email,
                    Name = model.Name,
                    Surname = model.Surname,
                    UserName = model.Email
                };
                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                _context.SaveChanges();
        
                if (result.Succeeded)
                {
                    user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        await _userManager.AddToRoleAsync(user,  "User");
                        await _signInManager.SignInAsync(user, false);
                        return RedirectToAction(nameof(Index), "Home");   // TODO: ТЕПАТЬ ЧЕЛА НА ПРЕДЫДУЩУЮ СТРАНИЦУ
                    }
                }   
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }
        
        public async Task<IActionResult> GetUserInfo()
        {
            return View(_userManager.GetUserAsync(_httpContextAccessor.HttpContext.User));
        }
    }
}
