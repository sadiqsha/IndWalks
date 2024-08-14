using IndWalks.API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace IndWalks.API.Data
{
    public class IndWalkDbContext : DbContext
    {
        public IndWalkDbContext(DbContextOptions<IndWalkDbContext> options) : base(options)
        {
            
        }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walks { get; set; }

        public DbSet<WalkDifficulty> WalkDifficulty { get; set; }




    }
}
