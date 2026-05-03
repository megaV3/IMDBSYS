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
                        CanBeCold = true,
                        ImagePath = "/images/signatures/kape kastila.png"
                    },
                    new Menu
                    {
                        Name = "Iced White Mocha Latte",
                        Category = "Signatures",
                        Price = 89m,
                        Description = "White chocolate with our creamy milk blend and rich espresso, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/signatures/iced white mocha latte.png"
                    },
                    new Menu
                    {
                        Name = "Iced Caramel Macchiato",
                        Category = "Signatures",
                        Price = 89m,
                        Description = "Rich espresso with creamy milk and caramel, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/signatures/iced caramel macchiato.png"
                    },
                    new Menu
                    {
                        Name = "Pickup Crème Latte",
                        Category = "Signatures",
                        Price = 95m,
                        Description = "A latte made with rich espresso and caramelized custard crème, topped with a Crème Brûlée foam. Proudly our crème de la crème!",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/signatures/pickup creme latte.png"
                    },
                    new Menu
                    {
                        Name = "Cappuccino",
                        Category = "Espresso-based",
                        Price = 75m,
                        Description = "Rich espresso with hot steamed milk and foam.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/espresso-based/cappuccino.png"
                    },
                    new Menu
                    {
                        Name = "Iced Brown Sugar Latter",
                        Category = "Espresso-based",
                        Price = 79m,
                        Description = "Rich espresso with creamy milk and brown molasses syrup, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/iced brown sugar latte.png"
                    },
                    new Menu
                    {
                        Name = "Sea Salt Biscoff Latte",
                        Category = "Espresso-based",
                        Price = 115m,
                        Description = "Rich espresso with creamy milk, Biscoff cookie bits, topped with sea salt cream mousse.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/sea salt biscoff latte.png"
                    },
                    new Menu
                    {
                        Name = "Iced Sea Salt Latte",
                        Category = "Espresso-based",
                        Price = 89m,
                        Description = "Rich espresso with creamy milk, topped with sea salt milk foam.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/iced sea salt latte.png"
                    },
                    new Menu
                    {
                        Name = "Vietnamese Latte",
                        Category = "Espresso-based",
                        Price = 79m,
                        Description = "A strong blend of rich espresso with leche condensada.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/vietnamese latte.png"
                    },
                    new Menu
                    {
                        Name = "Iced Vanilla Latte",
                        Category = "Espresso-based",
                        Price = 85m,
                        Description = "A velvety blend of creamy vanilla with milk and rich espresso, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/iced vanilla latte.png"
                    },
                    new Menu
                    {
                        Name = "Americano",
                        Category = "Espresso-based",
                        Price = 50m,
                        Description = "Rich espresso with water. Also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/americano.png"
                    },
                    new Menu
                    {
                        Name = "Iced Dark Chocolate Latte",
                        Category = "Espresso-based",
                        Price = 85m,
                        Description = "Dark chocolate with creamy milk and rich espresso, also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/iced dark chocolate latte.png"
                    },
                    new Menu
                    {
                        Name = "Flat White",
                        Category = "Espresso-based",
                        Price = 75m,
                        Description = "Rich espresso with hot steamed milk.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/espresso-based/flat white.png"
                    },
                    new Menu
                    {
                        Name = "Iced Latte",
                        Category = "Espresso-based",
                        Price = 75m,
                        Description = "Rich espresso with creamy milk. Also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/iced latte.png"
                    },
                    new Menu
                    {
                        Name = "Espresso",
                        Category = "Espresso-based",
                        Price = 30m,
                        Description = "A double shot of rich espresso.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/espresso-based/espresso.png"
                    },
                    new Menu
                    {
                        Name = "Iced Hazelnut Latte",
                        Category = "Espresso-based",
                        Price = 95m,
                        Description = "Rich espresso with creamy milk and Nutella. Also available hot.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/espresso-based/iced hazelnut latte.png"
                    },
                    new Menu
                    {
                        Name = "Sea Salt Pistachio Milk",
                        Category = "Pistachio",
                        Price = 119m,
                        Description = "Creamy milk with rich pistachio, topped with sea salt milk foam.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/pistachio/sea salt pistachio milk.png"
                    },
                    new Menu
                    {
                        Name = "Hot Auro Chocolate Pistachio Latte",
                        Category = "Pistachio",
                        Price = 129m,
                        Description = "Hot steamed milk with rich pistachio, Auro’s 100% Natural Cacao Powder, and bold espresso.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/pistachio/hot auro chocolate pistachio latte.png"
                    },
                    new Menu
                    {
                        Name = "Hot Auro Chocolate Pistachio Milk",
                        Category = "Pistachio",
                        Price = 109m,
                        Description = "Hot steamed milk with rich pistachio and Auro's 100% Natural Cacao Powder.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/pistachio/hot auro chocolate pistachio milk.png"
                    },
                    new Menu
                    {
                        Name = "Hot Pistachio Latte",
                        Category = "Pistachio",
                        Price = 119m,
                        Description = "Hot steamed milk with rich pistachio and bold espresso.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/pistachio/hot pistachio latte.png"
                    }
                    /*
                    new Menu
                    {
                        Name = " ",
                        Category = "",
                        Price = m,
                        Description = "",
                        CanBeHot = ,
                        CanBeCold = ,
                        ImagePath = "/images/"
                    },
                    */
                );
                context.SaveChanges();
            }
        }
    }
}
