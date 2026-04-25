using Microsoft.AspNetCore.Mvc;

namespace ELNET_FinalsProject.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
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
