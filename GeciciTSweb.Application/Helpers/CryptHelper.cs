using Azure.Core;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeciciTSweb.Application.Helpers
{
    public static class CryptHelper
    {

        public static string HashPassword(string password)
        {
            // BCrypt kütüphanesi zaten kullanmış olduğum EnhancedHashPassword metodu içerisinde saltlama işlemi yapıyor.
            // HashPassword(inputKey, GenerateSalt(workFactor), enhancedEntropy: true, hashType)
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA512);
        }   

        public static bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword, HashType.SHA512);
        }

        public static string ValidateAndResetPassword(string oldPassword, string oldHashedPassword, string newPassword)
        {
            return BCrypt.Net.BCrypt.ValidateAndReplacePassword(oldPassword, oldHashedPassword, newPassword);
        }
    }
}
