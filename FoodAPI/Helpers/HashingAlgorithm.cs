using Newtonsoft.Json.Linq;
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
        public string GeneratePasswordHash(string plainPassword)
        {

            byte[] saltBytes = GenerateRandomCryptographicBytes();
            Rfc2898DeriveBytes pbkdf = new Rfc2898DeriveBytes(plainPassword, saltBytes, _numberOfIterations);
            byte[] derivedBytes = pbkdf.GetBytes(32);            
            string password = Convert.ToBase64String(derivedBytes);
            string salt = Convert.ToBase64String(saltBytes);
            int saltHalfLength = salt.Length / 2;
            return $"{(saltHalfLength * 8)}R{password.Length * 4}L{salt.Substring(0, saltHalfLength)}{password}{salt.Substring(saltHalfLength)}";
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
        public bool AuthenticateUser(string? plainpassword, string hashpassword)
        {
            try
            {
                if (string.IsNullOrEmpty(plainpassword)) return false;
                var keys = GetSaltFromHash(hashpassword);
                var pbkdf = new Rfc2898DeriveBytes(plainpassword, Convert.FromBase64String(keys[0]), _numberOfIterations);
                var passwordtocheck = Convert.ToBase64String(pbkdf.GetBytes(32));
                return passwordtocheck == keys[1];
            }
            catch (Exception)
            {
                return false;
            }
        }
        private string[] GetSaltFromHash1(string hashpassword)
        {
            string[] s = new string[2];
            int sn = Convert.ToInt32(hashpassword.Split("L")[0].Split("R")[0])/8;
            int pn = Convert.ToInt32(hashpassword.Split("L")[0].Split("R")[1])/4;
            int r = hashpassword.Split("L")[0].Length + 1;
            s[0] = hashpassword.Substring(r, sn) + hashpassword.Substring(sn + pn + r);
            s[1] = hashpassword.Substring(r + sn, pn);
            return s;
        }
        private string[] GetSaltFromHash(string hashpassword)
        {
            string[] s = new string[2];

            int sn = Convert.ToInt32(hashpassword.Split("L")[0].Split("R")[0]) / 8;
            int pn = Convert.ToInt32(hashpassword.Split("L")[0].Split("R")[1]) / 4;

            int r = hashpassword.Split("L")[0].Length + 1;

            s[0] = hashpassword[r..(r + sn)] + hashpassword[(r + sn + pn)..];
            s[1] = hashpassword[(r + sn)..(r + sn + pn)];

            return s;
        }
    }
}
