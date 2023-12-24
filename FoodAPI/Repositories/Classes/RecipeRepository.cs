using AutoMapper;
using Azure.Core;
using FoodAPI.Database;
using FoodAPI.Dtos.RequestDto;
using FoodAPI.Dtos.ResponseDto;
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
        private readonly DateTime dt;
        private readonly FoodDbContext _context;
        private IDocumentRepository _documentRepository;
        private IMapper _mapper;
        public RecipeRepository(FoodDbContext context, IDocumentRepository documentRepository,IMapper mapper)
        {
            dt = DateTime.Now;
            _context = context;
            _documentRepository = documentRepository;
            _mapper = mapper;
        }

        public async Task<RecipeDetailsResponse> GetActiveRecipeById(decimal id)
        {
            var result = (from a in _context.Recipes
                          join b in _context.Ingredients.Where(x => x.ACTIVE == "Y" && x.DELETE_FLAG != "Y") on a.ID equals b.RECIPE_ID into ab
                          where a.ACTIVE == "Y" && a.DELETE_FLAG != "Y" && a.ID == id
                          group ab by a into g
                          select new RecipeDetailsResponse()
                          {
                              ingredients = _mapper.Map<List<IngredientsResponse>>(g.SelectMany(x => x).ToList()),
                              recipe_name = g.Key.RECIPE_NAME!,
                              description = g.Key.DESCRIPTION!,
                              id = g.Key.ID,
                              image_id = g.Key.IMAGE_ID,
                              recipe_type_id = g.Key.RECIPE_TYPE_ID,
                          }).AsNoTracking().FirstOrDefault();
            if (result?.image_id != null)
                result.image = await _documentRepository.GetImage(result.image_id);
            return result;
        }
        public async Task<PagerResponse<RecipeDetailsResponse>> GetAllActiveRecipes(DecimalPageRequest request)
        {
            var data = (from a in _context.Recipes
                        join b in _context.Ingredients.Where(x => x.DELETE_FLAG != "Y" && x.ACTIVE == "Y") on a.ID equals b.RECIPE_ID
                        where a.DELETE_FLAG != "Y" && a.ACTIVE == "Y" && a.USER_ID == request.id
                        group a by new { a.ID, a.RECIPE_NAME, a.IMAGE_ID, a.RECIPE_TYPE_ID,a.FAVOURITES } into grouped
                        select new RecipeDetailsResponse
                        {
                            totalingredients = grouped.Count(),
                            recipe_name = grouped.Key.RECIPE_NAME,
                            id = grouped.Key.ID,
                            image_id = grouped.Key.IMAGE_ID,
                            recipe_type_id = grouped.Key.RECIPE_TYPE_ID,
                            favourites = grouped.Key.FAVOURITES
                        }).AsNoTracking();
            var result = await Pager<RecipeDetailsResponse>.Paginate(data, request.pager);
            if (request.pager.view == "block-view")
                foreach (var item in result.Pages.Where(x => x.image_id.HasValue))
                {
                    item.image = await _documentRepository.GetImage(item.image_id);
                }
            return result;
        }
        public async Task<PagerResponse<RecipeDetailsResponse>> GetAllFavouriteRecipes(DecimalPageRequest request)
        {
            var data = (from a in _context.Recipes
                        join b in _context.Ingredients on a.ID equals b.RECIPE_ID into ingredientsGroup
                        from ingobj in ingredientsGroup.DefaultIfEmpty()
                        where a.DELETE_FLAG != "Y" && a.ACTIVE == "Y" && a.FAVOURITES == "Y" && a.USER_ID == request.id
                        group a by new { a.ID, a.RECIPE_NAME, a.IMAGE_ID, a.RECIPE_TYPE_ID, a.FAVOURITES } into grouped
                        select new RecipeDetailsResponse
                        {
                            totalingredients = grouped.Count(),
                            recipe_name = grouped.Key.RECIPE_NAME,
                            id = grouped.Key.ID,
                            image_id = grouped.Key.IMAGE_ID,
                            recipe_type_id = grouped.Key.RECIPE_TYPE_ID,
                            favourites = grouped.Key.FAVOURITES
                        }).AsNoTracking();
            var result = await Pager<RecipeDetailsResponse>.Paginate(data, request.pager);
            if (request.pager.view == "block-view")
                result.Pages.Where(x => x.image_id.HasValue).ToList().ForEach(async x => x.image = await _documentRepository.GetImage(x.image_id));
            return result;
        }
        public async Task<decimal> AddRecipe(RecipeResponse request)
        {
            var recipe = _mapper.Map<FoodAPI.Models.Recipe>(request);
            recipe.ACTIVE = "Y";
            recipe.DELETE_FLAG = "N";
            recipe.CREATED_DATE = dt.Date;
            recipe.CREATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
            await _context.Recipes.AddAsync(recipe);
            if (await _context.SaveChangesAsync() > 0)
                return recipe.ID;
            else return 0;
        }
        public async Task<decimal> UpdateRecipe(RecipeResponse request)
        {
            var exist = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.ID == request.id && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if(exist != null)
            {
                var recipe = _mapper.Map<FoodAPI.Models.Recipe>(request);
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
                exist.DELETE_FLAG = "Y";
                exist.UPDATED_DATE = dt.Date;
                exist.UPDATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
                _context.Recipes.Update(exist);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }
        public async Task<decimal> AddIngredient(IngredientsResponse request)
        {
            var ingredient = _mapper.Map<Ingredient>(request);
            ingredient.ACTIVE = "Y";
            ingredient.DELETE_FLAG = "N";
            ingredient.CREATED_DATE = dt.Date;
            ingredient.CREATED_TIME = TimeSpan.Parse(dt.ToString("HH:mm:ss"));
            await _context.Ingredients.AddAsync(ingredient);
            if (await _context.SaveChangesAsync() > 0)
                return ingredient.ID;
            else return 0;
        }
        public async Task<decimal> UpdateIngredient(IngredientsResponse request)
        {
            var ingredient = _mapper.Map<Ingredient>(request);
            var exist = await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(x => x.RECIPE_ID == ingredient.RECIPE_ID
                        && x.ID == ingredient.ID && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if(exist != null)
            {
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
        public async Task<bool> InsertOrUpdateIngredients(List<IngredientsResponse> ingredients,decimal? id)
        {
            foreach(var item in ingredients)
            {
                item.recipe_id = id ?? 0;
                if (item.id == 0) item.id = await AddIngredient(item);
                else item.id = await UpdateIngredient(item);
            }
            return ingredients.Count(x => x.id != 0) > 0;
        }
        public async Task<bool> DeleteIngredient(decimal id)
        {
            var exist = await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id && x.ACTIVE == "Y" && x.DELETE_FLAG != "Y");
            if(exist != null)
            {
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
