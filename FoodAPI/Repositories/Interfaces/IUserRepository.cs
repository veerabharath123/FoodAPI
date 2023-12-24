using FoodAPI.Dtos.RequestDto;
using FoodAPI.Models;

namespace FoodAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetActiveUserById(decimal id);
        Task<User> GetUserById(decimal id);
        Task<List<User>> GetUsers();
        Task<User> GetUserByUsername(string username);
        Task<decimal> InsertUser(LoginRequest request);
    }
}
