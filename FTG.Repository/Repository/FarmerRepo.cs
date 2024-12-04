using Farmers.DataModel;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FTG.Repository.Repository
{
    public class FarmerRepo : GenericRepo<Farmer>, IFarmerRepo
    {
        public FarmerRepo(AppDbContext context) : base(context) { }

        public async Task<Farmer> GetByUserIdAsync(string userId)
        {
            return await _context.Farmers
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.UserId == userId);
        }
    }
}
