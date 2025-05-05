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
    }
}
