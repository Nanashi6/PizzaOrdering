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

    public void Update(CreatePizzaViewModel pizzaDto)
    {
        Name = pizzaDto.Name;
        Size = pizzaDto.Size;
        Price = pizzaDto.Price;
    }
}