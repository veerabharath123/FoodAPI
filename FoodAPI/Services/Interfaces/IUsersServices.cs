using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.Models;

namespace FoodAPI.Services.Interfaces
{
    public interface IUsersServices
    {
        Task<ApiResponse<UserResponse>> Login(LoginRequest request);
        string gettoken();
        Task<ApiResponse<UserResponse>> CreateUser(LoginRequest login);
    }
}
