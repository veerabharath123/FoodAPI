using FoodAPI.Models;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodAPI.Helpers
{
    public static class Utilities
    {
        public static byte[] GetBytes(this string stringInBase64)
        {
            return System.Convert.FromBase64String(stringInBase64);
        }
        public static string GetToken(IConfiguration config, ClaimsIdentity? claims)
        {

            var JwtSection = config.GetSection("JwtSection");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(JwtSection.GetSection("key").Value!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.Now.AddHours(2), // Set token expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = JwtSection.GetSection("Audience").Value,
                Issuer = JwtSection.GetSection("Issuer").Value
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public static ClaimsIdentity GenerateClaims(this List<string> rolelist, User user)
        {
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.USERNAME!));
            return claimsIdentity;
        }
    } 
}
