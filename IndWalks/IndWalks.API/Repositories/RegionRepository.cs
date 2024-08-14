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

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await indWalkdbContext.AddAsync(region);
            await indWalkdbContext.SaveChangesAsync();
            return region;

        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await indWalkdbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null)
            {
                return null;
            }

            indWalkdbContext.Regions.Remove(region);
            await indWalkdbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await indWalkdbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await indWalkdbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingregion = await indWalkdbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);

            if (existingregion == null)
            {
                return null;
            }

            existingregion.Code= region.Code;
            existingregion.Name= region.Name;
            existingregion.Lat= region.Lat;
            existingregion.Long = region.Long;
            existingregion.Area = region.Area;
            existingregion.Population = region.Population;

            await indWalkdbContext.SaveChangesAsync();
            return existingregion;
        }
    }
}
