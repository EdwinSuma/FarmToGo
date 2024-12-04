using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Farmers.DataModel
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        [Required]
        public int FarmerId { get; set; }

        [ForeignKey("FarmerId")]
        public Farmer Farmer { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public ProductCategory Category { get; set; }

        // Navigation Properties
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
