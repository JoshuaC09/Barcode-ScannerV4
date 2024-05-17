using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace Price_Checker.Configuration
{
    internal class SecurityService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public SecurityService(string password, byte[] salt)
        {
            _key = DeriveKeyFromPassword(password, salt);
            _iv = new byte[16]; // AES uses a 128-bit (16-byte) IV
        }

        private byte[] DeriveKeyFromPassword(string password, byte[] salt)
        {
            const int iterations = 1000;
            const int desiredKeyLength = 32; // 256 bits
            using (var deriveBytes = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(password), salt, iterations, HashAlgorithmName.SHA256))
            {
                return deriveBytes.GetBytes(desiredKeyLength);
            }
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (var aes = Aes.Create())
                {
                    aes.Key = _key;
                    aes.IV = _iv;
                    using (var ms = new MemoryStream(cipherBytes))
                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException ex)
            {
                HandleDecryptionError(ex.Message);
                return null;
            }
            catch (FormatException ex)
            {
                HandleDecryptionError(ex.Message);
                return null;
            }
        }

        private void HandleDecryptionError(string message)
        {
            Console.WriteLine($"Decryption Error: {message}");
            MessageBox.Show("An error occurred while decrypting the data. Please check the encryption key and the encrypted data.", "Decryption Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            Environment.Exit(1);
        }
    }
}
