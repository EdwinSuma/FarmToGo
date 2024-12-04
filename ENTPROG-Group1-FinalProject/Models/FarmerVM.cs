using Farmers.DataModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class FarmerVM
    {
        [Key]
        public int FarmerId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        public string ApprovalStatus { get; set; } = "Pending"; // Pending, Approved, Rejected

        // Navigation Properties
        public ICollection<Product> Products { get; set; }

        public ICollection<Invoice> Invoices { get; set; } // One-to-Many with Invoice

    }
}
