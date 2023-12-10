using FoodAPI.IRepositories;
using FoodAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipe.Helpers;

namespace FoodAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private IRecipeRepository _recipeRepository;
        public FoodController(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }
        [HttpPost("GetRecipeList")]
        public async Task<IActionResult> GetRecipeList(PagerRequest request)
        {
            var data = await _recipeRepository.GetAllActiveRecipes(request);
            return Ok(new ApiResponse<PagerResponse<RecipeDetails>>
            {
                Result = data
            });
        }
        [HttpPost("GetRecipeById")]
        public async Task<IActionResult> GetRecipeById(KeyValues kv)
        {
            var data = await _recipeRepository.GetActiveRecipeById(kv.value??0);
            return Ok(new ApiResponse<RecipeDetails>
            {
                Result = data
            });
        }
        [HttpPost("SaveRecipe")]
        public async Task<IActionResult> SaveRecipe(RecipeDetails request)
        {
            var recipe = JsonConvert.DeserializeObject<FoodAPI.Models.Recipe>(JsonConvert.SerializeObject(request));
            if (recipe?.ID == 0) request.ID = await _recipeRepository.AddRecipe(recipe);
            else request.ID = await _recipeRepository.UpdateRecipe(recipe!);
            var result = await _recipeRepository.InsertOrUpdateIngredients(request.Ingredients, request.ID);
            return Ok(new ApiResponse<bool>
            {
                Result = result,
                Success = result,
                Message = $"Saving recipe {(result ? "" : "un")}successful"
            });
        }
        [HttpPost("DeleteRecipeById")]
        public async Task<IActionResult> DeleteRecipeById(KeyValues kv)
        {
            var result = await _recipeRepository.DeleteRecipe(kv.value ?? 0);
            return Ok(new ApiResponse<bool>
            {
                Result = result,
                Success = result,
                Message = $"Deleting recipe {(result ? "" : "un")}successful"
            });
        }
        [HttpPost("DeleteIngredientById")]
        public async Task<IActionResult> DeleteIngredientById(KeyValues kv)
        {
            var result = await _recipeRepository.DeleteIngredient(kv.value ?? 0);
            return Ok(new ApiResponse<bool>
            {
                Result = result,
                Success = result,
                Message = $"Deleting ingredient {(result ? "" : "un")}successful"
            });
        }
        [HttpPost("GetFavourites")]
        public async Task<IActionResult> GetFavourites(PagerRequest request)
        {
            var result = await _recipeRepository.GetAllFavouriteRecipes(request);
            return Ok(new ApiResponse<PagerResponse<RecipeDetails>>
            {
                Result = result,
            });
        }
        [HttpPost("ChangeFav")]
        public async Task<IActionResult> ChangeFav(KeyValues kv)
        {
            var result = await _recipeRepository.ChangeFav(kv.value??0,kv.key!);
            return Ok(new ApiResponse<bool>
            {
                Result = result,
            });
        }
    }
}
