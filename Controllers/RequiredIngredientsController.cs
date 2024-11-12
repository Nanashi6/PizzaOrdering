using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.Controllers
{
    public class RequiredIngredientsController : Controller
    {
        private readonly PizzeriaContext _context;

        public RequiredIngredientsController(PizzeriaContext context)
        {
            _context = context;
        }

        // GET: RequiredIngredients
        public async Task<IActionResult> Index()
        {
            var pizzeriaContext = _context.RequiredIngredients.Include(r => r.Ingredient).Include(r => r.Pizza);
            return View(await pizzeriaContext.ToListAsync());
        }

        // GET: RequiredIngredients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requiredIngredient = await _context.RequiredIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Pizza)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requiredIngredient == null)
            {
                return NotFound();
            }

            return View(requiredIngredient);
        }

        // GET: RequiredIngredients/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id");
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id");
            return View();
        }

        // POST: RequiredIngredients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IngredientId,PizzaId")] RequiredIngredient requiredIngredient)
        {
            if (ModelState.IsValid)
            {
                _context.Add(requiredIngredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", requiredIngredient.IngredientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id", requiredIngredient.PizzaId);
            return View(requiredIngredient);
        }

        // GET: RequiredIngredients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requiredIngredient = await _context.RequiredIngredients.FindAsync(id);
            if (requiredIngredient == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", requiredIngredient.IngredientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id", requiredIngredient.PizzaId);
            return View(requiredIngredient);
        }

        // POST: RequiredIngredients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IngredientId,PizzaId")] RequiredIngredient requiredIngredient)
        {
            if (id != requiredIngredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(requiredIngredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RequiredIngredientExists(requiredIngredient.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "Id", requiredIngredient.IngredientId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id", requiredIngredient.PizzaId);
            return View(requiredIngredient);
        }

        // GET: RequiredIngredients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var requiredIngredient = await _context.RequiredIngredients
                .Include(r => r.Ingredient)
                .Include(r => r.Pizza)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (requiredIngredient == null)
            {
                return NotFound();
            }

            return View(requiredIngredient);
        }

        // POST: RequiredIngredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var requiredIngredient = await _context.RequiredIngredients.FindAsync(id);
            if (requiredIngredient != null)
            {
                _context.RequiredIngredients.Remove(requiredIngredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RequiredIngredientExists(int id)
        {
            return _context.RequiredIngredients.Any(e => e.Id == id);
        }
    }
}
