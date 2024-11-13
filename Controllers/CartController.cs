using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.Infrastracture;
using PizzaOrdering.LogicLayer;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.Controllers
{
    public class CartController : Controller // TODO: СДЕЛАТЬ НОРМАЛЬНУЮ АДРЕСАЦИЮ
    {
        private readonly ICRUDService<Pizza> _pizzasService;
        private readonly UserManager<User> _userManager;

        public CartController(ICRUDService<Pizza> pizzasService, UserManager<User> userManager)
        {
            _pizzasService = pizzasService;
            _userManager = userManager;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCart()
        {
            return Ok(await GetUserCart());
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult> AddPizzaToCart(int pizzaId, int quantity)
        {
            if (pizzaId < 1 || quantity < 1) return BadRequest();
            Pizza? pizza = _pizzasService.Read(pizzaId);
            
            if (pizza is null) return NotFound("Pizza not found"); 
            Cart cart = await GetUserCart();
            
            PizzaInfoViewModel pizzaInfo = new PizzaInfoViewModel()
            {
                Id = pizzaId,
                Name = pizza.Name,
                Price = pizza.Price,
                Size = pizza.Size,
                Ingredients = pizza.RequiredIngredients.Select(e => e.Ingredient)
            };
            
            cart.AddItem(pizzaInfo, quantity);
            SaveCart("cart", cart);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> UpdatePizzaQuantityOnCart(int pizzaId, int newQuantity)
        {
            if (pizzaId < 1 || newQuantity < 1) return BadRequest();
            Pizza? pizza = _pizzasService.Read(pizzaId);
            
            if (pizza is null) return NotFound("Pizza not found"); 
            Cart cart = await GetUserCart();

            CartItem pizzaOnCart = cart.cartItems.FirstOrDefault(e => e.Pizza.Id == pizzaId);
            
            if(pizzaOnCart is null) return BadRequest("Pizza not found on Cart");
            
            pizzaOnCart.Quantity = newQuantity;
            SaveCart("cart", cart);
            return Ok();
        }
        
        [HttpPost("[action]")]
        public async Task<ActionResult> RemovePizzaFromCart(int pizzaId)
        {
            if (pizzaId < 1) return BadRequest();
            Pizza? pizza = _pizzasService.Read(pizzaId);
            
            if (pizza is null) return NotFound("Pizza not found");
            Cart cart = await GetUserCart();
            cart.RemoveItem(pizza);
            SaveCart("cart", cart);
            return Ok();
        }
        
        [NonAction]
        private async Task<Cart> GetUserCart()
        {
            Cart? cart = HttpContext.Session.GetCart("cart");
            if (cart == null)
            {
                cart = new Cart();
            }

            return cart;
        }

        [NonAction]
        private async Task SaveCart(string key, Cart cart)
        {
            HttpContext.Session.SetCart(key,  cart);
        }
    }
}
