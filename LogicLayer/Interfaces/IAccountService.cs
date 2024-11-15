using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.Models;

namespace PizzaOrdering.LogicLayer.Interfaces;

public interface IAccountService<TUser> where TUser : User
{
    Task<IActionResult> Login(LoginViewModel model);
    Task<IActionResult> Register(RegisterViewModel model);
    Task<IActionResult> Logout();
    Task<User?> GetUserInfo();
}
