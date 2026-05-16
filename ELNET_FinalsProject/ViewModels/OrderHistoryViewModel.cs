using IMDBSYS.Models;

namespace IMDBSYS.ViewModels
{
    public class OrderHistoryViewModel
    {

        public string? ProfileImagePath { get; set; }
        public int CartCount { get; set; }
        public List<Order> OrderHistory { get; set; }
        public decimal UserBalance { get; set; }
    }
}
