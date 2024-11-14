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

namespace PizzaOrdering.Controllers
{
    public class RequiredIngredientsController : Controller
    {
        private ICRUDService<RequiredIngredient> _requiredIngredientService;
        private ICRUDService<Ingredient> _ingredientService;
        private ICRUDService<Pizza> _pizzasService;

        public RequiredIngredientsController(ICRUDService<RequiredIngredient> requiredIngredientService, ICRUDService<Ingredient> ingredientService, ICRUDService<Pizza> pizzasService)
        {
            _requiredIngredientService = requiredIngredientService;
            _ingredientService = ingredientService;
            _pizzasService = pizzasService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();

            RequiredIngredient requiredIngredient = _requiredIngredientService.Read(id);
        
            if (requiredIngredient is null) return NotFound();
        
            return View(requiredIngredient);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(_requiredIngredientService.ReadAll());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Ingredients = _ingredientService.ReadAll();
            ViewBag.Pizzas = _pizzasService.ReadAll();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RequiredIngredient? requiredIngredient)
        {
            if (requiredIngredient is null) return BadRequest();

            _requiredIngredientService.Create(requiredIngredient);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            RequiredIngredient requiredIngredient = _requiredIngredientService.Read(id);
            
            ViewBag.Ingredients = _ingredientService.ReadAll();
            ViewBag.Pizzas = _pizzasService.ReadAll();
            
            return View(requiredIngredient);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RequiredIngredient? requiredIngredient)
        {
            if (requiredIngredient is null) return BadRequest();
        
            _requiredIngredientService.Update(requiredIngredient);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
        
            _requiredIngredientService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
