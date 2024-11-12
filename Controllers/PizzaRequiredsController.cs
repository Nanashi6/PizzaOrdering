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
    public class PizzaRequiredsController : Controller
    {
        private readonly PizzeriaContext _context;

        public PizzaRequiredsController(PizzeriaContext context)
        {
            _context = context;
        }

        // GET: PizzaRequireds
        public async Task<IActionResult> Index()
        {
            var pizzeriaContext = _context.PizzasRequired.Include(p => p.Order).Include(p => p.Pizza);
            return View(await pizzeriaContext.ToListAsync());
        }

        // GET: PizzaRequireds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaRequired = await _context.PizzasRequired
                .Include(p => p.Order)
                .Include(p => p.Pizza)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaRequired == null)
            {
                return NotFound();
            }

            return View(pizzaRequired);
        }

        // GET: PizzaRequireds/Create
        public IActionResult Create()
        {
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id");
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id");
            return View();
        }

        // POST: PizzaRequireds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PizzaId,OrderId")] PizzaRequired pizzaRequired)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pizzaRequired);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", pizzaRequired.OrderId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id", pizzaRequired.PizzaId);
            return View(pizzaRequired);
        }

        // GET: PizzaRequireds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaRequired = await _context.PizzasRequired.FindAsync(id);
            if (pizzaRequired == null)
            {
                return NotFound();
            }
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", pizzaRequired.OrderId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id", pizzaRequired.PizzaId);
            return View(pizzaRequired);
        }

        // POST: PizzaRequireds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PizzaId,OrderId")] PizzaRequired pizzaRequired)
        {
            if (id != pizzaRequired.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pizzaRequired);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PizzaRequiredExists(pizzaRequired.Id))
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
            ViewData["OrderId"] = new SelectList(_context.Orders, "Id", "Id", pizzaRequired.OrderId);
            ViewData["PizzaId"] = new SelectList(_context.Pizzas, "Id", "Id", pizzaRequired.PizzaId);
            return View(pizzaRequired);
        }

        // GET: PizzaRequireds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pizzaRequired = await _context.PizzasRequired
                .Include(p => p.Order)
                .Include(p => p.Pizza)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pizzaRequired == null)
            {
                return NotFound();
            }

            return View(pizzaRequired);
        }

        // POST: PizzaRequireds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pizzaRequired = await _context.PizzasRequired.FindAsync(id);
            if (pizzaRequired != null)
            {
                _context.PizzasRequired.Remove(pizzaRequired);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PizzaRequiredExists(int id)
        {
            return _context.PizzasRequired.Any(e => e.Id == id);
        }
    }
}
