namespace FoodAPI.Dtos.ResponseDto
{
    public class IngredientsResponse
    {
        public decimal id { get; set; }
        public DateTime? date { get; set; }
        public TimeSpan? time { get; set; }
        public string ingredient_name { get; set; } = string.Empty;
        public string quantity { get; set; } = string.Empty;
        public decimal? recipe_id { get; set; }
        public string optional_flag { get; set; } = string.Empty;
    }
}
