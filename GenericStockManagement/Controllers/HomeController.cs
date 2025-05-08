using System.Diagnostics;
using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using GenericStockManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

            var categoryCounts = categories.Select(category => new
            {
                CategoryName = category.Name,
                ProductCount = products.Count(p => p.CategoryId == category.Id),
                TotalValue = products.Where(p => p.CategoryId == category.Id).Sum(p => p.Price * p.Quantity),
                TotalStock = products.Where(p => p.CategoryId == category.Id).Sum(p => p.Quantity)
            }).ToList();

            ViewBag.CategoryCounts = JsonConvert.SerializeObject(categoryCounts);

            return View();
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
