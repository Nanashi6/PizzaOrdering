using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.Models;

public class CreateOrderViewModel
{
    [Required]
    public int Id { get; init; }
    [Required]
    public string Address { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public DateTime DeliveryDate { get; set; }
}