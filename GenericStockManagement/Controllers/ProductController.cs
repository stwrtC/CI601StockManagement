using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using GenericStockManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GenericStockManagement.Controllers
{
    public class ProductController : Controller
    {
        public readonly IRepository<Product> _productRepository;
        public readonly IRepository<Category> _categoryRepository;

        public ProductController(IRepository<Product> productRepo, IRepository<Category> categoryRepo)
        {
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

        public IActionResult StockActions(List<int> selectedIds, string submitButton)
        {
            if (submitButton == "Delete")
            {
                return RedirectToAction("Delete", "Product", new { ids = selectedIds });
            }
            else if (submitButton == "Update")
            {
                return RedirectToAction("Update", "Product", new { ids = selectedIds });
            }
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Categories = _categoryRepository.GetAll();
            return View("Create", new Product());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);
                return RedirectToAction("Index", "Product");

            }
            ViewBag.Categories = _categoryRepository.GetAll();
            return View("Create", product);
        }

        public IActionResult Delete(List<int> ids)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            var categories = _categoryRepository.GetAll();
            var products = new List<Product>();
            foreach (var id in ids)
            {
                products.Add(_productRepository.GetById(id));
            }
            HomeViewModel productsViewModel = new HomeViewModel(products, categories);
            return View(productsViewModel);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(List<int> selectedIds)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            foreach (var id in selectedIds)
            {
                _productRepository.Delete(id);
            }
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Update(List<int> ids)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            var categories = _categoryRepository.GetAll();
            var products = ids
                .Select(id => _productRepository.GetById(id))
                .Where(p => p != null)
                .Select(p => new ProductEditViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    CategoryId = p.CategoryId,
                    Brand = p.Brand,
                    Description = p.Description,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    ImageThumbnail = p.ImageThumbnail
                }).ToList();

            var viewModel = new ProductUpdateViewModel
            {
                Products = products,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmUpdate(List<ProductEditViewModel> updatedProducts)
        {
            if (HttpContext.Session.GetString("Role") != "Admin" && HttpContext.Session.GetString("Role") != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            foreach (var vm in updatedProducts)
            {
                var product = _productRepository.GetById(vm.Id);
                if (product != null)
                {
                    product.Name = vm.Name;
                    product.CategoryId = vm.CategoryId;
                    product.Brand = vm.Brand;
                    product.Description = vm.Description;
                    product.Price = vm.Price;
                    product.Quantity = vm.Quantity;
                    product.ImageThumbnail = vm.ImageThumbnail;

                    _productRepository.Update(product);
                }
            }

            return RedirectToAction("Index", "Product");
        }
    }
}


