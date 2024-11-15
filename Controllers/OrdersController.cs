using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.Infrastracture;
using PizzaOrdering.LogicLayer;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.LogicLayer.Services;
using PizzaOrdering.Models;

namespace PizzaOrdering.Controllers
{
    [Route("[controller]")]
    public class OrdersController : Controller
    { 
        private ICRUDService<Order> _orderService;
        private readonly UserManager<User> _userManager;
        private readonly ICRUDService<PizzaRequired> _pizzaRequiredService;
        private readonly IngredientService _ingredientService;

        public OrdersController(ICRUDService<Order> orderService, 
            UserManager<User> userManager, 
            ICRUDService<PizzaRequired> pizzaRequiredService, 
            ICRUDService<Ingredient> ingredientService)
        {
            _orderService = orderService;
            _userManager = userManager;
            _pizzaRequiredService = pizzaRequiredService;
            _ingredientService = (IngredientService)ingredientService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();

            Order order = _orderService.Read(id);
            
            if (order is null) return NotFound();
            
            return View(order);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            return View(_orderService.ReadAll());
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> UserOrders()
        {
            User user = await _userManager.GetUserAsync(User);
            if (user is null) return BadRequest();

            IEnumerable<Order>? orders = user.Orders;
            return View(orders);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateOrderViewModel? order)
        {
            if (ModelState.IsValid)
            {
                if (order is null) return BadRequest();
            
                Cart? cart = await GetUserCart();
                if (cart is null) return BadRequest("Cart is null");
                if (!HasIngredients(cart))
                {
                    ModelState.AddModelError("", "Pizzeria don't have ingredients");
                    return View();
                }      
            
                Order newOrder = new()
                {
                    PizzasCount = cart.pizzas.Sum(p => p.Quantity),
                    UserId = _userManager.GetUserId(User),
                    Address = order.Address,
                    DeliveryDate = order.DeliveryDate,
                    OrderDate = DateTime.Now,
                    Price = cart.pizzas.Sum(p => p.Pizza.Price)
                };
                _orderService.Create(newOrder);
            
                await CreatePizzasRequired(cart.pizzas, newOrder.Id);
            
                cart.Clear();
                SaveCart("cart", cart);
            
                return RedirectToAction(nameof(Index));   
            }
            return View(order);
        }

        [NonAction]
        private async Task CreatePizzasRequired(IEnumerable<CartItem> cartItems, int orderId)
        {
            foreach (var item in cartItems)
            {
                PizzaInfoViewModel pizza = item.Pizza;
                for (int i = 0; i < item.Quantity; i++)
                {
                    _pizzaRequiredService.Create(new PizzaRequired() {OrderId = orderId, PizzaId = pizza.Id});
                    await _ingredientService.DecriseIngredients(pizza.Ingredients.Select(e => e.Id));   
                }
            }
        }

        [NonAction]
        private bool HasIngredients(Cart? cart)
        {
            foreach (var cartItem in cart.pizzas)
            {
                PizzaInfoViewModel pizza = cartItem.Pizza;
                int quantity = cartItem.Quantity;
            
                foreach (var reqIng in pizza.Ingredients)
                {
                    if (reqIng.Quantity < quantity) return false;
                }
            }
            
            return true;
        }

        [HttpGet("[action]")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            CreateOrderViewModel order = (CreateOrderViewModel)_orderService.Read(id);
            
            return View(order);
        }
        [HttpPost("[action]")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Update(CreateOrderViewModel? orderDto)
        {
            if (orderDto is null) return BadRequest();
            
            Order order = _orderService.Read(orderDto.Id);
            if (order is null) return NotFound("Order not found");
            
            order.Update(orderDto);
            _orderService.Update(order);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("[action]")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> GetStatistic() {
          var orders = _orderService.ReadAll();
          ViewBag.Orders = orders;

          return View(orders);
        }

        [HttpGet("[action]")]
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            
            _orderService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        
        [NonAction]
        private async Task<Cart?> GetUserCart()
        {
            Cart? cart = HttpContext.Session.GetCart("cart");
            return cart;
        }

        [NonAction]
        private async Task SaveCart(string key, Cart cart)
        {
            HttpContext.Session.SetCart(key, cart);
        }
    }
}
