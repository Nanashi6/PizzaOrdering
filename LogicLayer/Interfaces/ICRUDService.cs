namespace PizzaOrdering.LogicLayer.Interfaces;

public interface ICRUDService<T>
{
    T? Read(int? id);
    IEnumerable<T> ReadAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(int? id);
}