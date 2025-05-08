using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using GenericStockManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GenericStockManagement.Controllers
{
    public class CategoryController : Controller
    {
        public readonly IRepository<Category> _categoryRepo;
        public readonly IRepository<Product> _productRepository;
        private readonly ISessionService _sessionService;
        public CategoryController(IRepository<Category> categoryRepo, IRepository<Product> productRepository, ISessionService sessionService)
        {
            _categoryRepo = categoryRepo;
            _productRepository = productRepository;
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            var categories = _categoryRepo.GetAll();

            var model = new HomeViewModel(products, categories);


            return View(model);
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
        public IActionResult Create()
        {
            if (_sessionService.GetRole() != "Admin" && _sessionService.GetRole() != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category entity)
        {
            if (_sessionService.GetRole() != "Admin" && _sessionService.GetRole() != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            var newCategory = new Category()
            {
                Name = entity.Name,
                Description = entity.Description,
            };

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(newCategory);
                return RedirectToAction("Index", "Category");
            }

            return View(newCategory);
        }

        public IActionResult Delete(List<int> ids)
        {
            if (_sessionService.GetRole() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            var categories = new List<Category>();
            var categoryProducts = new Dictionary<int, List<Product>>();

            foreach (var id in ids)
            {
                var category = _categoryRepo.GetById(id);
                if (category != null)
                {
                    categories.Add(category);
                    var products = _productRepository.GetAll().Where(p => p.CategoryId == id).ToList();
                    categoryProducts[id] = products;
                }
            }

            var viewModel = new CategoryListViewModel(categories, categoryProducts);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(List<int> selectedIds)
        {
            if (_sessionService.GetRole() != "Admin")
            {
                return RedirectToAction("Login", "Account");
            }
            foreach (var id in selectedIds)
            {
                _categoryRepo.Delete(id);
            }
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Update(List<int> ids)
        {
            if (_sessionService.GetRole() != "Admin" && _sessionService.GetRole() != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            var categories = ids.Select(id => _categoryRepo.GetById(id)).ToList();
            var viewModel = new CategoryListViewModel(categories, null);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmUpdate(List<Category> updatedCategories)
        {
            if (_sessionService.GetRole() != "Admin" && _sessionService.GetRole() != "Contributor")
            {
                return RedirectToAction("Login", "Account");
            }
            foreach (var category in updatedCategories)
            {
                _categoryRepo.Update(category);
            }
            return RedirectToAction("Index", "Category");
        }
    }
}
