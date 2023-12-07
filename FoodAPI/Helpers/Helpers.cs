using Microsoft.Identity.Client;

namespace FoodAPI.Helpers
{
    public static class Helpers
    {
        public static byte[] GetBytes(this string stringInBase64)
        {
            return System.Convert.FromBase64String(stringInBase64);
        }
    }
}
