using Farmers.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public interface IProductRepo : IGenericRepo<Product>
    {
        Task<List<Product>> GetByFarmerIdAsync(int farmerId);
        Task<Product> GetProductByIdAsync(int productId);
        Task<List<Product>> GetAvailableProductsAsync();
    }
}
