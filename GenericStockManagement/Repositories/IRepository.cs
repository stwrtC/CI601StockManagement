using GenericStockManagement.Models;

namespace GenericStockManagement.Repositories
{
    public interface IRepository<T> where T: class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        T Add(T item);
        T Update(T stock);
        void Delete(int id);
    }
}
