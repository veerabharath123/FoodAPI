using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.Models;
using Recipe.Helpers;

namespace FoodAPI.Services.Interfaces
{
    public interface IFoodServices
    {
        Task<ApiResponse<PagerResponse<RecipeDetailsResponse>>> GetRecipeList(DecimalPageRequest request);
        Task<ApiResponse<RecipeDetailsResponse>> GetRecipeById(decimal id);
        Task<ApiResponse<bool>> SaveRecipe(RecipeDetailsResponse request);
        Task<ApiResponse<bool>> DeleteRecipeById(decimal id);
        Task<ApiResponse<bool>> DeleteIngredientById(decimal id);
        Task<ApiResponse<PagerResponse<RecipeDetailsResponse>>> GetFavourites(DecimalPageRequest request);
        Task<ApiResponse<bool>> ChangeFav(decimal id, string change);

    }
}
