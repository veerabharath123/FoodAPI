namespace FoodAPI.Dtos.ResponseDto
{
    public class RecipeDetailsResponse : RecipeResponse
    {
        public string image { get; set; } = string.Empty;
        public int totalingredients { get; set; } 
        public List<IngredientsResponse> ingredients { get; set; } = new List<IngredientsResponse>();

    }
}
