using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class IngredientService : CRUDService<Ingredient>
{
    private readonly PizzeriaContext _context;

    public IngredientService(PizzeriaContext context) : base(context)
    {
        _context = context;
    }

    public async Task DecriseIngredients(IEnumerable<int> ingredientIds)
    {
        foreach (var ingredientId in ingredientIds)
        {
            Ingredient ingredient = Read(ingredientId);
            ingredient.Quantity -= 1;
            _context.Set<Ingredient>().Update(ingredient);
            _context.SaveChangesAsync();
        }
        
    }
}