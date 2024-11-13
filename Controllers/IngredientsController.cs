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

namespace PizzaOrdering.Controllers
{
    public class IngredientsController : Controller
    {
        private ICRUDService<Ingredient> _ingredientService;

        public IngredientsController(ICRUDService<Ingredient> ingredientService)
        {
            _ingredientService = ingredientService;
        }

        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();

            Ingredient ingredient = _ingredientService.Read(id);
        
            if (ingredient is null) return NotFound();
        
            return View(ingredient);
        }

        public async Task<IActionResult> Index()
        {
            return View(_ingredientService.ReadAll());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(Ingredient? ingredient)
        {
            if (ingredient is null) return BadRequest();

            _ingredientService.Create(ingredient);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
            Ingredient ingredient = _ingredientService.Read(id);
            
            return View(ingredient);
        }
        [HttpPut]
        public async Task<IActionResult> Update(Ingredient? ingredient)
        {
            if (ingredient is null) return BadRequest();
        
            _ingredientService.Update(ingredient);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
        
            _ingredientService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
