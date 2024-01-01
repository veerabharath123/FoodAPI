using FoodAPI.Models;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
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
        public static IQueryable<T> Sort<T>(this IQueryable<T> q, List<SortingModel> sortList)
        {

            var parameter = Expression.Parameter(typeof(T), "p");
            Expression expression = parameter;

            for (int i = 0; i < sortList.Count; i++)
            {
                var property = Expression.Property(expression, sortList[i].name);
                expression = Expression.Lambda(property, parameter);
                string methodName = $"{(i == 0 ? "Order" : "Then")}By{(sortList[i].desc ? "Descending" : string.Empty)}";
                expression = Expression.Call(typeof(Queryable), methodName, new[] { q.ElementType, property.Type }, q.Expression, expression);
            }
            return q.Provider.CreateQuery<T>(expression);
        }
    } 
    public class SortingModel
    {
        public string name { get; set; } = string.Empty;
        public bool desc { get; set; }
    }
}
