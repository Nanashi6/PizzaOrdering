using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.LogicLayer.Interfaces;

namespace PizzaOrdering.Controllers
{
    [Authorize(Roles="Admin")]
    public class UsersController : Controller
    {
        private ICRUDService<User> _userService;

        public UsersController(ICRUDService<User> userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();

            User user = _userService.Read(id);
        
            if (user is null) return NotFound();
        
            return View(user);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            return View(_userService.ReadAll());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User? user)
        {
            if (user is null) return BadRequest();

            _userService.Create(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            User user = _userService.Read(id);
            
            return View(user);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(User? user)
        {
            if (user is null) return BadRequest();
        
            _userService.Update(user);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
        
            _userService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
