using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PizzaOrdering.DataLayer.Interfaces;
using PizzaOrdering.Models;

namespace PizzaOrdering.DataLayer.Models;

public class Pizza : ITable
{
    [Key]
    public int Id { get; init; }
    [Column]
    public string Name { get; set; }
    [Column]
    public int Size { get; set; }
    [Column]
    public double Price { get; set; }
    [JsonIgnore]
    public virtual IEnumerable<RequiredIngredient>? RequiredIngredients { get; set; }

    public static explicit operator CreatePizzaViewModel(Pizza pizza) {
      CreatePizzaViewModel result = new();
      
      result.Id = pizza.Id;
      result.Name = pizza.Name;
      result.Size = pizza.Size;
      result.Price = pizza.Price;
      result.IngredientIds = pizza.RequiredIngredients?.Select(i => i.Ingredient.Id).ToArray() ?? [];

      return result;
    }

    public void Update(CreatePizzaViewModel pizzaDto)
    {
        Name = pizzaDto.Name;
        Size = pizzaDto.Size;
        Price = pizzaDto.Price;
    }
}
