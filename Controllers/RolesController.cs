using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.Controllers
{
    [Authorize(Roles="Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService<IdentityRole> _roleService;

        public RolesController(IRoleService<IdentityRole> roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = _roleService.ReadAll();
            ViewBag.Roles = result;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("[action]")]
        public async Task<ActionResult> Create(RoleViewModel model)
        {
            await _roleService.Create(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (id == null) return BadRequest();
            IdentityRole role = await _roleService.Read(id);
            
            return View(role);
        }
        [HttpPut("[action]")]
        public async Task<ActionResult> Update(RoleViewModel model)
        {
            await _roleService.Update(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> Delete(RoleViewModel model)
        {
            await _roleService.Delete(model);
            return RedirectToAction(nameof(Index));
        }
    }
}
