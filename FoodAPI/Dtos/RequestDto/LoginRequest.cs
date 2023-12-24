using System.ComponentModel.DataAnnotations;

namespace FoodAPI.Dtos.RequestDto
{
    public class LoginRequest
    {
        [Required]
        public string? emailOrUsername { get; set; }
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
