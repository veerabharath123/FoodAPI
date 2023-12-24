using AutoMapper;
using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.IRepositories;
using FoodAPI.Models;
using FoodAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipe.Helpers;

namespace FoodAPI.Services.Classes
{
    public class FoodServices: IFoodServices
    {
        private IRecipeRepository _recipeRepository;
        private IMapper _mapper;
        public FoodServices(IRecipeRepository recipeRepository,IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<PagerResponse<RecipeDetailsResponse>>> GetRecipeList(DecimalPageRequest request)
        {
            var data = await _recipeRepository.GetAllActiveRecipes(request);
            return new ApiResponse<PagerResponse<RecipeDetailsResponse>>
            {
                Result = data
            };
        }
        public async Task<ApiResponse<RecipeDetailsResponse>> GetRecipeById(decimal id)
        {
            var data = await _recipeRepository.GetActiveRecipeById(id);
            return new ApiResponse<RecipeDetailsResponse>
            {
                Result = data
            };
        }
        public async Task<ApiResponse<bool>> SaveRecipe(RecipeDetailsResponse request)
        {
            var recipe = _mapper.Map<RecipeResponse>(request);
            if (recipe?.id == 0) request.id = await _recipeRepository.AddRecipe(recipe);
            else request.id = await _recipeRepository.UpdateRecipe(recipe!);
            var result = await _recipeRepository.InsertOrUpdateIngredients(request.ingredients, request.id);
            return new ApiResponse<bool>
            {
                Result = result,
                Success = result,
                Message = $"{(recipe?.id == 0 ? "Saving" : "Updating")} recipe was {(result ? "" : "un")}successful"
            };
        }
        public async Task<ApiResponse<bool>> DeleteRecipeById(decimal id)
        {
            var result = await _recipeRepository.DeleteRecipe(id);
            return new ApiResponse<bool>
            {
                Result = result,
                Success = result,
                Message = $"Deleting recipe was {(result ? "" : "un")}successful"
            };
        }
        public async Task<ApiResponse<bool>> DeleteIngredientById(decimal id)
        {
            var result = await _recipeRepository.DeleteIngredient(id);
            return new ApiResponse<bool>
            {
                Result = result,
                Success = result,
                Message = $"Deleting ingredient was {(result ? "" : "un")}successful"
            };
        }
        public async Task<ApiResponse<PagerResponse<RecipeDetailsResponse>>> GetFavourites(DecimalPageRequest request)
        {
            var result = await _recipeRepository.GetAllFavouriteRecipes(request);
            return new ApiResponse<PagerResponse<RecipeDetailsResponse>>
            {
                Result = result,
            };
        }
        public async Task<ApiResponse<bool>> ChangeFav(decimal id,string change)
        {
            var result = await _recipeRepository.ChangeFav(id, change);
            return new ApiResponse<bool>
            {
                Result = result,
            };
        }
    }
}
