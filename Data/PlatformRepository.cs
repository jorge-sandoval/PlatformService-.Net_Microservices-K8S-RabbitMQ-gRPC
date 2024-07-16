using PlatformService.Models;
using Microsoft.EntityFrameworkCore;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;

        public PlatformRepository(AppDbContext context)
        {
            _context = context;  
        }

        public async Task CreatePlaform(Platform platform)
        {
            if ( platform == null)
            {
                throw new ArgumentNullException( nameof(platform) );
            }

            await _context.PlatForms.AddAsync(platform);
        }

        public async Task<IEnumerable<Platform>> GetAllPlatforms()
        {
            return await _context.PlatForms.ToListAsync();
        }

        public async Task<Platform?> GetPlatformById(int id)
        {
            return await _context.PlatForms.FirstOrDefaultAsync( p => p.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}