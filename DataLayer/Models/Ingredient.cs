using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PizzaOrdering.DataLayer.Interfaces;

namespace PizzaOrdering.DataLayer.Models;

public class Ingredient : ITable
{
    [Key]
    public int Id { get; init; }
    [Column]
    public string Name { get; init; }
    [Column]
    public int Quantity { get; set; }
    
    [JsonIgnore]
    public virtual IEnumerable<RequiredIngredient>? RequiredIngredients { get; set; }
}