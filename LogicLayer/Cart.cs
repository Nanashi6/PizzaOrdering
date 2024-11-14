using PizzaOrdering.DataLayer.Models;
using PizzaOrdering.Models;

namespace PizzaOrdering.LogicLayer;

public class Cart
{
    public List<CartItem> pizzas { get; init; } = new List<CartItem>();

    public void AddItem(PizzaInfoViewModel pizza, int quantity)
    {
        CartItem? line = pizzas.FirstOrDefault(g => g.Pizza.Id == pizza.Id);

        if (line == null)
        {
            pizzas.Add(new CartItem
            {
                Pizza = pizza,
                Quantity = quantity
            });
        }
        else
        {
            line.Quantity += quantity;
        }
    }

    public void RemoveItem(Pizza pizza)
    {
        pizzas.RemoveAll(l => l.Pizza.Id == pizza.Id);
    }

    public void Clear()
    {
        pizzas.Clear();
    }
}

public class CartItem
{
    public PizzaInfoViewModel Pizza { get; set; }
    public int Quantity { get; set; }
}