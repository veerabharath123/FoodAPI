namespace FoodAPI.Models
{
    public class RecipeDetails:Recipe
    {
        public int TotalIngredients { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public RecipeDetails()
        {
            Ingredients = new List<Ingredient>();
        }
    }
}
