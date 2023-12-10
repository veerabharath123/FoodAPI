using FoodAPI.Models;
using Recipe.Helpers;

namespace FoodAPI.IRepositories
{
    public interface IRecipeRepository
    {
        Task<RecipeDetails> GetActiveRecipeById(decimal id);
        Task<PagerResponse<RecipeDetails>> GetAllActiveRecipes(PagerRequest request);
        Task<decimal> AddRecipe(FoodAPI.Models.Recipe recipe);
        Task<decimal> UpdateRecipe(FoodAPI.Models.Recipe recipe);
        Task<bool> DeleteRecipe(decimal id);
        Task<bool> InsertOrUpdateIngredients(List<Ingredient> ingredients,decimal? id);
        Task<bool> DeleteIngredient(decimal id);
        Task<PagerResponse<RecipeDetails>> GetAllFavouriteRecipes(PagerRequest request);
        Task<bool> ChangeFav(decimal id, string change);


    }
}
