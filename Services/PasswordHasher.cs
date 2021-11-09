using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Services
{
    public class PasswordHasher
    {
        private byte[] salt;
        private byte[] hash;

        public PasswordHasher()
        {
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
        }

        private void createHash(string password)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            hash = pbkdf2.GetBytes(20);
        }

        public string hashedPassword(string password)
        {
            createHash(password);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            return Convert.ToBase64String(hashBytes);
        }

        public bool verifyPassword(string password, string hashed)
        {
            byte[] hashBytes = Convert.FromBase64String(hashed);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;

        }
    }
}
