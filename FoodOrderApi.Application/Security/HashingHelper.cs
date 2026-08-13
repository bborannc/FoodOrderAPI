using System.Security.Cryptography;
using System.Text;

namespace FoodOrderApi.Application.Security
{
    public static class HashingHelper
    {
        // 1. Şifreyi Hash ve Salt'a dönüştürür (Kullanıcı Kayıt Esnasında)
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key; // Algoritmanın rastgele ürettiği tuz (salt) anahtarı
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        // 2. Girilen şifreyi veritabanındaki Hash ile karşılaştırır (Kullanıcı Giriş Esnasında)
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            // İki binary dizinin birebir eşit olup olmadığını kontrol eder
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
