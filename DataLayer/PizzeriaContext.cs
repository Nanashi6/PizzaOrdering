using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PizzaOrdering.DataLayer.Models;

namespace PizzaOrdering.DataLayer;

public class PizzeriaContext : IdentityDbContext<User>
{
    public PizzeriaContext(DbContextOptions<PizzeriaContext> options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = true;
        Database.EnsureCreated();
    }
    
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<RequiredIngredient> RequiredIngredients { get; set; }
    public DbSet<PizzaRequired> PizzasRequired { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLazyLoadingProxies();
    }
}