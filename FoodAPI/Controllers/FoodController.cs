using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.IRepositories;
using FoodAPI.Models;
using FoodAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipe.Helpers;

namespace FoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FoodController : ControllerBase
    {
        private IFoodServices _foodServices;
        public FoodController(IFoodServices foodServices)
        {
            _foodServices = foodServices;
        }
        [HttpPost("GetRecipeList")]
        public async Task<IActionResult> GetRecipeList(DecimalPageRequest request)
        {
            var result = await _foodServices.GetRecipeList(request);
            return Ok(result);
        }
        [HttpPost("GetRecipeById")]
        public async Task<IActionResult> GetRecipeById(DecimalRequest request)
        {
            var result = await _foodServices.GetRecipeById(request.id);
            return Ok(result);
        }
        [HttpPost("SaveRecipe")]
        public async Task<IActionResult> SaveRecipe(RecipeDetailsResponse request)
        {
            var result = await _foodServices.SaveRecipe(request);
            return Ok(result);
        }
        [HttpPost("DeleteRecipeById")]
        public async Task<IActionResult> DeleteRecipeById(DecimalRequest request)
        {
            var result = await _foodServices.DeleteRecipeById(request.id);
            return Ok(result);
        }
        [HttpPost("DeleteIngredientById")]
        public async Task<IActionResult> DeleteIngredientById(DecimalRequest request)
        {
            var result = await _foodServices.DeleteIngredientById(request.id);
            return Ok(result);
        }
        [HttpPost("GetFavourites")]
        public async Task<IActionResult> GetFavourites(DecimalPageRequest request)
        {
            var result = await _foodServices.GetFavourites(request);
            return Ok(result);
        }
        [HttpPost("ChangeFav")]
        public async Task<IActionResult> ChangeFav(KeyValues kv)
        {
            var result = await _foodServices.ChangeFav(kv.value??0,kv.key!);
            return Ok(result);
        }
    }
}
