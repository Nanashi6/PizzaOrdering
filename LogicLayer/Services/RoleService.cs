using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class RoleService : IRoleService<IdentityRole>
{
    private readonly RoleManager<IdentityRole> _roleManager;

    public RoleService(RoleManager<IdentityRole> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<IdentityRole>> ReadAll()
    {
        return _roleManager.Roles;
    }

    public async Task<IActionResult> Create(RoleViewModel role)
    {
        IdentityRole newRole = new IdentityRole(role.Name);
        await _roleManager.CreateAsync(newRole);
        return new OkResult();
    }

    public async Task<IActionResult> Update(RoleViewModel role)
    {
        IdentityRole newRole = new IdentityRole(role.Name);
        await _roleManager.UpdateAsync(newRole);
        return new OkResult();
    }

    public async Task<IActionResult> Delete(RoleViewModel role)
    {
        IdentityRole newRole = new IdentityRole(role.Name);
        await _roleManager.DeleteAsync(newRole);
        return new OkResult();
    }
}