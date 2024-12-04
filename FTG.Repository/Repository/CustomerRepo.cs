using Farmers.DataModel;

namespace FTG.Repository.Repository
{
    public class CustomerRepo : GenericRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id); 
        }

    }
}
