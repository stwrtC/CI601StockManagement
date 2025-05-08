using GenericStockManagement.ViewModels;
using GenericStockManagement;
using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly StockDbContext _context;

    public LoginController(StockDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    public IActionResult Index(LoginViewModel model)
    {
        string inputHash = PasswordHelper.HashPassword(model.Password);
        var user = _context.Users
            .FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == inputHash);
        if (user != null)
        {
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Invalid credentials";
        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
