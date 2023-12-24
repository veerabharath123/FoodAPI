using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.Models;
using Recipe.Helpers;

namespace FoodAPI.IRepositories
{
    public interface IRecipeRepository
    {
        Task<RecipeDetailsResponse> GetActiveRecipeById(decimal id);
        Task<PagerResponse<RecipeDetailsResponse>> GetAllActiveRecipes(DecimalPageRequest request);
        Task<PagerResponse<RecipeDetailsResponse>> GetAllFavouriteRecipes(DecimalPageRequest request);
        Task<decimal> AddRecipe(RecipeResponse request);
        Task<decimal> UpdateRecipe(RecipeResponse request);
        Task<bool> DeleteRecipe(decimal id);
        Task<decimal> AddIngredient(IngredientsResponse request);
        Task<decimal> UpdateIngredient(IngredientsResponse request);
        Task<bool> InsertOrUpdateIngredients(List<IngredientsResponse> ingredients, decimal? id);
        Task<bool> DeleteIngredient(decimal id);
        Task<bool> ChangeFav(decimal id, string change);



    }
}
