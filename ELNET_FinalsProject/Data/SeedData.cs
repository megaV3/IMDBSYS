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
                        Name = "Kingston KC600 SSD Sata 3",
                        Category = "StorageDevices",
                        Description = "Boost your PC's speed and reliability with this 512GB SATA 3 SSD, designed for lightning-fast boot times and seamless multitasking.",
                        ImagePath = "/images/storageDevices/Kingston 512G SSD Sata 3.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "256GB",
                                Price = 2500m,
                                StockQuantity = 100
                            },
                            new MenuVariation
                            {
                                VariantName = "512GB",
                                Price = 3000m, // 2500 base + 500 variation cost from your UI
                                StockQuantity = 40,
                            },
                            new MenuVariation
                            {
                                VariantName = "1TB",
                                Price = 4000m, // 2500 base + 1500 variation cost
                                StockQuantity = 48,
                            },
                            new MenuVariation
                            {
                                VariantName = "2TB",
                                Price = 6000m, // 2500 base + 3500 variation cost
                                StockQuantity = 50,
                            }
                        }

                    },
                    new Menu
                    {
                        Name = "Samsung 990 PRO NVMe SSD",
                        Category = "StorageDevices",
                        Description = "Experience peak performance with the Samsung 990 PRO, a PCIe 4.0 NVMe SSD offering unrivaled speeds and power efficiency for elite gaming and intensive creative workflows.",
                        ImagePath = "/images/storageDevices/Samsung 990 PRO NVMe SSD.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "1TB",
                                Price = 10500m,
                                StockQuantity = 30,
                            },
                            new MenuVariation
                            {
                                VariantName = "2TB",
                                Price = 17000m,
                                StockQuantity = 25,
                            },
                            new MenuVariation
                            {
                                VariantName = "4TB",
                                Price = 31500m,
                                StockQuantity = 17,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Seagate SkyHawk HDD",
                        Category = "StorageDevices",
                        Description = "Optimized for 24/7 surveillance, this 2TB Seagate SkyHawk HDD provides reliable, high-capacity storage for multi-bay DVR and NVR systems with ImagePerfect™ firmware to ensure smooth video streaming.",
                        ImagePath = "/images/storageDevices/Seagate SkyHawk 2TB HDD.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "1TB",
                                Price = 4500m,
                                StockQuantity = 30,
                            },
                            new MenuVariation
                            {
                                VariantName = "2TB",
                                Price = 7500m,
                                StockQuantity = 25,
                            },
                            new MenuVariation
                            {
                                VariantName = "4TB",
                                Price = 13500m,
                                StockQuantity = 17,
                            },
                            new MenuVariation
                            {
                                VariantName = "8TB",
                                Price = 19000m,
                                StockQuantity = 17,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "WD Blue HDD",
                        Category = "StorageDevices",
                        Price = 699m,
                        Description = "The WD Blue 1TB HDD is a dependable, high-capacity hard drive designed for reliable everyday performance and bulk storage in desktop PCs and external enclosures.",
                        ImagePath = "/images/storageDevices/WD Blue 1TB HDD.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "1TB",
                                Price = 2100m,
                                StockQuantity = 70,
                            },
                            new MenuVariation
                            {
                                VariantName = "2TB",
                                Price = 3500m,
                                StockQuantity = 60,
                            },
                            new MenuVariation
                            {
                                VariantName = "4TB",
                                Price = 6500m,
                                StockQuantity = 30,
                            },
                            new MenuVariation
                            {
                                VariantName = "8TB",
                                Price = 11500m,
                                StockQuantity = 17,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Crucial DDR4",
                        Category = "RAM",
                        Description = "This 4GB 2666MHz DDR4 memory module provides a fast, efficient performance boost for multitasking and system responsiveness in modern computing environments.",
                        ImagePath = "/images/RAM/Crucial 4GB 2666MHz DDR4.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "4GB 2666MHz",
                                Price = 3000m,
                                StockQuantity = 50,
                            },
                            new MenuVariation
                            {
                                VariantName = "8GB 2666MHz",
                                Price = 5800m,
                                StockQuantity = 60,
                            },
                            new MenuVariation
                            {
                                VariantName = "16GB 2666MHz",
                                Price = 9500m,
                                StockQuantity = 30,
                            },
                            new MenuVariation
                            {
                                VariantName = "16GB 3200MHz",
                                Price = 10000m,
                                StockQuantity = 17,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Kingston HyperX Fury DDR4",
                        Category = "RAM",
                        Description = "Featuring plug-and-play overclocking and a sleek heat spreader, this high-performance 16GB 3200MHz DDR4 RAM module delivers boosted gaming speeds and enhanced system reliability.",
                        ImagePath = "/images/RAM/Kingston HyperX Fury 16GB 3200MHz DDR4.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "4GB 2666MHz",
                                Price = 1439m,
                                StockQuantity = 200,
                            },
                            new MenuVariation
                            {
                                VariantName = "8GB 2666MHz",
                                Price = 2856m,
                                StockQuantity = 150,
                            },
                            new MenuVariation
                            {
                                VariantName = "16GB 2666MHz",
                                Price = 5199m,
                                StockQuantity = 75,
                            },
                            new MenuVariation
                            {
                                VariantName = "16GB 3200MHz",
                                Price = 5299m,
                                StockQuantity = 40,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Samsung DDR4",
                        Category = "RAM",
                        Description = "This high-performance 16GB 3200MHz DDR4 memory module delivers reliable speed and efficient multitasking capabilities to optimize stability for modern desktop and laptop systems.",
                        ImagePath = "/images/RAM/Samsung 16GB 3200Hz DDR4.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "4GB 2666MHz",
                                Price = 1487m,
                                StockQuantity = 300,
                            },
                            new MenuVariation
                            {
                                VariantName = "8GB 2666MHz",
                                Price = 2786m,
                                StockQuantity = 215,
                            },
                            new MenuVariation
                            {
                                VariantName = "16GB 2666MHz",
                                Price = 5384m,
                                StockQuantity = 127,
                            },
                            new MenuVariation
                            {
                                VariantName = "16GB 3200MHz",
                                Price = 6106m,
                                StockQuantity = 66,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Corsair CV650 Bronze ATX",
                        Category = "PSU",
                        Description = "The Corsair CV650 Bronze ATX power supply delivers steady, reliable power with 80 PLUS Bronze efficiency, making it a solid choice for budget-friendly PC builds and home office systems.",
                        ImagePath = "/images/PSU/Corsair CV650 Bronze ATX.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 3650m,
                                StockQuantity = 400,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Corsair CX550 Bronze ATX",
                        Category = "PSU",
                        Description = "The Corsair CX550 Bronze ATX power supply provides 550 watts of reliable power with 80 PLUS Bronze efficiency, offering a quiet and dependable solution for entry-level and mid-range PC builds.",
                        ImagePath = "/images/PSU/Corsair CX550 Bronze ATX.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 3100m,
                                StockQuantity = 250,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Corsair CX750 Bronze ATX",
                        Category = "PSU",
                        Description = "The Corsair CX750 Bronze ATX power supply unit offers 750 watts of consistent power with 80 PLUS Bronze efficiency, ensuring a reliable and quiet performance for high-demand PC builds.",
                        ImagePath = "/images/PSU/Corsair CX750 Bronze ATX.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 4550m,
                                StockQuantity = 178,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Corsair CX750m Bronze m-ATX",
                        Category = "PSU",
                        Description = "The Corsair CX750m Bronze m-ATX power supply combines a compact form factor with semi-modular cables for easier cable management, delivering 750 watts of reliable 80 PLUS Bronze power to your system.",
                        ImagePath = "/images/PSU/Corsair CX750m Bronze m-ATX.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 4000m,
                                StockQuantity = 198,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Accuratus 201 keyboard",
                        Category = "inputDevices",
                        Description = "The Accuratus 201 keyboard features a high-contrast design with large print keys, offering enhanced visibility and a durable 1.8-meter cable for reliable desktop connectivity.",
                        ImagePath = "/images/inputDevices/Accuratus 201 keyboard 1.8k.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 1890m,
                                StockQuantity = 467,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Dell KB813",
                        Category = "inputDevices",
                        Price = 1450m,
                        Description = "The Dell KB813 keyboard features an integrated smart card reader for secure data access and a spill-resistant design, providing a comfortable and reliable typing experience for professional environments.",
                        ImagePath = "/images/inputDevices/Dell KB813 1K.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 1450m,
                                StockQuantity = 765,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Dell MS116 Optical Mouse",
                        Category = "inputDevices",
                        Description = "The Dell MS116 Optical Mouse features high-precision LED tracking and a comfortable, ergonomic design, providing reliable performance for everyday office tasks and home use.",
                        ImagePath = "/images/inputDevices/Dell MS116 Optical Mouse.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 545m,
                                StockQuantity = 250
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Logictech m185 Wireless Mouse",
                        Category = "inputDevices",
                        Description = "The Logitech M185 Wireless Mouse provides a plug-and-forget nano receiver and reliable 2.4 GHz connectivity, ensuring a clutter-free workspace with a comfortable, contoured design for both left and right-handed users.",
                        ImagePath = "/images/inputDevices/Logictech m185 Wireless Mouse.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 999m,
                                StockQuantity = 356
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Logitech keyboard",
                        Category = "inputDevices",
                        Description = "The Logitech K120 keyboard features a low-profile, spill-resistant design and a plug-and-play USB connection, offering a durable and quiet typing experience for any standard workspace.",
                        ImagePath = "/images/inputDevices/Logitech keyboard 1k.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 1350m,
                                StockQuantity = 700
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Vention KTBBO Wireless Mouse",
                        Category = "inputDevices",
                        Description = "The Vention KTBBO Wireless Mouse offers a slim, ergonomic profile with silent clicking and adjustable DPI settings, providing a smooth and quiet navigation experience for both office and travel use.",
                        ImagePath = "/images/inputDevices/Vention KTBBO Wireless Mouse.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 999m,
                                StockQuantity = 564
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Allan Speaker Mini Portable",
                        Category = "outputDevices",
                        Description = "The Allan Mini Portable Speaker features a compact, two-channel design with 6W output power, offering a budget-friendly audio solution for laptops, PCs, and smartphones via a 3.5mm stereo jack and USB power.",
                        ImagePath = "/images/outputDevices/Allan Speaker Mini Portable.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 500m,
                                StockQuantity = 897
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Kensington Hi-Fi headset",
                        Category = "outputDevices",
                        Description = "The Kensington Hi-Fi Headset features high-quality stereo sound and a generous 1.4-meter cable, providing a comfortable and adjustable fit for students and professionals during long listening sessions or calls.",
                        ImagePath = "/images/outputDevices/Kensington Hi-Fi headset 1.4k.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 760m,
                                StockQuantity = 250,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Logitech Z150 Multimedia Speakers",
                        Category = "outputDevices",
                        Description = "The Logitech Z150 Multimedia Speakers deliver 6 watts of peak power and clear stereo sound, featuring a compact design with a convenient headphone jack and auxiliary input for versatile audio connectivity.",
                        ImagePath = "/images/outputDevices/Logitech Z150 Multimedia Speakers.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 850m,
                                StockQuantity = 333,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "RAPOO H100 3.5mm headset",
                        Category = "outputDevices",
                        Description = "The Rapoo H100 Wired Stereo Headset features a lightweight design and clear audio output, utilizing a 3.5mm jack for easy compatibility with a variety of devices while offering a comfortable fit for daily use.",
                        ImagePath = "/images/outputDevices/RAPOO H100 3.5mm headset 430.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 430m,
                                StockQuantity = 412,
                            }
                        }
                    },
                    new Menu
                    {
                        Name = "Wired Headset Stereo 100",
                        Category = "outputDevices",
                        Description = "The Wired Headset Stereo 100 is a lightweight, entry-level audio solution featuring a plug-and-play 3.5mm connection and a built-in microphone, making it ideal for clear communication and casual media use across various devices.",
                        ImagePath = "/images/outputDevices/Wired Headset Stereo 100.webp",

                        // Seed the variants and their respective inventory data right here
                        Variations = new List<MenuVariation>
                        {
                            new MenuVariation
                            {
                                VariantName = "Standard",
                                Price = 129m,
                                StockQuantity = 1234,
                            }
                        }
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
