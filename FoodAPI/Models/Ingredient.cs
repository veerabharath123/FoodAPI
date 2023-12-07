namespace FoodAPI.Models
{
    public class Ingredient:BaseClass<decimal>
    {
        public string? INGREDIENT_NAME { get; set; }
        public string? QUANTITY { get; set; }
        public decimal? RECIPE_ID { get; set; }

    }
}
