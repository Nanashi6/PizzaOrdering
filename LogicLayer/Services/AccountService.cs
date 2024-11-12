using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class AccountService : IAccountService<User>
{
    private readonly PizzeriaContext _context;
    private readonly UserManager<User?> _userManager;
    private readonly SignInManager<User?> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(PizzeriaContext context, UserManager<User?> userManager, SignInManager<User?> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        User? user = await _userManager.FindByEmailAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (result.Succeeded)
            {
                return new OkObjectResult("Welcome");
            }
        }
        SerializableError error = new SerializableError();
        error.Add(String.Empty, "Email or password is incorrect.");
        return new BadRequestObjectResult(error);
    }

    public async Task<IActionResult> Register(RegisterViewModel model)
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
                return new OkObjectResult("Welcome");   
            }
        }

        var error = result.Errors.Select(e => e.Description);
        return new BadRequestObjectResult(error);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return new OkResult();
    }

    public async Task<User?> GetUserInfo()
    {
        return await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
    }
}