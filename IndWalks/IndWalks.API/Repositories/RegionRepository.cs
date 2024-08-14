using IndWalks.API.Data;
using IndWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly IndWalkDbContext indWalkdbContext;

        public RegionRepository(IndWalkDbContext indWalkdbContext)
        {
            this.indWalkdbContext = indWalkdbContext;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await indWalkdbContext.Regions.ToListAsync();
        }
    }
}
