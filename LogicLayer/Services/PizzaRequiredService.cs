using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class PizzaRequiredService : CRUDService<PizzaRequired>
{
    public PizzaRequiredService(PizzeriaContext context) : base(context)
    {
    }
}