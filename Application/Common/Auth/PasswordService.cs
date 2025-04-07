using Application.Business.Common.ViewModel;
using Application.Common.Interfaces.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Auth
{
    public class PasswordService : IPasswordService
    {
        public Password_VM HashPassword(string password, byte[] hasSalt)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();

            hasSalt = new byte[24];
            cryptoProvider.GetBytes(hasSalt);

            var hash = GetPbkdf2Bytes(password, hasSalt, 1000, 20);

            var hashed = 1000 + ":" + Convert.ToBase64String(hasSalt) + ":" + Convert.ToBase64String(hash);

            return new Password_VM { HashedPassword = hashed, Salt = hasSalt };
        }

        public byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);

            pbkdf2.IterationCount = iterations;

            return pbkdf2.GetBytes(outputBytes);
        }

        public bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[0]);
            var salt = Convert.FromBase64String(split[1]);
            var hash = Convert.FromBase64String(split[2]);

            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }


    }
}
