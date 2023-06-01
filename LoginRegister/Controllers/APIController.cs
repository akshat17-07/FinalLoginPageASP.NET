using LoginRegister.Data;
using LoginRegister.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LoginRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class APIController : ControllerBase
    {
        private readonly UserDbContext _database;
        public APIController(UserDbContext database)
        {
            _database = database;
        }

        // GET: api/<APIController>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<UserModel>> Get()
        {
            var list = _database.LoginDetail.ToList();
            return Ok(list);
        }

        // GET api/<APIController>/5
        [Authorize]
        [HttpGet("{Email}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult Get(string Email)
        {
            var user = _database.LoginDetail.FirstOrDefault(u => u.Email == Email);
            if (user == null)
            {
                return NotFound();
            }
            var list = _database.LoginDetail.FirstOrDefault(u => u.Email == Email);
            return Ok(list);
        }

        // POST api/<APIController>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(408)]
        [ProducesResponseType(404)]
        public ActionResult<UserModel> Post([FromBody] UserModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                if (_database.LoginDetail.Find(model.Email) != null)
                {
                    return Conflict();
                }
                _database.LoginDetail.Add(model);
                _database.SaveChanges();
                return Ok();
            }



        }

        // PUT api/<APIController>/5
        [HttpPut]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public ActionResult Put([FromBody] UserModel value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _database.LoginDetail.Update(value);
            _database.SaveChanges();
            return Ok();
        }

        // DELETE api/<APIController>/5
        [HttpDelete("{Email}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult Delete(UserModel Email)
        {
            var user = _database.LoginDetail.Find(Email.Email);
            if (user == null)
            {
                return NotFound();
            }
            _database.LoginDetail.Remove(user);
            _database.SaveChanges();
            return Ok();

        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromBody] UserModel model)
        {
            UserModel temp = _database.LoginDetail.Find(model.Email);

            if (temp != null && temp.Email == model.Email && temp.Password == model.Password)
            {
                var identity = new ClaimsIdentity(new[] {new Claim(
                    ClaimTypes.Name, temp.FirstName) }, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("Username", temp.Email);

                return Ok();
            }


            return BadRequest();
        }

        [HttpPost]
        [Route("logout")]
        [Authorize]
        public ActionResult LogOut()
        {

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return Ok();
        }
    }
}
