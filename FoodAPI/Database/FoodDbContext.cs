using FoodAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodAPI.Database
{
    public class FoodDbContext:DbContext
    {
        public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Image> Images { get; set; }
        
    }
}
