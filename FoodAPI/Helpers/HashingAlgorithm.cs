using System.Security.Cryptography;

namespace FoodAPI.Helpers
{
    public class HashingAlgorithm
    {
        public readonly int _numberOfIterations;
        public HashingAlgorithm(int numberOfIterations)
        {
            _numberOfIterations = numberOfIterations;
        }
        public string[] GeneratePasswordHash(string plainPassword)
        {

            byte[] saltBytes = GenerateRandomCryptographicBytes();
            Rfc2898DeriveBytes pbkdf = new Rfc2898DeriveBytes(plainPassword, saltBytes, _numberOfIterations);
            byte[] derivedBytes = pbkdf.GetBytes(32);
            string[] s = new string[2];
            s[0] = Convert.ToBase64String(derivedBytes);
            s[1] = Convert.ToBase64String(saltBytes);
            return s;
        }
        public byte[] GenerateRandomCryptographicBytes()
        {

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }
        public bool AuthenticateUser(string plainpassword, string hashpassword, string salt)
        {
            try
            {
                var pbkdf = new Rfc2898DeriveBytes(plainpassword, Convert.FromBase64String(salt), _numberOfIterations);
                var passwordtocheck = Convert.ToBase64String(pbkdf.GetBytes(32));
                return passwordtocheck == hashpassword;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
