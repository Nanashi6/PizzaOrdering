using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using PizzaOrdering.DataLayer.Interfaces;

namespace PizzaOrdering.DataLayer.Models;

public class User : IdentityUser, ITable
{
    [Key]
    public int Id { get; init; }
    
    [Column]
    public string Name { get; set; }

    [Column] public string Surname { get; set; }

    [JsonIgnore]
    public virtual IEnumerable<Order>? Orders { get; set; }
}