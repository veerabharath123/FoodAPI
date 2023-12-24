using Recipe.Helpers;

namespace FoodAPI.Dtos.RequestDto
{
    public class DecimalPageRequest:DecimalRequest
    {
        public PagerRequest pager { get; set; }
    }
}
