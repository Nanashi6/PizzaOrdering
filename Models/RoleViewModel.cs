using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.Models;

public class RoleViewModel
{
    [Required]
    public string Name { get; set; }
}