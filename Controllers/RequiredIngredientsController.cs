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

        public RequiredIngredientsController(ICRUDService<RequiredIngredient> requiredIngredientService)
        {
            _requiredIngredientService = requiredIngredientService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if (id < 1 || id is null) return BadRequest();

            RequiredIngredient requiredIngredient = _requiredIngredientService.Read(id);
        
            if (requiredIngredient is null) return NotFound();
        
            return Ok(requiredIngredient);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(_requiredIngredientService.ReadAll());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
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
            
            return View(requiredIngredient);
        }
        [HttpPut("[action]")]
        public async Task<IActionResult> Update(RequiredIngredient? requiredIngredient)
        {
            if (requiredIngredient is null) return BadRequest();
        
            _requiredIngredientService.Update(requiredIngredient);
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id < 1 || id is null) return BadRequest();
        
            _requiredIngredientService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
