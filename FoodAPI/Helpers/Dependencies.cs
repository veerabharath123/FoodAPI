using FoodAPI.Database;
using FoodAPI.IRepositories;
using FoodAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Helpers
{
    public static class Dependencies
    {
        public static void Inject(this IServiceCollection services)
        {
            services.AddScoped<DbContext, FoodDbContext>();
            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }
    }
}
