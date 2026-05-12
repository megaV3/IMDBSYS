using System.ComponentModel.DataAnnotations;

namespace ELNET_FinalsProject.ViewModels
{
    public class TopUpViewModel
    {
        [Required(ErrorMessage = "Please enter an amount.")]
        [Range(1, 10000, ErrorMessage = "You can only top up between ₱1 and ₱10,000.")]
        public decimal? Amount { get; set; }
        // You can add PaymentMethod, etc. here
        public string Email { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; }
    }
}
