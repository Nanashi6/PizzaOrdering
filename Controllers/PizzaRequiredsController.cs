using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.LogicLayer.Services;

namespace PizzaOrdering.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles="Admin")]
    public class PizzaRequiredsController : Controller
    {
        private readonly ICRUDService<PizzaRequired> _pizzaRequiredService;
        private readonly OrderService _orderService;
        private readonly ICRUDService<Pizza> _pizzaService;

        public PizzaRequiredsController(ICRUDService<PizzaRequired> pizzaRequiredService,
            ICRUDService<Order> orderService, ICRUDService<Pizza> pizzaService)
        {
            _pizzaRequiredService = pizzaRequiredService;
            _orderService = (OrderService)orderService;
            _pizzaService = pizzaService;
        }
    
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            PizzaRequired pizzaRequired = _pizzaRequiredService.Read(id);
        
            if (pizzaRequired is null) return NotFound();
        
            return View(pizzaRequired);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            return View(_pizzaRequiredService.ReadAll());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            var pizzas = _pizzaService.ReadAll();
            var orders = _orderService.ReadAll();

            ViewBag.Pizzas = pizzas;
            ViewBag.Orders = orders;

            return View();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(PizzaRequired? pizzaRequired)
        {
            if (pizzaRequired is null) return BadRequest();

            _orderService.IncreasePizzaCount(pizzaRequired.OrderId, 1);
        
            _pizzaRequiredService.Create(pizzaRequired);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            PizzaRequired pizzaRequired = _pizzaRequiredService.Read(id);

            var pizzas = _pizzaService.ReadAll();
            var orders = _orderService.ReadAll();

            ViewBag.Pizzas = pizzas;
            ViewBag.Orders = orders;
            
            return View(pizzaRequired);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Update(PizzaRequired? pizzaRequired)
        {
            if (pizzaRequired is null) return BadRequest();
        
            _pizzaRequiredService.Update(pizzaRequired);
            return RedirectToAction(nameof(Index));
        }
    
        [HttpGet("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
        
            PizzaRequired pizzaRequired = _pizzaRequiredService.Read(id);
            if (pizzaRequired is null) return NotFound("PizzaRequired is not found");
        
            _orderService.DecriesePizzaCount(pizzaRequired.OrderId, 1);
            _pizzaRequiredService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
