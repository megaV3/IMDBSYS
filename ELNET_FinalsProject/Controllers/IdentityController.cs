using ELNET_FinalsProject.Data;
using ELNET_FinalsProject.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace ELNET_FinalsProject.Controllers
{
    public class IdentityController : Controller
    {
        private readonly AppDbContext _context;

        public IdentityController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //Displays the form
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        //Handles form submission
        [HttpPost]
        public IActionResult CreateAccount(User register)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(r => r.Username == register.Username)) //checks if the username already exissts in the database. Returns a boolean.
                { 
                    return View(register);
                }

                _context.Users.Add(register);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(register);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User login)
        {
            var user = _context.Users.FirstOrDefault(l => l.Username == login.Username && l.Password == login.Password); //Returns the actual user if found. Returns null if no match.

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim("LastLogin", DateTime.Now.ToString())
                    };

                    // 2. Create the Identity and Principal
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 3. This line "signs them in" by creating the encrypted cookie
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    /* 
                    If there are no users in the database, or if the model state is invalid, return the login view with the provided login
                    data (which may include validation errors).
                    */

                    return RedirectToAction("Index");
                }
                else
                {
                    return View(login);
                }
            }

            return View(login);
        }


        public IActionResult Menu()
        {
            return View();
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
