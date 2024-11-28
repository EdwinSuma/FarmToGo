using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class ProductVM
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SellerId { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Product Name must not exceed 30 characters.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }
    }
}
