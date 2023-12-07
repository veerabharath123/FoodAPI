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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users"); 
            modelBuilder.Entity<Recipe>().ToTable("Recipes"); 
            modelBuilder.Entity<Ingredient>().ToTable("Ingredients"); 
            modelBuilder.Entity<Image>().ToTable("Images"); 
        }
    }
}
