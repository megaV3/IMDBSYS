using System.ComponentModel.DataAnnotations;

namespace ELNET_FinalsProject.ViewModels
{
    public class TopUpViewModel
    {
        [Required]
        [Range(1, 10000)]
        public decimal Amount { get; set; }
        // You can add PaymentMethod, etc. here
    }
}
