using FoodAPI.Database;
using FoodAPI.Models;

namespace FoodAPI.Repositories
{
    public class DocumentRepository
    {
        private readonly FoodDbContext _context;
        public DocumentRepository(FoodDbContext context)
        {
            _context = context;
        }

        public async Task<KeyValues> SaveImage(KeyValues kv)
        {
            if (!string.IsNullOrEmpty(kv.key))
            {

            }
            return kv;
        }
    }
}
