using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class ProductVM
    {
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be at least 1")]
        public int Stock { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int FarmerId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        // For display purposes
        public string CategoryName { get; set; }
        public string FarmerName { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
