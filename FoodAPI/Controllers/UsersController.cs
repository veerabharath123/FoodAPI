using FoodAPI.Dtos.RequestDto;
using FoodAPI.Helpers;
using FoodAPI.IRepositories;
using FoodAPI.Services.Classes;
using FoodAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Helpers;

namespace FoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IUsersServices _usersServices;
        public UsersController(IUsersServices usersServices)
        {
            _usersServices = usersServices;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _usersServices.Login(request);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("Gettoken")]
        public IActionResult Gettoken()
        {
            var result = _usersServices.gettoken();
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(LoginRequest request)
        {
            var result = await _usersServices.CreateUser(request);
            return Ok(result);
        }
    }
}
