using Azure.Core;
using FoodAPI.Database;
using FoodAPI.IRepositories;
using FoodAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Recipe.Helpers;
using System.Text.Json.Serialization;

namespace FoodAPI.Repositories
{
    public class RecipeRepository: IRecipeRepository
    {
        private readonly FoodDbContext _context;
        private IDocumentRepository _documentRepository;
        public RecipeRepository(FoodDbContext context, IDocumentRepository documentRepository)
        {
            _context = context;
            _documentRepository = documentRepository;
        }

        public async Task<RecipeDetails> GetActiveRecipeById(decimal id)
        {
            var RecipeDetail = new RecipeDetails();
            var data = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(a => a.ACTIVE == "Y" && a.DELETE_FLAG != "Y"
                              && a.ID == id);
            if(data != null)
            {
                RecipeDetail = JsonConvert.DeserializeObject<RecipeDetails>(JsonConvert.SerializeObject(data));
                RecipeDetail.Ingredients = await _context.Ingredients.AsNoTracking().Where(a => a.ACTIVE == "Y" && a.DELETE_FLAG != "Y"
                              && a.RECIPE_ID == id).ToListAsync();
                if (data.IMAGE_ID.HasValue)
                {
                    RecipeDetail.Image = await _documentRepository.GetImage(data.IMAGE_ID);
                }
            }
            return RecipeDetail;
        }
        public async Task<PagerResponse<RecipeDetails>> GetAllActiveRecipes(PagerRequest request)
        {
            var RecipeDetails = new List<RecipeDetails>();
            var data = await _context.Recipes.AsNoTracking().Where(a => a.ACTIVE == "Y" && a.DELETE_FLAG != "Y").ToListAsync();
                
            foreach (var item in data)
            {
                var RecipeDetail = new RecipeDetails();
                RecipeDetail = JsonConvert.DeserializeObject<RecipeDetails>(JsonConvert.SerializeObject(item));
                RecipeDetail.TotalIngredients = await _context.Ingredients.AsNoTracking().CountAsync(a => a.ACTIVE == "Y" && a.DELETE_FLAG != "Y"
                              && a.RECIPE_ID == item.ID);
                if (item.IMAGE_ID.HasValue)
                {
                    RecipeDetail.Image = await _documentRepository.GetImage(item.IMAGE_ID);
                }
                RecipeDetails.Add(RecipeDetail);
            }
            return await Pager<RecipeDetails>.Paginate(RecipeDetails, request);
        }
        public async Task<PagerResponse<RecipeDetails>> GetAllFavouriteRecipes(PagerRequest request)
        {
            var RecipeDetails = new List<RecipeDetails>();
            var data = await _context.Recipes.AsNoTracking().Where(a => a.ACTIVE == "Y" && a.DELETE_FLAG != "Y" && a.FAVOURITES == "Y").ToListAsync();
            foreach (var item in data)
            {
                var RecipeDetail = new RecipeDetails();
                RecipeDetail = JsonConvert.DeserializeObject<RecipeDetails>(JsonConvert.SerializeObject(item));
                RecipeDetail.TotalIngredients = await _context.Ingredients.AsNoTracking().CountAsync(a => a.ACTIVE == "Y" && a.DELETE_FLAG != "Y"
                              && a.RECIPE_ID == item.ID);
                if (item.IMAGE_ID.HasValue)
                {
                    RecipeDetail.Image = await _documentRepository.GetImage(item.IMAGE_ID);
                }
                RecipeDetails.Add(RecipeDetail);
            }
            return await Pager<RecipeDetails>.Paginate(RecipeDetails, request);
        }
        public async Task<decimal> AddRecipe(FoodAPI.Models.Recipe recipe)
        {
            var dt = DateTime.Now;
            recipe.ACTIVE = "Y";
            recipe.DELETE_FLAG = "N";
            recipe.CREATED_DATE = dt.Date;
            recipe.CREATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
            await _context.Recipes.AddAsync(recipe);
            if (await _context.SaveChangesAsync() > 0)
                return recipe.ID;
            else return 0;
        }
        public async Task<decimal> UpdateRecipe(FoodAPI.Models.Recipe recipe)
        {
            var exist = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.ID == recipe.ID && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if(exist != null)
            {
                var dt = DateTime.Now;
                exist.DESCRIPTION = recipe.DESCRIPTION;
                exist.RECIPE_NAME = recipe.RECIPE_NAME;
                exist.IMAGE_ID = recipe.IMAGE_ID;
                exist.UPDATED_DATE = dt.Date;
                exist.RECIPE_TYPE_ID = recipe.RECIPE_TYPE_ID;
                exist.UPDATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
                _context.Recipes.Update(exist);
                if (await _context.SaveChangesAsync() > 0)
                    return recipe.ID;
            }            
            return 0;
        }
        public async Task<bool> DeleteRecipe(decimal id)
        {
            var exist = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if (exist != null)
            {
                var dt = DateTime.Now;
                exist.DELETE_FLAG = "Y";
                exist.UPDATED_DATE = dt.Date;
                exist.UPDATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
                _context.Recipes.Update(exist);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<decimal> AddIngredient(Ingredient ingredient)
        {
            var dt = DateTime.Now;
            ingredient.ACTIVE = "Y";
            ingredient.DELETE_FLAG = "N";
            ingredient.CREATED_DATE = dt.Date;
            ingredient.CREATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
            await _context.Ingredients.AddAsync(ingredient);
            if (await _context.SaveChangesAsync() > 0)
                return ingredient.ID;
            else return 0;
        }
        public async Task<decimal> UpdateIngredient(Ingredient ingredient)
        {
            var exist = await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(x => x.RECIPE_ID == ingredient.RECIPE_ID
                        && x.ID == ingredient.ID && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if(exist != null)
            {
                var dt = DateTime.Now;
                exist.UPDATED_DATE = dt.Date;
                exist.UPDATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
                exist.INGREDIENT_NAME = ingredient.INGREDIENT_NAME;
                exist.QUANTITY = ingredient.QUANTITY;
                _context.Ingredients.Update(exist);
                if (await _context.SaveChangesAsync() > 0)
                    return ingredient.ID;
            }
            return 0;
        }
        public async Task<bool> InsertOrUpdateIngredients(List<Ingredient> ingredients,decimal? id)
        {
            foreach(var item in ingredients)
            {
                item.RECIPE_ID = id ?? 0;
                if (item.ID == 0) item.ID = await AddIngredient(item);
                else item.ID = await UpdateIngredient(item);
            }
            return ingredients.Count(x => x.ID != 0) > 0;
        }
        public async Task<bool> DeleteIngredient(decimal id)
        {
            var exist = await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if(exist != null)
            {
                var dt = DateTime.Now;
                exist.UPDATED_DATE = dt.Date;
                exist.UPDATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
                exist.DELETE_FLAG = "Y";
                _context.Ingredients.Update(exist);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<bool> ChangeFav(decimal id,string change)
        {
            var exist = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if (exist != null)
            {
                exist.FAVOURITES = change;
                _context.Recipes.Update(exist);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}
