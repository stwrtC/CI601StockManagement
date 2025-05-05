using GenericStockManagement.Models;

namespace GenericStockManagement.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly StockDbContext _context;
        public CategoryRepository(StockDbContext context)
        {
            _context = context;
        }
        public Category Add(Category item)
        {
            _context.Categories.Add(item);
            _context.SaveChanges();
            return item;
        }
        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                _context.Categories.Remove(item);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Category> GetAll()
        {
            var categories = _context.Categories.ToList();
            return categories;
        }
        public Category? GetById(int id)
        {
            return _context.Categories.FirstOrDefault(x => x.Id == id);
        }
        public Category Update(Category item)
        {
            var category = GetById(item.Id);
            if (category != null)
            {
                category.Name = item.Name;
                _context.Categories.Update(category);
                _context.SaveChanges();
                return category;
            }
            return null;
        }
    }
}
