using static System.Net.WebRequestMethods;
using System;

namespace FoodAPI.Models
{
    public class User:BaseClass<decimal>
    {
        public string? FIRSTNAME { get; set; }
        public string? LASTNAME { get; set; }
        public string? USERNAME { get; set; }
        public string? EMAIL { get; set; }
        public string? PASSWORD { get; set; }
        public string? LAST_PASSWORD { get; set; }
        public int? OTP { get; set; }
        public DateTime? OTP_DATE { get; set; }
        public TimeSpan? OTP_TIME { get; set; }
        public decimal? INGREDIENT_ID { get; set; }
    }
}
