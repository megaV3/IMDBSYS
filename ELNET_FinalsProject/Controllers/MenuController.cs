using Microsoft.AspNetCore.Mvc;
using ELNET_FinalsProject.Models;
using ELNET_FinalsProject.Data;
using System.Runtime.InteropServices;

namespace ELNET_FinalsProject.Controllers
{
    public class MenuController : Controller
    {
        private readonly AppDbContext _context;

        public MenuController(AppDbContext context)
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
                if (_context.Users.Any(r => r.Username == register.Username)) //checks if the username already exissts in the database
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
            var user = _context.Users.FirstOrDefault(l => l.Username == login.Username && l.Password == login.Password);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    RedirectToAction("Index");
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
