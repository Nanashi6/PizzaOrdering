using PizzaOrdering.DataLayer;
using PizzaOrdering.DataLayer.Interfaces;
using PizzaOrdering.LogicLayer.Interfaces;

namespace PizzaOrdering.LogicLayer;

public class CRUDService<T> : ICRUDService<T> where T : class, ITable
{
    private PizzeriaContext _context;

    public CRUDService(PizzeriaContext context)
    {
        _context = context;
    }
    
    public T? Read(int? id)
    {
        return _context.Set<T>().Find(id); 
    }

    public IEnumerable<T> ReadAll()
    {
        return _context.Set<T>().ToList();
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int? id)
    {
        _context.Set<T>().Remove(Read(id));
        _context.SaveChanges();
    }
}