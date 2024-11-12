using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.LogicLayer.Services;

public class OrderService : CRUDService<Order>
{
    private readonly PizzeriaContext _context;

    public OrderService(PizzeriaContext context) : base(context)
    {
        _context = context;
    }

    public void IncreasePizzaCount(int orderId, int count)
    {
        Order order = Read(orderId);
        order.PizzasCount += count;
        _context.Set<Order>().Update(order);
        _context.SaveChanges();
    }
    
    public void DecriesePizzaCount(int orderId, int count)
    {
        Order order = Read(orderId);
        order.PizzasCount -= count;
        _context.Set<Order>().Update(order);
        _context.SaveChanges();
    }
}