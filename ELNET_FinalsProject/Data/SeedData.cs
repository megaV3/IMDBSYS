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
                        Name = "Kingston 512 SSD Sata 3",
                        Category = "StorageDevices",
                        Price = 75m,
                        Description = "Boost your PC's speed and reliability with this 512GB SATA 3 SSD, designed for lightning-fast boot times and seamless multitasking.",
                        CanBeHot = true,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/storageDevices/Kingston 512G SSD Sata 3.webp"
                    },
                    new Menu
                    {
                        Name = "Samsung 990 PRO NVMe SSD",
                        Category = "StorageDevices",
                        Price = 89m,
                        Description = "Experience peak performance with the Samsung 990 PRO, a PCIe 4.0 NVMe SSD offering unrivaled speeds and power efficiency for elite gaming and intensive creative workflows.",
                        CanBeHot = true,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/storageDevices/Samsung 990 PRO NVMe SSD.webp"
                    },
                    new Menu
                    {
                        Name = "Seagate SkyHawk 2TB HDD",
                        Category = "StorageDevices",
                        Price = 89m,
                        Description = "Optimized for 24/7 surveillance, this 2TB Seagate SkyHawk HDD provides reliable, high-capacity storage for multi-bay DVR and NVR systems with ImagePerfect™ firmware to ensure smooth video streaming.",
                        CanBeHot = true,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/storageDevices/Seagate SkyHawk 2TB HDD.webp"
                    },
                    new Menu
                    {
                        Name = "WD Blue 1TB HDD",
                        Category = "StorageDevices",
                        Price = 95m,
                        Description = "The WD Blue 1TB HDD is a dependable, high-capacity hard drive designed for reliable everyday performance and bulk storage in desktop PCs and external enclosures.",
                        CanBeHot = false,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/storageDevices/WD Blue 1TB HDD.webp"
                    },
                    //new Menu
                    //{
                    //    Name = "Biscoff Frappe",
                    //    Category = "Frappes",
                    //    Price = 110m,
                    //    Description = "A smooth and creamy iced frappe blended with Biscoff, delivering a rich caramel flavor with a subtle spiced finish.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/biscoff.png"
                    //},
                    new Menu
                    {
                        Name = "Crucial 4GB 2666MHz DDR4",
                        Category = "RAM",
                        Price = 95m,
                        Description = "This 4GB 2666MHz DDR4 memory module provides a fast, efficient performance boost for multitasking and system responsiveness in modern computing environments.",
                        CanBeHot = false,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/RAM/Crucial 4GB 2666MHz DDR4.webp"
                    },
                    new Menu
                    {
                        Name = "Kingston HyperX Fury 16GB 3200MHz DDR4",
                        Category = "RAM",
                        Price = 115m,
                        Description = "Featuring plug-and-play overclocking and a sleek heat spreader, this high-performance 16GB 3200MHz DDR4 RAM module delivers boosted gaming speeds and enhanced system reliability.",
                        CanBeHot = false,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/RAM/Kingston HyperX Fury 16GB 3200MHz DDR4.webp"
                    },
                    new Menu
                    {
                        Name = "Samsung 16GB 3200Hz DDR4",
                        Category = "RAM",
                        Price = 85m,
                        Description = "This high-performance 16GB 3200MHz DDR4 memory module delivers reliable speed and efficient multitasking capabilities to optimize stability for modern desktop and laptop systems.",
                        CanBeHot = false,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/RAM/Samsung 16GB 3200Hz DDR4.webp"
                    },
                    new Menu
                    {
                        Name = "Corsair CV650 Bronze ATX",
                        Category = "PSU",
                        Price = 79m,
                        Description = "The Corsair CV650 Bronze ATX power supply delivers steady, reliable power with 80 PLUS Bronze efficiency, making it a solid choice for budget-friendly PC builds and home office systems.",
                        CanBeHot = false,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/PSU/Corsair CV650 Bronze ATX.webp"
                    },
                    new Menu
                    {
                        Name = "Corsair CX550 Bronze ATX",
                        Category = "PSU",
                        Price = 75m,
                        Description = "The Corsair CX550 Bronze ATX power supply provides 550 watts of reliable power with 80 PLUS Bronze efficiency, offering a quiet and dependable solution for entry-level and mid-range PC builds.",
                        CanBeHot = true,
                        CanBeCold = false,
                        HasVariation = true,
                        ImagePath = "/images/PSU/Corsair CX550 Bronze ATX.webp"
                    },
                    new Menu
                    {
                        Name = "Corsair CX750 Bronze ATX",
                        Category = "PSU",
                        Price = 79m,
                        Description = "The Corsair CX750 Bronze ATX power supply unit offers 750 watts of consistent power with 80 PLUS Bronze efficiency, ensuring a reliable and quiet performance for high-demand PC builds.",
                        CanBeHot = true,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/PSU/Corsair CX750 Bronze ATX.webp"
                    },
                    new Menu
                    {
                        Name = "Corsair CX750m Bronze m-ATX",
                        Category = "PSU",
                        Price = 115m,
                        Description = "The Corsair CX750m Bronze m-ATX power supply combines a compact form factor with semi-modular cables for easier cable management, delivering 750 watts of reliable 80 PLUS Bronze power to your system.",
                        CanBeHot = false,
                        CanBeCold = true,
                        HasVariation = true,
                        ImagePath = "/images/PSU/Corsair CX750m Bronze m-ATX.webp"
                    },
                    new Menu
                    {
                        Name = "Accuratus 201 keyboard 1.8k",
                        Category = "inputDevices",
                        Price = 89m,
                        Description = "The Accuratus 201 keyboard features a high-contrast design with large print keys, offering enhanced visibility and a durable 1.8-meter cable for reliable desktop connectivity.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/inputDevices/Accuratus 201 keyboard 1.8k.webp"
                    },
                    new Menu
                    {
                        Name = "Dell KB813 1K",
                        Category = "inputDevices",
                        Price = 79m,
                        Description = "The Dell KB813 keyboard features an integrated smart card reader for secure data access and a spill-resistant design, providing a comfortable and reliable typing experience for professional environments.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/inputDevices/Dell KB813 1K.webp"
                    },
                    new Menu
                    {
                        Name = "Dell MS116 Optical Mouse",
                        Category = "inputDevices",
                        Price = 85m,
                        Description = "The Dell MS116 Optical Mouse features high-precision LED tracking and a comfortable, ergonomic design, providing reliable performance for everyday office tasks and home use.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/inputDevices/Dell MS116 Optical Mouse.webp"
                    },
                    new Menu
                    {
                        Name = "Logictech m185 Wireless Mouse",
                        Category = "inputDevices",
                        Price = 50m,
                        Description = "The Logitech M185 Wireless Mouse provides a plug-and-forget nano receiver and reliable 2.4 GHz connectivity, ensuring a clutter-free workspace with a comfortable, contoured design for both left and right-handed users.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/inputDevices/Logictech m185 Wireless Mouse.webp"
                    },
                    new Menu
                    {
                        Name = "Logitech keyboard 1k",
                        Category = "inputDevices",
                        Price = 85m,
                        Description = "The Logitech K120 keyboard features a low-profile, spill-resistant design and a plug-and-play USB connection, offering a durable and quiet typing experience for any standard workspace.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/inputDevices/Logitech keyboard 1k.webp"
                    },
                    new Menu
                    {
                        Name = "Vention KTBBO Wireless Mouse",
                        Category = "inputDevices",
                        Price = 75m,
                        Description = "The Vention KTBBO Wireless Mouse offers a slim, ergonomic profile with silent clicking and adjustable DPI settings, providing a smooth and quiet navigation experience for both office and travel use.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/inputDevices/Vention KTBBO Wireless Mouse.webp"
                    },
                    new Menu
                    {
                        Name = "Allan Speaker Mini Portable",
                        Category = "outputDevices",
                        Price = 75m,
                        Description = "The Allan Mini Portable Speaker features a compact, two-channel design with 6W output power, offering a budget-friendly audio solution for laptops, PCs, and smartphones via a 3.5mm stereo jack and USB power.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/outputDevices/Allan Speaker Mini Portable.webp"
                    },
                    new Menu
                    {
                        Name = "Kensington Hi-Fi headset 1.4",
                        Category = "outputDevices",
                        Price = 30m,
                        Description = "The Kensington Hi-Fi Headset features high-quality stereo sound and a generous 1.4-meter cable, providing a comfortable and adjustable fit for students and professionals during long listening sessions or calls.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/outputDevices/Kensington Hi-Fi headset 1.4k.webp"
                    },
                    new Menu
                    {
                        Name = "Logitech Z150 Multimedia Speakers",
                        Category = "outputDevices",
                        Price = 95m,
                        Description = "The Logitech Z150 Multimedia Speakers deliver 6 watts of peak power and clear stereo sound, featuring a compact design with a convenient headphone jack and auxiliary input for versatile audio connectivity.",
                        CanBeHot = true,
                        CanBeCold = true,
                        ImagePath = "/images/outputDevices/Logitech Z150 Multimedia Speakers.webp"
                    },
                    new Menu
                    {
                        Name = "RAPOO H100 3.5mm headset 430",
                        Category = "outputDevices",
                        Price = 119m,
                        Description = "The Rapoo H100 Wired Stereo Headset features a lightweight design and clear audio output, utilizing a 3.5mm jack for easy compatibility with a variety of devices while offering a comfortable fit for daily use.",
                        CanBeHot = false,
                        CanBeCold = true,
                        ImagePath = "/images/outputDevices/RAPOO H100 3.5mm headset 430.webp"
                    },
                    new Menu
                    {
                        Name = "Wired Headset Stereo 100",
                        Category = "outputDevices",
                        Price = 129m,
                        Description = "The Wired Headset Stereo 100 is a lightweight, entry-level audio solution featuring a plug-and-play 3.5mm connection and a built-in microphone, making it ideal for clear communication and casual media use across various devices.",
                        CanBeHot = true,
                        CanBeCold = false,
                        ImagePath = "/images/outputDevices/Wired Headset Stereo 100.webp"
                    }//,
                    //new Menu
                    //{
                    //    Name = "Hot Auro Chocolate Pistachio Milk",
                    //    Category = "Pistachio",
                    //    Price = 109m,
                    //    Description = "Hot steamed milk with rich pistachio and Auro's 100% Natural Cacao Powder.",
                    //    CanBeHot = true,
                    //    CanBeCold = false,
                    //    ImagePath = "/images/pistachio/hot auro chocolate pistachio milk.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Hot Pistachio Latte",
                    //    Category = "Pistachio",
                    //    Price = 119m,
                    //    Description = "Hot steamed milk with rich pistachio and bold espresso.",
                    //    CanBeHot = true,
                    //    CanBeCold = false,
                    //    ImagePath = "/images/pistachio/hot pistachio latte.png"
                    //},
                    // new Menu
                    //{
                    //    Name = "White Mocha Frappe",
                    //    Category = "Frappes",
                    //    Price = 119m,
                    //    Description = "Rich espresso and white chocolate, ice-blended with creamy milk, topped with whipped cream.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/white mocha frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Matcha Frappe",
                    //    Category = "Frappes",
                    //    Price = 115m,
                    //    Description = "Pure matcha, ice-blended with creamy milk, topped with whipped cream.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/matcha frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Tripple Choco Chip Frappe",
                    //    Category = "Frappes",
                    //    Price = 115m,
                    //    Description = "Dark chocolate, ice-blended with milk chocolate chips and creamy milk, topped with whipped cream.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/tripple choco chip frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Oreo Frappe",
                    //    Category = "Frappes",
                    //    Price = 115m,
                    //    Description = "Oreo, ice-blended with creamy milk, topped with whipped cream.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/oreo frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Kape Kastila Frappe",
                    //    Category = "Frappes",
                    //    Price = 99m,
                    //    Description = "Rich espresso and leche condensada, ice-blended with creamy milk, topped with whipped cream. Our take on a Spanish Latte frappe.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/kape kastila frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Cafe Mocha Chip Frappe",
                    //    Category = "Frappes",
                    //    Price = 115m,
                    //    Description = "Rich espresso and dark chocolate, ice-blended with milk chocolate chips and creamy milk, topped with whipped cream.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/cafe mocha chip frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Brown Sugar Coffee Jelly Frappe",
                    //    Category = "Frappes",
                    //    Price = 129m,
                    //    Description = "Rich espresso and brown molasses syrup, ice-blended with creamy milk, with a base of coffee jelly, topped with whipped cream.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/brown sugar coffee jelly frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Caramel Frappe",
                    //    Category = "Frappes",
                    //    Price = 99m,
                    //    Description = "Rich espresso and buttery caramel, ice-blended with creamy milk, topped with whipped cream and caramel drizzle",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/frappes/caramel frappe.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Sea Salt Biscoff Milk",
                    //    Category = "Milk-Based",
                    //    Price = 99m,
                    //    Description = "Creamy milk with Biscoff cookie bits, topped with sea salt cream mousse, over ice.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/milkbased/sea salt biscoff milk.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Classic Milk Tea",
                    //    Category = "Milk-Based",
                    //    Price = 70m,
                    //    Description = "Creamy milk with black tea and boba pearls. Our take on a classic Milk Tea.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/milkbased/classic milk tea.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Brown Sugar Boba Milk",
                    //    Category = "Milk-Based",
                    //    Price = 75m,
                    //    Description = "Creamy milk, brown sugar and boba pearls, topped with cream mousse.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/milkbased/brown sugar boba milk.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Milosaurus",
                    //    Category = "Milk-Based",
                    //    Price = 79m,
                    //    Description = "Milo with creamy milk and cream mousse. Our take on the classic Milo Dinosaur.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/milkbased/milosaurus.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Classic Chocolate Milk",
                    //    Category = "Milk-Based",
                    //    Price = 75m,
                    //    Description = "Creamy milk with rich chocolate, also available hot.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/milkbased/classic chocolate milk.png"
                    //},
                    //new Menu
                    //{
                    //    Name = "Iced Ube Milk",
                    //    Category = "Milk-Based",
                    //    Price = 75m,
                    //    Description = "Creamy milk with rich ube. A special bevarage using a Filipino favorite, also available hot.",
                    //    CanBeHot = false,
                    //    CanBeCold = true,
                    //    ImagePath = "/images/milkbased/iced ube milk.png"
                    //}
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
