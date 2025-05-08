using System.Diagnostics;
using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using GenericStockManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GenericStockManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly IRepository<Product> _productRepository;
        public readonly IRepository<Category> _categoryRepository;

        public HomeController(ILogger<HomeController> logger, IRepository<Product> productRepo, IRepository<Category> categoryRepo)
        {
            _logger = logger;
            _productRepository = productRepo;
            _categoryRepository = categoryRepo;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            var categories = _categoryRepository.GetAll();

            var model = new HomeViewModel(products, categories);


            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult StockActions(List<int> selectedIds, string submitButton)
        {
            if (submitButton == "Delete")
            {
                return RedirectToAction("Delete", "Product", new { ids = selectedIds});
            }
            else if (submitButton == "Update")
            {
                return RedirectToAction("Update", "Product", new { ids = selectedIds});
            }
            return RedirectToAction("Index");
        }
        public IActionResult CategoryActions(List<int> selectedCatIds, string submitButton)
        {
            if (submitButton == "Delete")
            {
                return RedirectToAction("Delete", "Category", new { ids = selectedCatIds });
            }
            else if (submitButton == "Update")
            {
                return RedirectToAction("Update", "Category", new { ids = selectedCatIds });
            }
            return RedirectToAction("Index");
        }

    }
}
