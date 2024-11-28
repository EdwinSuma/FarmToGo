using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmers.DataModel
{
    public class Courier
    {
        [Key]
        public int CourierId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string ContactNumber { get; set; }

        public DateTime DateAdded { get; set; } = DateTime.Now;

        // Navigation Properties
        public ICollection<Shipping> Shipments { get; set; }
    }
}
