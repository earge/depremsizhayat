using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Security
{
    public class Encryptor
    {
        private const string Key = "048827a1-62e9-4943-9e28-ccfa4e7ece41";
        public static string Encrypt(string clearString)
        {
            return EncryptCore(clearString, Encryptor.Key);
        }
        private static string EncryptCore(string clearString, string key)
        {
            byte[] byteArray = UTF8Encoding.UTF8.GetBytes(clearString);
            MD5CryptoServiceProvider cryptoService = new MD5CryptoServiceProvider();
            byte[] keyArray = cryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            cryptoService.Clear();
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = keyArray;
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            byte[] resultArray = tripleDES.CreateEncryptor().TransformFinalBlock(byteArray, 0, byteArray.Length);
            tripleDES.Clear();
            string encryptText = Convert.ToBase64String(resultArray, 0, resultArray.Length);
            return encryptText.Replace(" ", "ivj_jvi").Replace("+", "ivjpljvi").Replace("=", "ivjeqjvi").Replace("/", "ivjsljvi");
        }
    }
}
