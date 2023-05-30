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
            if (ModelState.IsValid && _database.LoginDetail.Find(model.Email) == null)
            {
                _database.LoginDetail.Add(model);
                _database.SaveChanges();



                return RedirectToAction("login", "Login");
            }
            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserModel model)
        {
            UserModel temp = _database.LoginDetail.Find(model.Email);

            if (temp != null && temp.Email == model.Email && temp.Password == model.Password)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}

