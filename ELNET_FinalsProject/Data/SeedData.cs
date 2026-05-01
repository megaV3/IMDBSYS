using ELNET_FinalsProject.Models;
using Microsoft.EntityFrameworkCore;

namespace ELNET_FinalsProject.Data
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            AppDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        FirstName = "John Wayne",
                        LastName = "Yabao",
                        Username = "heatedrivalry",
                        Email = "johnwayneyabao@gmail.com",
                        Password = "ilovedilao",

                    });
                context.SaveChanges();
            }

            if (!context.Menus.Any())
            {
                context.Menus.AddRange(
                    new Menu
                    {
                        Name = "Kape Kastila",
                        Category = "Signatures",
                        Price = 75m,
                        Description = "Leche condensada with creamy milk and rich espresso. Also available hot.",
                        CanBeHot = true,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Iced White Mocha Latte",
                        Category = "Signatures",
                        Price = 89m,
                        Description = "White chocolate with our creamy milk blend and rich espresso, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Iced Caramel Macchiato",
                        Category = "Signatures",
                        Price = 89m,
                        Description = "Rich espresso with creamy milk and caramel, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Pickup Crème Latte",
                        Category = "Signatures",
                        Price = 95m,
                        Description = "A latte made with rich espresso and caramelized custard crème, topped with a Crème Brûlée foam. Proudly our crème de la crème!",
                        CanBeHot = false,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Cappuccino",
                        Category = "Espresso-based",
                        Price = 75m,
                        Description = "Rich espresso with hot steamed milk and foam.",
                        CanBeHot = true,
                        CanBeCold = false
                    },
                    new Menu
                    {
                        Name = "Iced Brown Sugar Latter",
                        Category = "Espresso-based",
                        Price = 79m,
                        Description = "Rich espresso with creamy milk and brown molasses syrup, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Sea Salt Biscoff Latte",
                        Category = "Espresso-based",
                        Price = 115m,
                        Description = "Rich espresso with creamy milk, Biscoff cookie bits, topped with sea salt cream mousse.",
                        CanBeHot = false,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Iced Sea Salt Latte",
                        Category = "Espresso-based",
                        Price = 89m,
                        Description = "Rich espresso with creamy milk, topped with sea salt milk foam.",
                        CanBeHot = false,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Vietnamese Latte",
                        Category = "Espresso-based",
                        Price = 79m,
                        Description = "A strong blend of rich espresso with leche condensada.",
                        CanBeHot = true,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Sea Salt Pistachio Milk",
                        Category = "Pistachio",
                        Price = 119m,
                        Description = "Creamy milk with rich pistachio, topped with sea salt milk foam.",
                        CanBeHot = false,
                        CanBeCold = true
                    },
                    new Menu
                    {
                        Name = "Hot Auro Chocolate Pistachio Latte",
                        Category = "Pistachio",
                        Price = 129m,
                        Description = "Hot steamed milk with rich pistachio, Auro’s 100% Natural Cacao Powder, and bold espresso.",
                        CanBeHot = true,
                        CanBeCold = false
                    },
                    new Menu
                    {
                        Name = "Hot Auro Chocolate Pistachio Milk",
                        Category = "Pistachio",
                        Price = 109m,
                        Description = "Hot steamed milk with rich pistachio and Auro's 100% Natural Cacao Powder.",
                        CanBeHot = true,
                        CanBeCold = false
                    },
                    new Menu
                    {
                        Name = "Hot Pistachio Latte",
                        Category = "Pistachio",
                        Price = 119m,
                        Description = "Hot steamed milk with rich pistachio and bold espresso.",
                        CanBeHot = true,
                        CanBeCold = false
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
