using ELNET_FinalsProject.Models;

namespace ELNET_FinalsProject.ViewModels
{
    public class StoreViewModel
    {
        public User UserProfile { get; set; } = new User();
        public List<Menu> Menus { get; set; } = new List<Menu>();
    }
}
