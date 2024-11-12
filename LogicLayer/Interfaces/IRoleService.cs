using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.Models;

namespace PizzaOrdering.LogicLayer.Interfaces;

public interface IRoleService<TRole> where TRole : IdentityRole
{
    Task<IEnumerable<TRole>> ReadAll();
    Task<IActionResult> Create(RoleViewModel role);
    Task<IActionResult> Update(RoleViewModel role);
    Task<IActionResult> Delete(RoleViewModel role);
}