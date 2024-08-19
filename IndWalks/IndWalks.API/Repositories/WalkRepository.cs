using IndWalks.API.Data;
using IndWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IndWalkDbContext indWalkDbContext;

        public WalkRepository(IndWalkDbContext indWalkDbContext)
        {
            this.indWalkDbContext = indWalkDbContext;
        }

        public async Task<Walk> GetAsync(Guid id)
        {
            return await indWalkDbContext.Walks.Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await indWalkDbContext.Walks
                .Include("Region")
                .Include("WalkDifficulty")
                .ToListAsync();
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign new id
            walk.Id=Guid.NewGuid();
            await indWalkDbContext.Walks.AddAsync(walk);
            await indWalkDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var exisistingWalk= await indWalkDbContext.Walks.FindAsync(id);

            if (exisistingWalk != null)
            {
                exisistingWalk.Length=walk.Length;
                exisistingWalk.Name=walk.Name;
                exisistingWalk.WalkDifficultyId=walk.WalkDifficultyId;
                exisistingWalk.RegionId=walk.RegionId;

                await indWalkDbContext.SaveChangesAsync();

                return exisistingWalk;
            }
            return null;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await indWalkDbContext.Walks.FindAsync(id);

            if(existingWalk != null)
            {
                indWalkDbContext.Walks.Remove(existingWalk);
                await indWalkDbContext.SaveChangesAsync();
                return existingWalk;
            }
            return null;
        }
    }
}
