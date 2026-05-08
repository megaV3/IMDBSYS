using System.ComponentModel.DataAnnotations;

namespace ELNET_FinalsProject.Models
{
    public class TopUpHistory
    {
        [Key]
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Status { get; set; }
    }
}
