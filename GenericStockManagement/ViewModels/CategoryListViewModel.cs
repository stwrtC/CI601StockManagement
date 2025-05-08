using GenericStockManagement.Models;

namespace GenericStockManagement.ViewModels
{
    public class CategoryListViewModel
    {
        public IEnumerable<Category> _categories { get; set; } = new List<Category>();

        public Dictionary<int, List<Product>> CategoryProducts { get; set; } = new Dictionary<int, List<Product>>();

        public CategoryListViewModel(IEnumerable<Category> categories, Dictionary<int, List<Product>>? categoryProducts)
        {
            _categories = categories;
            CategoryProducts = categoryProducts;
        }
    }
}
