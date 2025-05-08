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
        public CategoryController(IRepository<Category> categoryRepo, IRepository<Product> productRepository)
        {
            _categoryRepo = categoryRepo;
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult<Category> Create(Category entity)
        {
            var newCategory = new Category()
            {
                Name = entity.Name,
                Description = entity.Description,
            };

            if (ModelState.IsValid)
            {
                _categoryRepo.Add(newCategory);
                return RedirectToAction("Index", "Home");
            }

            return View(newCategory);
        }

        public IActionResult Delete(List<int> ids)
        {
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
            foreach (var id in selectedIds)
            {
                _categoryRepo.Delete(id);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Update(List<int> ids)
        {
            var categories = ids.Select(id => _categoryRepo.GetById(id)).ToList();
            var viewModel = new CategoryListViewModel(categories, null);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmUpdate(List<Category> updatedCategories)
        {
            foreach (var category in updatedCategories)
            {
                _categoryRepo.Update(category);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
