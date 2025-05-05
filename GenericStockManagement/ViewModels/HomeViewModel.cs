using GenericStockManagement.Models;

namespace GenericStockManagement.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> _products { get; set; } = new List<Product>();
        public IEnumerable<Category> _categories { get; set; } = new List<Category>();

        public HomeViewModel(IEnumerable<Product> products, IEnumerable<Category> categories)
        {
            _categories = categories;
            _products = products;
        }
    }
}
