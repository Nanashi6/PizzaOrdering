using System.ComponentModel.DataAnnotations;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.Models;

public class PizzaInfoViewModel
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
    public IEnumerable<Ingredient> Ingredients { get; set; } = Enumerable.Empty<Ingredient>();
}