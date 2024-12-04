using Farmers.DataModel;

namespace FTG.Repository.Repository
{
    public interface ICustomerRepo : IGenericRepo<Customer>
    {
        Task<Customer> GetByIdAsync(int id);
    }
}
