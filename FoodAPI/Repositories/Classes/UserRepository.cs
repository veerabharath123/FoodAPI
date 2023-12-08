using FoodAPI.Database;
using FoodAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Repositories
{
    public class UserRepository
    {
        private readonly FoodDbContext _context;
        public UserRepository(FoodDbContext context) {
            _context = context;
        }

        public async Task<User> GetActiveUserById(decimal id)
        {
            var data = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            return data;
        }

        public async Task<User> GetUserById(decimal id)
        {
            var data = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id && x.DELETE_FLAG != "Y");
            return data;
        }

        public async Task<List<User>> GetUsers()
        {
            var data = await _context.Users.AsNoTracking().Where(x => x.DELETE_FLAG != "Y").ToListAsync();
            return data;
        }
    }
}
