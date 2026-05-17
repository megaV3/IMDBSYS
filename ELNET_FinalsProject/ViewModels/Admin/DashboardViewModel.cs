using IMDBSYS.Models;

namespace IMDBSYS.ViewModels.Admin
{
    public class DashboardViewModel
    {
        // 1. Statistical Summary Cards (View Totals)
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; } // Total unique Menu items
        public int TotalOrders { get; set; }
        public decimal TotalSalesRevenue { get; set; } // Sum of Completed Order TotalAmounts
        public decimal TotalTopUpAmount { get; set; }  // Sum of Approved/Completed TopUps

        // 2. Inventory Monitor Metrics
        public int TotalStockQuantity { get; set; }   // Global sum of all MenuVariation stocks
        public List<MenuVariation> LowStockProducts { get; set; } = new();

        // 3. Recent Activities / Tables
        public List<Order> RecentOrders { get; set; } = new();
        public List<TopUpHistory> RecentTopUps { get; set; } = new();

        public List<ProductDeliveryLog> RecentStockChanges { get; set; } = new();
    }
}
