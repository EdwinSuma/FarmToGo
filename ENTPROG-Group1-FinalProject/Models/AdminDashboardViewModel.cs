using System.Collections.Generic;

namespace Farmers.App.Models
{
    public class AdminDashboardViewModel
    {
        public IEnumerable<ApplicationUser> PendingFarmers { get; set; }
        public IEnumerable<ApplicationUser> ActiveCouriers { get; set; }
        public IEnumerable<ApplicationUser> ActiveFarmers { get; set; }
        public IEnumerable<ApplicationUser> ActiveCustomers { get; set; }
    }
}
