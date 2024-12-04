using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Farmers.DataModel
{
    public class Farmer
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
