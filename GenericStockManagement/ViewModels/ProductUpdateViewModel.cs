using GenericStockManagement.Models;

namespace GenericStockManagement.ViewModels
{
    public class ProductUpdateViewModel
    {
        public List<ProductEditViewModel> Products { get; set; } = new();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
