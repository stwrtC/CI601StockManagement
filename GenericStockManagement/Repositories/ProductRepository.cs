using GenericStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericStockManagement.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly StockDbContext _context;

        public ProductRepository(StockDbContext context)
        {
            _context = context;
        }
        public Product Add(Product item)
        {
            _context.Products.Add(item);
            _context.SaveChanges();
            return item;
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null) { 
                _context.Products.Remove(item);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            var products = _context.Products.ToList();
            return products;
        }

        public Product? GetById(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }

        public Product Update(Product item)
        {
            var product = GetById(item.Id);
            if (product != null) { 
                product.Name = item.Name;
                product.Description = item.Description;
                product.Price = item.Price;
                product.Category = item.Category;
                product.Quantity = item.Quantity;
                product.Brand = item.Brand;
                product.ImageThumbnail = item.ImageThumbnail;

                _context.Products.Update(product);
                _context.SaveChanges();
                return product;
            }
            return null;
        }
    }
}
