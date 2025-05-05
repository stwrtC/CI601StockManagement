using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GenericStockManagement.Controllers
{
    public class ProductController : Controller
    {
        public readonly IRepository<Product> _productRepository;
        public readonly IRepository<Category> _categoryRepository;
        public readonly StockDbContext _context;

        public ProductController(IRepository<Product> productRepo, IRepository<Category> categoryRepo, StockDbContext context)
        {
            _productRepository = productRepo;
            _categoryRepository = categoryRepo;
            _context = context;
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return PartialView("Create", new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            ViewBag.Categories = _context.Categories.ToList();
            return PartialView("Create", product);
        }

    }
}


