using System.ComponentModel.DataAnnotations;

namespace PizzaOrdering.DataLayer.Interfaces;

public interface ITable
{
    [Key]
    int Id { get; init; }
}