using ELNET_FinalsProject.Models;

namespace ELNET_FinalsProject.ViewModels
{
    public class OrderHistoryViewModel
    {

        public string? ProfileImagePath { get; set; }
        public int CartCount { get; set; }
        public List<Order> OrderHistory { get; set; }
    }
}
