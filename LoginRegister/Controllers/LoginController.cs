using LoginRegister.Data;
using LoginRegister.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoginRegister.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDbContext _database;

        public int _isactive { get; set; }

        public LoginController(UserDbContext database)
        {
            _database = database;
            _isactive = 0;
        }

        private void LoginUser() {
            _isactive = 1;
        }

        private void LogoutUser() {
            _isactive = 0;
        }

        public bool InSession() {
            if (_isactive != 0)
            {
                return true;
            }
            else {
                return false;
            }
        }

        // Get Request
        public IActionResult Index()
        {
            if (_isactive != 0)
            {
                return RedirectToAction("Welcome", "Login");
            }
            else { 
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Welcome() {
            return View();
        }

        // Post Request
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserModel model)
        {
            UserModel temp = _database.LoginDetail.Find(model.Email);

            if (temp != null && temp.Email == model.Email && temp.Password == model.Password)
            {
                _isactive = 1;
                bool t = InSession();

                if (t == true) {
                    return RedirectToAction("Index", "Login"); }
                return View();
            }

            return View();
        }
    }
}

