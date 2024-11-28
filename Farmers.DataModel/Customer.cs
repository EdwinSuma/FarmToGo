using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmers.DataModel
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; set; }

        public ICollection<Invoice> Invoices { get; set; } // One-to-Many with Invoice
    }
}
