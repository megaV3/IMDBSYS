using Humanizer;
using IMDBSYS.Data;
using IMDBSYS.Models;
using IMDBSYS.ViewModels;
using IMDBSYS.ViewModels.Admin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace IMDBSYS.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Dashboard()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 1. Fetch full user metadata safely
            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null) return NotFound();

            var viewModel = new DashboardViewModel();

            // 1. Fetching Totals
            viewModel.TotalUsers = await _context.Users.CountAsync(); // Assuming you have a Users DbSet
            viewModel.TotalProducts = await _context.Menus.CountAsync();
            viewModel.TotalOrders = await _context.Orders.CountAsync();

            // Calculate total sales from completed orders
            viewModel.TotalSalesRevenue = await _context.Orders
                .Where(o => o.IsCompleted)
                .SumAsync(o => o.TotalAmount);

            // Calculate total successful wallet top-ups
            viewModel.TotalTopUpAmount = await _context.TopUpHistories
                .Where(t => t.Status == "Success" || t.Status == "Approved") // Adjust string based on your data
                .SumAsync(t => t.Amount);

            // 2. Inventory & Stock Monitoring
            viewModel.TotalStockQuantity = await _context.MenuVariations
                .SumAsync(mv => mv.StockQuantity);

            // Fetch products where inventory is less than or equal to their low-stock threshold
            viewModel.LowStockProducts = await _context.MenuVariations
                .Include(mv => mv.Menu) // Includes parent Menu info (Name, Category, etc.)
                .Where(mv => mv.StockQuantity <= mv.LowStockThreshold)
                .OrderBy(mv => mv.StockQuantity)
                .Take(5) // Limit to top 5 most urgent items
                .ToListAsync();

            // 3. Recent Activities
            viewModel.RecentOrders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .ToListAsync();

            viewModel.RecentTopUps = await _context.TopUpHistories
                .OrderByDescending(t => t.TransactionDate)
                .Take(5)
                .ToListAsync();

            // NEW QUERY: Fetch the latest 5 delivery intake records
            viewModel.RecentStockChanges = await _context.ProductDeliveryLogs
                .Include(l => l.MenuVariation)
                .ThenInclude(v => v.Menu) // Multi-level include to jump from Log -> Variant -> Parent Product Name
                .OrderByDescending(l => l.DeliveryDate)
                .Take(5)
                .ToListAsync();

            return View(viewModel);
        }

        // ==========================================
        // 2. PRODUCTS / MENU CATALOG
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> MenuIndex()
        {
            // .Include(m => m.Variations) ensures the View can count how many variations exist
            var menus = await _context.Menus
                .Include(m => m.Variations)
                .OrderBy(m => m.MenuId)
                .ToListAsync();

            return View("MenuIndex", menus); // Explicitly naming the view if it doesn't match action name
        }

        // ========================================================
        // GET: Admin/OrderRegistry
        // ========================================================
        [HttpGet]
        public async Task<IActionResult> OrderRegistry()
        {
            // Eagerly load order rows along with their corresponding line items and parent product metadata
            var entireOrdersLogList = await _context.Orders
                .Include(o => o.OrderItems)                           // 1. Include the collections of order items
                    .ThenInclude(i => i.Menu)                         // 2. CORRECTION: Include the Menu property that exists on OrderItem
                .OrderByDescending(o => o.OrderDate)                  // Serve the newest transactions first
                .ToListAsync();

            return View(entireOrdersLogList);
        }

        // ==========================================
        // 4. WALLET TOP-UPS LEDGER
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> TopUpLedger()
        {
            var topUps = await _context.TopUpHistories
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();

            return View("TopUpLedger", topUps);
        }

        // ==========================================
        // 5. USER ACCESS MANAGEMENT
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> UserManagement()
        {
            // Fetching users from your database context
            // Projecting them directly into a clear view model structure
            var users = await _context.Users
                .Select(u => new UserManagementViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role // Assuming "Admin" or "User" strings are saved here
                })
                .OrderBy(u => u.LastName)
                .ToListAsync();

            return View("UserManagement", users);
        }


        // ========================================================
        // GET: Admin/RequestRestock
        // ========================================================
        [HttpGet]
        public async Task<IActionResult> RequestRestock()
        {
            // Fetch all variants and include parent menu titles for dropdown assignment
            var availableVariants = await _context.MenuVariations
                .Include(v => v.Menu)
                .OrderBy(v => v.Menu.Name)
                .ThenBy(v => v.VariantName)
                .ToListAsync();

            // Pass the raw list directly down to the view!
            return View(availableVariants);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestRestock(int menuVariationId, int quantityReceived, decimal totalAmount)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            // 1. Fetch full user metadata safely
            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null) return NotFound();

            if (quantityReceived <= 0)
            {
                ModelState.AddModelError("", "Quantity must be at least 1 unit.");
            }
            if (totalAmount <= 0)
            {
                ModelState.AddModelError("", "Calculated total delivery cost must be a positive value.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Pull the targeted configuration variant record from the data tables
                    var variant = await _context.MenuVariations
                        .FirstOrDefaultAsync(v => v.MenuVariationId == menuVariationId);

                    if (variant == null)
                    {
                        return NotFound();
                    }

                    // 1. Math modification step: Update warehouse physical inventory count
                    variant.StockQuantity += quantityReceived;
                    _context.Update(variant);

                    // 2. Map metrics down safely into the updated strongly-typed database log entity columns
                    var historyLog = new ProductDeliveryLog
                    {
                        MenuVariationId = menuVariationId,
                        QuantityAdded = quantityReceived,
                        TotalAmount = totalAmount, // SAVED AS STRONGLY-TYPED NUMERIC TIERS
                        DeliveryDate = DateTime.Now,
                        ProcessedBy = userProfile.Username,
                        Remarks = "Verified Supplier Fulfillment Intake Batch Processing Run Pass"
                    };
                    _context.ProductDeliveryLogs.Add(historyLog);

                    // 3. Save as single database transaction block
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Dashboard));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "A critical data error occurred while updating stock profiles.");
                }
            }

            // Fallback block if any model state rules trigger issues
            var fallbackList = await _context.MenuVariations.Include(v => v.Menu).ToListAsync();
            return View(fallbackList);
        }

        // ========================================================
        // GET: Admin/RestockLedger
        // ========================================================
        [HttpGet]
        public async Task<IActionResult> RestockLedger()
        {
            // Gather all historical trace lines from your database context logging table
            var fullDeliveryLogs = await _context.ProductDeliveryLogs
                .Include(l => l.MenuVariation)
                .ThenInclude(v => v.Menu) // Join across to display product title parameters
                .OrderByDescending(l => l.DeliveryDate) // Serve fresh logs first
                .ToListAsync();

            return View(fullDeliveryLogs);
        }

        // ========================================================
        // POST: Admin/EditUserRole
        // ========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserRole(int id, string role)
        {
            // 1. Target the designated user domain entry from the account context mapping layer
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // 2. Map down the updated security entitlement context string field
            user.Role = role;
            _context.Update(user);
            await _context.SaveChangesAsync();

            // 3. Seamlessly kick back to refresh the active page listing state data 
            return RedirectToAction(nameof(UserManagement));
        }

        // ========================================================
        // POST: Admin/DeleteUser
        // ========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Perform an immediate destructive row removal drop execution sequence pass
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(UserManagement));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Menu incomingMenu)
        {
            if (incomingMenu == null)
            {
                return BadRequest();
            }

            try
            {
                // 1. Pull the existing product from the database including its variants
                var dbMenu = await _context.Menus
                    .Include(m => m.Variations)
                    .FirstOrDefaultAsync(m => m.MenuId == incomingMenu.MenuId);

                if (dbMenu == null)
                {
                    return NotFound();
                }

                // 2. Update the parent product parameters
                dbMenu.Name = incomingMenu.Name;
                dbMenu.Category = incomingMenu.Category;
                dbMenu.Price = incomingMenu.Price;
                dbMenu.Description = incomingMenu.Description;

                // 3. Loop and safely update the inner retail variant rows
                if (incomingMenu.Variations != null && dbMenu.Variations != null)
                {
                    foreach (var incomingVar in incomingMenu.Variations)
                    {
                        var dbVar = dbMenu.Variations
                            .FirstOrDefault(v => v.MenuVariationId == incomingVar.MenuVariationId);

                        if (dbVar != null)
                        {
                            // Update only the editable field we targeted in the modal form collection matrix
                            dbVar.Price = incomingVar.Price;

                            // Updates base price if the standard variation is updated, ensuring the main product price always reflects the default variant's price for storefront display logic
                            if (dbVar.MenuVariationId == 1)
                            {
                                dbMenu.Price = dbVar.Price;
                            }
                        }

                    }
                }

                // 4. Commit everything to the database registry in a single batch transaction pass
                _context.Update(dbMenu);
                await _context.SaveChangesAsync();

                // 5. Redirection slot: Kick the admin straight back to the same page without full-view reloads!
                return RedirectToAction(nameof(MenuIndex));
            }
            catch (DbUpdateConcurrencyException)
            {
                // Log error logic here if checking missing connection traces
                ModelState.AddModelError("", "Unable to save changes. The database record was updated by another user process.");
            }

            // If something fails, drop back into the main index view safely with validation state messages
            var menus = await _context.Menus.Include(m => m.Variations).ToListAsync();
            return View("MenuIndex", menus);
        }


        // ========================================================
        // GET: Admin/ProfileDetails
        // ========================================================
        [HttpGet]
        public async Task<IActionResult> ProfileDetails()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null)
            {
                return NotFound();
            }

            var adminProfile = new
            {
                FullName = $"{userProfile.FirstName} {userProfile.LastName}",
                Email = userProfile.Email,
                Role = userProfile.Role ?? "Admin",
                SecurityScope = "Global Read/Write/Delete Permissions",
                AccountStatus = "Active",
                // SYNC: Point directly to your model's ProfileImagePath property
                ProfilePicturePath = !string.IsNullOrEmpty(userProfile.ProfileImagePath)
                    ? userProfile.ProfileImagePath.Replace("~", "") // Strip the tilde out for clear standard HTML rendering
                    : "/images/profiles/default-picture.webp"
            };

            ViewBag.Profile = adminProfile;
            return View();
        }

        // ========================================================
        // POST: Admin/ProfileDetails
        // ========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfileDetails(string firstName, string lastName, IFormFile? profilePicture)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return RedirectToAction("Login", "Auth");
            }

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                ModelState.AddModelError("", "First Name and Last Name cannot be blank.");
                return RedirectToAction(nameof(ProfileDetails));
            }

            var userProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userProfile == null)
            {
                return NotFound();
            }

            if (profilePicture != null && profilePicture.Length > 0)
            {
                // Save files cleanly inside your preset profiles folder footprint
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var cleanFileName = $"admin_{userProfile.Id}_{DateTime.Now.Ticks}{Path.GetExtension(profilePicture.FileName)}";
                var filePath = Path.Combine(uploadFolder, cleanFileName);

                // SYNC: Check and delete old unique asset files before overriding paths
                if (!string.IsNullOrEmpty(userProfile.ProfileImagePath) && !userProfile.ProfileImagePath.Contains("default-picture.webp"))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", userProfile.ProfileImagePath.Replace("~", "").TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePicture.CopyToAsync(stream);
                }

                // SYNC: Save structural string using standard web root mapping syntax
                userProfile.ProfileImagePath = $"/images/profiles/{cleanFileName}";
            }

            userProfile.FirstName = firstName.Trim();
            userProfile.LastName = lastName.Trim();

            _context.Update(userProfile);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ProfileDetails));
        }

        // ==========================================
        // ADMIN LOGOUT
        // ==========================================

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
