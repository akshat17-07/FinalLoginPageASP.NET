using LoginRegister.Data;
using LoginRegister.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LoginRegister.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserDbContext _database;

        public LoginController(UserDbContext database)
        {
            _database = database;
        }

        // Get Request
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Welcome", "Login");
            }
            else { 
                return RedirectToAction("Login", "Login");
            }
        }

        public IActionResult LogOut() {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies) {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Login", "Login");
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Welcome", "Login");
            }
            return View();
        }

        public IActionResult Login()
        {
            TempData["errorMessage"] = "";
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Welcome", "Login");
            }
            return View();
        }

        [Authorize]
        public IActionResult Welcome() {
            
            return View(_database.LoginDetail.ToList());
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
                var identity = new ClaimsIdentity(new[] {new Claim(
                    ClaimTypes.Name, temp.FirstName) }, CookieAuthenticationDefaults.AuthenticationScheme);
                
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("Username", temp.Email);

                return RedirectToAction ("Welcome", "Login");
            }
            TempData["errorMessage"] = "Username or Password is incorrect";

            return View();
        }
        
    }
}

