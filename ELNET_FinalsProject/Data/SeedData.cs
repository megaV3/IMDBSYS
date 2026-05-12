using IMDBSYS.Models;
using Microsoft.EntityFrameworkCore;

namespace IMDBSYS.Data
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
                        Name = "Biscoff Frappe",
                        Category = "Frappes",
                        Price = 110m,
                        Description = "A smooth and creamy iced frappe blended with Biscoff, delivering a rich caramel flavor with a subtle spiced finish.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/biscoff.png"
                    },
                    new Menu
                    {
                        Name = "Strawberry Milkshake",
                        Category = "Milk-Based",
                        Price = 95m,
                        Description = "A creamy, refreshing blend of milk and sweet strawberries, offering a smooth and fruity treat in every sip.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/strawberry.png"
                    },
                    new Menu
                    {
                        Name = "Cocoa Fudge Frappe",
                        Category = "Frappes",
                        Price = 115m,
                        Description = "A rich, chocolatey frappe with smooth cocoa and fudgy sweetness.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/chocolate.png"
                    },
                    new Menu
                    {
                        Name = "Cold Matcha Frappe",
                        Category = "Frappes",
                        Price = 85m,
                        Description = "A creamy matcha frappe topped with light, airy cloud foam.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/matcha.png"
                    },
                    new Menu
                    {
                        Name = "Salted Caramel Frappe",
                        Category = "Frappes",
                        Price = 79m,
                        Description = "A rich caramel frappe with a smooth, lightly salted finish.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/caramel.png"
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
                    },
                     new Menu
                    {
                        Name = "White Mocha Frappe",
                        Category = "Frappes",
                        Price = 119m,
                        Description = "Rich espresso and white chocolate, ice-blended with creamy milk, topped with whipped cream.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/white mocha frappe.png"
                    },
                    new Menu
                    {
                        Name = "Matcha Frappe",
                        Category = "Frappes",
                        Price = 115m,
                        Description = "Pure matcha, ice-blended with creamy milk, topped with whipped cream.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/matcha frappe.png"
                    },
                    new Menu
                    {
                        Name = "Tripple Choco Chip Frappe",
                        Category = "Frappes",
                        Price = 115m,
                        Description = "Dark chocolate, ice-blended with milk chocolate chips and creamy milk, topped with whipped cream.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/tripple choco chip frappe.png"
                    },
                    new Menu
                    {
                        Name = "Oreo Frappe",
                        Category = "Frappes",
                        Price = 115m,
                        Description = "Oreo, ice-blended with creamy milk, topped with whipped cream.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/oreo frappe.png"
                    },
                    new Menu
                    {
                        Name = "Kape Kastila Frappe",
                        Category = "Frappes",
                        Price = 99m,
                        Description = "Rich espresso and leche condensada, ice-blended with creamy milk, topped with whipped cream. Our take on a Spanish Latte frappe.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/kape kastila frappe.png"
                    },
                    new Menu
                    {
                        Name = "Cafe Mocha Chip Frappe",
                        Category = "Frappes",
                        Price = 115m,
                        Description = "Rich espresso and dark chocolate, ice-blended with milk chocolate chips and creamy milk, topped with whipped cream.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/cafe mocha chip frappe.png"
                    },
                    new Menu
                    {
                        Name = "Brown Sugar Coffee Jelly Frappe",
                        Category = "Frappes",
                        Price = 129m,
                        Description = "Rich espresso and brown molasses syrup, ice-blended with creamy milk, with a base of coffee jelly, topped with whipped cream.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/brown sugar coffee jelly frappe.png"
                    },
                    new Menu
                    {
                        Name = "Caramel Frappe",
                        Category = "Frappes",
                        Price = 99m,
                        Description = "Rich espresso and buttery caramel, ice-blended with creamy milk, topped with whipped cream and caramel drizzle",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/frappes/caramel frappe.png"
                    },
                    new Menu
                    {
                        Name = "Sea Salt Biscoff Milk",
                        Category = "Milk-Based",
                        Price = 99m,
                        Description = "Creamy milk with Biscoff cookie bits, topped with sea salt cream mousse, over ice.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/sea salt biscoff milk.png"
                    },
                    new Menu
                    {
                        Name = "Classic Milk Tea",
                        Category = "Milk-Based",
                        Price = 70m,
                        Description = "Creamy milk with black tea and boba pearls. Our take on a classic Milk Tea.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/classic milk tea.png"
                    },
                    new Menu
                    {
                        Name = "Brown Sugar Boba Milk",
                        Category = "Milk-Based",
                        Price = 75m,
                        Description = "Creamy milk, brown sugar and boba pearls, topped with cream mousse.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/brown sugar boba milk.png"
                    },
                    new Menu
                    {
                        Name = "Milosaurus",
                        Category = "Milk-Based",
                        Price = 79m,
                        Description = "Milo with creamy milk and cream mousse. Our take on the classic Milo Dinosaur.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/milosaurus.png"
                    },
                    new Menu
                    {
                        Name = "Classic Chocolate Milk",
                        Category = "Milk-Based",
                        Price = 75m,
                        Description = "Creamy milk with rich chocolate, also available hot.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/classic chocolate milk.png"
                    },
                    new Menu
                    {
                        Name = "Iced Ube Milk",
                        Category = "Milk-Based",
                        Price = 75m,
                        Description = "Creamy milk with rich ube. A special bevarage using a Filipino favorite, also available hot.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/milkbased/iced ube milk.png"
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
