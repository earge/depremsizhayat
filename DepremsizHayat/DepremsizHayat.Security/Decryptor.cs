using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DepremsizHayat.Security
{
    public class Decryptor
    {
        private const string Key = "048827a1-62e9-4943-9e28-ccfa4e7ece41";

        public static string Decrypt(string encryptText)
        {
            return DecryptCore(encryptText, Decryptor.Key);
        }
        public static int DecryptInt(string encryptText)
        {
            return Convert.ToInt32(DecryptCore(encryptText, Decryptor.Key));
        }
        private static string DecryptCore(string text, string key)
        {
            text = text.Replace("ivj_jvi", " ").Replace("ivjpljvi", "+").Replace("ivjeqjvi", "=").Replace("ivjsljvi", "/").Replace("%3d", "=");
            byte[] byteArray = Convert.FromBase64String(text);

            MD5CryptoServiceProvider cryptoService = new MD5CryptoServiceProvider();

            byte[] keyArray = cryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            cryptoService.Clear();

            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

            tripleDES.Key = keyArray;

            tripleDES.Mode = CipherMode.ECB;

            tripleDES.Padding = PaddingMode.PKCS7;

            byte[] resultArray = tripleDES.CreateDecryptor().TransformFinalBlock(byteArray, 0, byteArray.Length);

            tripleDES.Clear();

            string returnText = UTF8Encoding.UTF8.GetString(resultArray);

            return returnText;
        }
        //private static string DecryptCore(string text, string key)
        //{
        //    text = Base64Decode(text);
        //    //text = text.Replace("_", " ").Replace("-", "+").Replace("*", "/") + "=";
        //    byte[] byteArray = Convert.FromBase64String(text);

        //    MD5CryptoServiceProvider cryptoService = new MD5CryptoServiceProvider();

        //    byte[] keyArray = cryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

        //    cryptoService.Clear();

        //    TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();

        //    tripleDES.Key = keyArray;

        //    tripleDES.Mode = CipherMode.ECB;

        //    tripleDES.Padding = PaddingMode.PKCS7;

        //    byte[] resultArray = tripleDES.CreateDecryptor().TransformFinalBlock(byteArray, 0, byteArray.Length);

        //    tripleDES.Clear();

        //    string clearPassword = UTF8Encoding.UTF8.GetString(resultArray);

        //    return clearPassword;
        //}
        //public static string Base64Decode(string base64EncodedData)
        //{
        //    var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        //    return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //}
    }
}
