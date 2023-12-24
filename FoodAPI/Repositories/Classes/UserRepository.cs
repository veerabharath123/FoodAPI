using FoodAPI.Database;
using FoodAPI.Dtos.RequestDto;
using FoodAPI.Helpers;
using FoodAPI.Models;
using FoodAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly FoodDbContext _context;
        private readonly DateTime _dt;
        private HashingAlgorithm _hash;
        public UserRepository(FoodDbContext context) {
            _hash = new HashingAlgorithm(1000);
            _dt = DateTime.Now;
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

        public async Task<User> GetUserByUsername(string username)
        {
            var data = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => (x.USERNAME == username || x.EMAIL == username) && x.DELETE_FLAG != "Y" && x.ACTIVE == "Y");
            return data;
        }
        public async Task<decimal> InsertUser(LoginRequest request)
        {
            var user = new User
            {
                CREATED_DATE = _dt.Date,
                CREATED_TIME = TimeSpan.Parse(_dt.ToString("HH:mm:ss")),
                USERNAME = request.username,
                EMAIL = request.emailOrUsername,
                PASSWORD = _hash.GeneratePasswordHash(request.password!),
                DELETE_FLAG = "N",
                ACTIVE = "Y"
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user.ID;
        }
    }
}
