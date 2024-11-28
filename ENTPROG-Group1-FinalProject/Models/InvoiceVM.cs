using System.ComponentModel.DataAnnotations;

namespace Farmers.App.Models
{
    public class InvoiceVM
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Invoice Number must not exceed 20 characters")]
        [Display(Name = "Invoice Number")]
        public string InvoiceNumber { get; set; }
    }
}
