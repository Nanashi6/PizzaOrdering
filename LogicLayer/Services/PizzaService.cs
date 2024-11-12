using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class PizzaService : CRUDService<Pizza>
{
    public PizzaService(PizzeriaContext context) : base(context)
    {
    }
}