using GenericStockManagement.Models;
using GenericStockManagement.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GenericStockManagement.Controllers
{
    public class CategoryController : Controller
    {
        public readonly IRepository<Category> _categoryRepo;

        public CategoryController(IRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
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
                return RedirectToAction("List");
            }

            return View(newCategory);
        }
    }
}
