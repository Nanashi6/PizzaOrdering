using Microsoft.AspNetCore.Identity;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class UserService : CRUDService<User>
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(PizzeriaContext context) : base(context) {}
}