using Farmers.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public interface ICategoryRepo : IGenericRepo<ProductCategory>
    {
        Task<List<ProductCategory>> GetAllForDropdownAsync();
    }
}
