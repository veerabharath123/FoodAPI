using Azure.Core;

namespace FoodAPI.Dtos.ResponseDto
{
    public class UserResponse
    {
        public string accessToken { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;
        public DateTime expireyDate { get; set; }
        public decimal user_id { get; set; }

    }
}
