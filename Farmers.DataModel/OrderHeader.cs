using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Farmers.DataModel
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        public string OrderStatus { get; set; } = "Pending"; // Pending, Completed, Cancelled

        // Navigation Properties
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Shipping Shipping { get; set; }

        public Invoice Invoice { get; set; } // One-to-One with Invoice
    }
}
