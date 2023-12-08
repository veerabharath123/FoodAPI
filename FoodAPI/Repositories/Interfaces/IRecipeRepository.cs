using FoodAPI.Models;

namespace FoodAPI.IRepositories
{
    public interface IRecipeRepository
    {
        Task<RecipeDetails> GetActiveRecipeById(decimal id);
        Task<List<RecipeDetails>> GetAllActiveRecipes();
        Task<decimal> AddRecipe(Recipe recipe);
        Task<decimal> UpdateRecipe(Recipe recipe);
        Task<bool> DeleteRecipe(decimal id);
        Task<bool> InsertOrUpdateIngredients(List<Ingredient> ingredients,decimal? id);
        Task<bool> DeleteIngredient(decimal id);
        Task<List<RecipeDetails>> GetAllFavouriteRecipes();
        Task<bool> ChangeFav(decimal id, string change);


    }
}
