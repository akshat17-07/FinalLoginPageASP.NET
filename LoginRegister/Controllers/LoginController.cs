using LoginRegister.Data;
using LoginRegister.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegister.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDbContext _database;

        public LoginController(UserDbContext database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserModel model)
        {
            _database.LoginDetail.Add(model);
            _database.SaveChanges();



            return RedirectToAction("Index", "Home");
        }
    }
}
