using IndWalks.API.Data;
using IndWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly IndWalkDbContext indWalkDbContext;

        public WalkDifficultyRepository(IndWalkDbContext indWalkDbContext)
        {
            this.indWalkDbContext = indWalkDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id=Guid.NewGuid();
            await indWalkDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await indWalkDbContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var exisistingWalkDifficulty = await indWalkDbContext.WalkDifficulty.FindAsync(id);

            if (exisistingWalkDifficulty == null)
            {
                return null;
            }

            indWalkDbContext.WalkDifficulty.Remove(exisistingWalkDifficulty);
            await indWalkDbContext.SaveChangesAsync();
            return exisistingWalkDifficulty;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await indWalkDbContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await indWalkDbContext.WalkDifficulty.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var exisistingWalkDifficulty = await indWalkDbContext.WalkDifficulty.FindAsync(id);

            if(exisistingWalkDifficulty==null)
            {
                return null;
            }

            exisistingWalkDifficulty.Code = walkDifficulty.Code;
            await indWalkDbContext.SaveChangesAsync();
            return exisistingWalkDifficulty;

        }
    }
}
