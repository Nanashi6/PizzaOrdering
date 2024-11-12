using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using PizzaOrdering.DataLayer.Interfaces;

namespace PizzaOrdering.DataLayer.Models;

public class RequiredIngredient : ITable
{
    [Key]
    public int Id { get; init; }
    
    [Column]
    public int IngredientId { get; init; }
    [JsonIgnore]
    public virtual Ingredient?  Ingredient { get; init; }
    
    [Column]
    public int PizzaId { get; init; }
    [JsonIgnore]
    public virtual Pizza? Pizza { get; init; }
}