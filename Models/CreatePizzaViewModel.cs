using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.Models;

public class CreatePizzaViewModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int Size { get; set; }
    [Required]
    public double Price { get; set; }
    
    [Required]
    public IEnumerable<int> IngredientIds { get; set; } = Enumerable.Empty<int>();
}