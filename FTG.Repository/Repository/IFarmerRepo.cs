using Farmers.DataModel;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public interface IFarmerRepo : IGenericRepo<Farmer>
    {
        Task<Farmer> GetByUserIdAsync(string userId);
    }
}
