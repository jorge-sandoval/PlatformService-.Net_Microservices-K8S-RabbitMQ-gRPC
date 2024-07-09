using PlatformService.Models;

namespace PlatformService.Data
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;

        public PlatformRepository(AppDbContext context)
        {
            _context = context;  
        }

        public void CreatePlaform(Platform platform)
        {
            if ( platform == null)
            {
                throw new ArgumentNullException( nameof(platform) );
            }

            _context.PlatForms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.PlatForms.ToList();
        }

        public Platform GetPlatformById(int id)
        {
            return _context.PlatForms.FirstOrDefault( p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}