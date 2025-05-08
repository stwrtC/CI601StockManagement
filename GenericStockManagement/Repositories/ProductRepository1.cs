using GenericStockManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GenericStockManagement.Repositories
{
    public class ProductRepository1 : IRepository<Product>
    {
        private List<Product> _products;
        public ProductRepository1(StockDbContext context)
        {
            _products = new List<Product>();
        }
        public Product Add(Product item)
        {
            _products.Add(item);
            return item;
        }

        public void Delete(int id)
        {
            var item = GetById(id);
            if (item != null) { 
                _products.Remove(item);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        public Product Update(Product item)
        {
            var product = GetById(item.Id);
            if (product != null) { 
                product.Name = item.Name;
                product.Description = item.Description;
                product.Price = item.Price;
                product.Quantity = item.Quantity;
                product.Brand = item.Brand;
                product.ImageThumbnail = item.ImageThumbnail;

                return product;
            }
            return null;
        }
    }
}
