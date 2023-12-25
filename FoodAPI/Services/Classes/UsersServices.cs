using Azure;
using Azure.Core;
using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.Helpers;
using FoodAPI.Models;
using FoodAPI.Repositories;
using FoodAPI.Repositories.Interfaces;
using FoodAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace FoodAPI.Services.Classes
{
    public class UsersServices : IUsersServices
    {
        private IConfiguration _config;
        private IUserRepository _userRepository;
        private HashingAlgorithm _hash;
        public UsersServices(IConfiguration config, IUserRepository userRepository)
        {
            _hash = new HashingAlgorithm(1000);
            _config = config;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<UserResponse>> Login(LoginRequest request)
        {
            bool response = false;
            var user = await _userRepository.GetUserByUsername(request.emailOrUsername!);
            if(user != null)
            {
                var plainPassword = AESAlgorithm.DecryptStringAES(request.password!, _config);
                response = _hash.AuthenticateUser(plainPassword, user.PASSWORD!);
                if (response) return GetUserDetails(user.USERNAME!,user.ID);
            }
            return new ApiResponse<UserResponse>
            {
                Success = response,
                Message = user == null ? $"User '{request.emailOrUsername}' is not registered" : "Invalid password"
            };
        }
        public string gettoken()
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, "hello"));
            return Utilities.GetToken(_config, claimsIdentity);
        }
        private ApiResponse<UserResponse> GetUserDetails(string username,decimal id)
        {
            var token = Utilities.GetToken(_config, null);
            return new ApiResponse<UserResponse>
            {
                Result = new UserResponse
                {
                    user_id = id,
                    username = username,
                    accessToken = token,
                    expireyDate = DateTime.Now.AddHours(Convert.ToInt32(_config.GetSection("JwtSection").GetSection("ExpireInHours").Value))
                }
            };
        }   
        public async Task<ApiResponse<UserResponse>> CreateUser(LoginRequest login)
        {
            login.password = AESAlgorithm.DecryptStringAES(login.password!, _config);
            decimal result = await _userRepository.InsertUser(login);
            if (result > 0) return GetUserDetails(login.username!, result);
            return new ApiResponse<UserResponse>
            {
                Success = false,
                Message = "Failed to create your account"
            };
        }
    }
}
