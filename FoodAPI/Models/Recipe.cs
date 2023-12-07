namespace FoodAPI.Models
{
    public class Recipe:BaseClass<decimal>
    {
        public string? RECIPE_NAME { get; set; }
        public decimal? USER_ID { get; set; }
        public string? DESCRIPTION { get; set; } = string.Empty;
        public string? FAVOURITES { get; set; }
    }
}
