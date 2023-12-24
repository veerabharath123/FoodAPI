using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;

namespace FoodAPI.Helpers
{
    public static class AESAlgorithm
    {
        private static string DecryptStringFromBytes(this byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            // Declare the string used to hold
            // the decrypted text.
            string? plaintext = null;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var aesAlg = Aes.Create())
            {
                //Settings
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                //aesAlg.FeedbackSize = 128;
                aesAlg.Key = key;
                aesAlg.IV = iv;
                // Create a decrytor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        public static string? DecryptStringAES(this string cipherText,IConfiguration config)
        {
            try
            {
                var keybytes = Encoding.UTF8.GetBytes(config.GetSection("Encryption").GetSection("key").Value!);
                var iv = Encoding.UTF8.GetBytes(config.GetSection("Encryption").GetSection("iv").Value!);

                var encrypted = Convert.FromBase64String(cipherText);
                var decriptedFromJavascript = encrypted.DecryptStringFromBytes(keybytes, iv);
                return string.Format(decriptedFromJavascript);
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
