using System.ComponentModel.DataAnnotations;

namespace FoodAPI.Models
{
    public class LoginUser
    {
        [Required]
        public string? emailOrUsername { get; set; }
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
    }
}
