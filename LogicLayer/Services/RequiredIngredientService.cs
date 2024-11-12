using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class RequiredIngredientService : CRUDService<RequiredIngredient>
{
    public RequiredIngredientService(PizzeriaContext context) : base(context)
    {
    }
}