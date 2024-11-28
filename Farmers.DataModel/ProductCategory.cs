using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Farmers.DataModel
{
    public class ProductCategory
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        // Navigation Properties
        public ICollection<Product> Products { get; set; }
    }
}
