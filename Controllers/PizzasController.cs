using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.LogicLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.Controllers
{
    [Route("[controller]")]
    public class PizzasController : Controller
    {
        private ICRUDService<Pizza> _pizzaService;
        private readonly ICRUDService<RequiredIngredient> _requiredIngredientService;
        private readonly ICRUDService<Ingredient> _crudIngredient;
        
        public PizzasController(
            ICRUDService<Pizza> pizzaService, 
            ICRUDService<RequiredIngredient> requiredIngredientService,
            ICRUDService<Ingredient> crudIngredient)
        {
            _pizzaService = pizzaService;
            _requiredIngredientService = requiredIngredientService;
            _crudIngredient = crudIngredient;
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();

            Pizza pizza = _pizzaService.Read(id);
            
            if (pizza is null) return NotFound();
            
            PizzaInfoViewModel resultPizzaDto = new PizzaInfoViewModel()
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Size = pizza.Size,
                Price = pizza.Price,
                Ingredients = pizza.RequiredIngredients.Select(e => e.Ingredient)
            };
            
            return View(resultPizzaDto);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Pizza> pizzas = _pizzaService.ReadAll();
            
            IEnumerable<PizzaInfoViewModel> pizzaInfoDtos = pizzas.Select(pizza => new PizzaInfoViewModel()
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Size = pizza.Size,
                Price = pizza.Price,
                Ingredients = pizza.RequiredIngredients.Select(ri => ri.Ingredient)
            });
            
            return View(pizzaInfoDtos);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Filter(double? maxPrice = null, double minPrice = 0, int? size = null/*, IEnumerable<int> ingredientIds = null*/)
        {
            IEnumerable<Pizza> pizzas = _pizzaService.ReadAll().Where(p => p.Price >= minPrice);
            if (maxPrice is not null) pizzas = pizzas.Where(p => p.Price <= maxPrice);
            if (size is not null) pizzas = pizzas.Where(p => p.Size == size);
            // if (ingredientIds is not null)
            //     pizzas = pizzas.Where(pizza => pizza.RequiredIngredients.All(ri => ingredientIds.Contains(ri.Ingredient.Id)));
            
            IEnumerable<PizzaInfoViewModel> pizzaInfoDtos = pizzas.Select(pizza => new PizzaInfoViewModel()
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Size = pizza.Size,
                Price = pizza.Price,
                Ingredients = pizza.RequiredIngredients.Select(ri => ri.Ingredient)
            });
            
            return View(pizzaInfoDtos);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create()
        {
            var ingredients = _crudIngredient.ReadAll();
            ViewBag.Ingredients = ingredients;
            
            return View();
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreatePizzaViewModel? pizza)
        {
            if (pizza is null) return BadRequest();

            Pizza newPizza = new Pizza()
            {
                Name = pizza.Name,
                Price = pizza.Price,
                Size = pizza.Size,
            };
            _pizzaService.Create(newPizza);

            IEnumerable<RequiredIngredient> requiredIngredients = pizza.
                IngredientIds.Select(ingredientId => new RequiredIngredient
                {
                    PizzaId = newPizza.Id,
                    IngredientId = ingredientId
                });

            CreateRequiredIngredientsAsync(requiredIngredients);
            
            return RedirectToAction(nameof(Index));
        }
        
        [NonAction]
        private async Task CreateRequiredIngredientsAsync(IEnumerable<RequiredIngredient> requiredIngredients)
        {
            foreach (var requiredIngredient in requiredIngredients)
            {
                _requiredIngredientService.Create(requiredIngredient);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            CreatePizzaViewModel pizza = (CreatePizzaViewModel)_pizzaService.Read(id);

            var ingredients = _crudIngredient.ReadAll();
            ViewBag.Ingredients = ingredients;
            
            return View(pizza);
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Update(CreatePizzaViewModel? pizzaDto)
        {
            if (pizzaDto is null) return BadRequest();
            Pizza pizza = _pizzaService.Read(pizzaDto.Id);
            if (pizza is null) return NotFound("Pizza not found");
            
            IEnumerable<int> exceptsIngredientsIds = 
                pizza.RequiredIngredients.Where(ri => !pizzaDto.IngredientIds.Contains(ri.IngredientId)).Select(ri => ri.Id);
            IEnumerable<int> newIngredientsIds =
                pizzaDto.IngredientIds.Except(pizza.RequiredIngredients.Select(ri => ri.Ingredient.Id));
            
            foreach (int existingIngredientId in exceptsIngredientsIds)
                _requiredIngredientService.Delete(existingIngredientId);

            foreach (int newIngredientId in newIngredientsIds)
                _requiredIngredientService.Create(new RequiredIngredient() {IngredientId = newIngredientId, PizzaId = pizza.Id});
            
            pizza.Update(pizzaDto);
            _pizzaService.Update(pizza);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            
            _pizzaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
