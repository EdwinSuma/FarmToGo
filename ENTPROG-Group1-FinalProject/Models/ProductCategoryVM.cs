using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class ProductCategoryVM
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Category name must not exceed 20 characters.")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
    }
}
