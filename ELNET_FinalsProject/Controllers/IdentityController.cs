using ELNET_FinalsProject.Data;
using ELNET_FinalsProject.Models;
using ELNET_FinalsProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ELNET_FinalsProject.Controllers
{
    public class IdentityController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public IdentityController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(User register)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(r => r.Username == register.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists.");
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
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(l => l.Email == login.Email && l.Password == login.Password);

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FirstName),
                        new Claim("LastLogin", DateTime.Now.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Store");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password.");
                    return View(login);
                }
            }
            return View(login);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return RedirectToAction("Login");

            var userId = int.Parse(userIdClaim);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Login");

            // 2. Calculate Cart Count from the database
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.UserId == userId && !o.IsCompleted);

            // Sum up the quantities of all items in the cart
            int count = order?.OrderItems.Sum(oi => oi.Quantity) ?? 0;

            var vm = new ProfileViewModel
            {
                Balance = user.Balance,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImagePath = user.ProfileImagePath,
                CartCount = count
            };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return RedirectToAction("Login");

            var userId = int.Parse(userIdClaim);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToAction("Login");

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;

            if (vm.ProfileImage != null && vm.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profiles");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.ProfileImage.CopyToAsync(stream);

                user.ProfileImagePath = "/uploads/profiles/" + fileName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var user = await _context.Users.FindAsync(int.Parse(userIdClaim));
            if (user == null) return NotFound();

            var vm = new ProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                ProfileImagePath = user.ProfileImagePath,
                Balance = user.Balance

            };

            return View(vm);

            return Json(new
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                profileImagePath = user.ProfileImagePath
            });
        }

        // For saving changes of user's profile
        [HttpPost]
        public async Task<IActionResult> ProfileAjax(ProfileViewModel vm)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            var user = await _context.Users.FindAsync(int.Parse(userIdClaim));
            if (user == null) return NotFound();

            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;

            if (vm.ProfileImage != null && vm.ProfileImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "profiles");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await vm.ProfileImage.CopyToAsync(stream);

                user.ProfileImagePath = "/uploads/profiles/" + fileName;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Identity");
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

        public IActionResult TopUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProcessTopUp(TopUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Handle validation errors (e.g., negative amount)
                return RedirectToAction("Index", "Store");
            }

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            // 1. Update the User's Wallet Balance
            var user = await _context.Users.FindAsync(int.Parse(userIdClaim));
            user.Balance += model.Amount;

            // 2. Create the History Record
            var history = new TopUpHistory
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Amount = model.Amount,
                TransactionDate = DateTime.Now,
                Email = user.Email,
                PaymentMethod = model.PaymentMethod, // This can be dynamic based on user input
                Status = "Success"
            };

            _context.Users.Update(user);
            _context.TopUpHistories.Add(history);
            await _context.SaveChangesAsync();

            return RedirectToAction("Profile", "Identity"); // Refresh the profile page to show new balance
        }
    }
}