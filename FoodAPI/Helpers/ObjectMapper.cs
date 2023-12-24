using AutoMapper;
using FoodAPI.Dtos.ResponseDto;
using FoodAPI.Models;

namespace FoodAPI.Helpers
{
    public class ObjectMapper:Profile
    {
        public ObjectMapper()
        {
            CreateMap<RecipeDetailsResponse, FoodAPI.Models.Recipe>().ReverseMap();
            CreateMap<RecipeResponse, FoodAPI.Models.Recipe>().ReverseMap();
            CreateMap<IngredientsResponse, Ingredient>().ReverseMap();
            CreateMap<RecipeDetailsResponse, RecipeResponse>().ReverseMap();
        }
    }
}
