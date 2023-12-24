using FoodAPI.Database;
using FoodAPI.IRepositories;
using FoodAPI.Repositories;
using FoodAPI.Repositories.Interfaces;
using FoodAPI.Services.Classes;
using FoodAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Helpers
{
    public static class Dependencies
    {
        public static void InjectDependencies(this IServiceCollection services)
        {
            #region Db

            services.AddScoped<DbContext, FoodDbContext>();

            #endregion

            #region repositories

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            #endregion 

            #region services

            services.AddScoped<IUsersServices, UsersServices>();
            services.AddScoped<IFoodServices, FoodServices>();
            services.AddScoped<IDocumentServices, DocumentServices>();

            #endregion

            #region others

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            #endregion
        }
    }
}
