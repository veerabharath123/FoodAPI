using FoodAPI.Helpers;
using FoodAPI.IRepositories;
using FoodAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Recipe.Helpers;

namespace FoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IRecipeRepository _recipeRepository;
        private IConfiguration _config;
        public UsersController(IRecipeRepository recipeRepository, IConfiguration config)
        {
            _recipeRepository = recipeRepository;
            _config = config;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser request)
        {
            var str = AESAlgorithm.DecryptStringAES(request.password!,_config);
            return Ok(new ApiResponse<bool>
            {
                Result = true
            });
        }
    }
}
