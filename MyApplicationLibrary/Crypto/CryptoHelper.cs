using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
namespace MyApplicationLibrary.Crypto
{
    public sealed class CryptoHelper
    {
        private const int ITERATOR_COUNT = 1024;
        private const string SALT = "";
        /// <summary>
        /// 
        /// 密码加密  
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static string PasswordCrypto(string username, string password)
        {
            return Crypto(username + password);
        }
        /// <summary>
        /// 加密算法
        /// </summary>
        /// <param name="str"></param>
        /// <param name="iteratorCount"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Crypto(string str, int iteratorCount = ITERATOR_COUNT, string salt = "")
        {
            byte[] byteArray = UTF32Encoding.UTF32.GetBytes(str + salt);
            MD5 md5 = MD5.Create("md5");
            for (int i = 0; i <= iteratorCount; i++)
            {
                byteArray = md5.ComputeHash(byteArray);
            }
            return Convert.ToBase64String(byteArray);
        }
        /// <summary>
        /// 盐加密
        /// </summary>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string SaltCrypto(string salt)
        {
            return Crypto(salt,2);
        }
        
    }
}
