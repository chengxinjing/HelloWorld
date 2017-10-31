using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MyApplicationLibrary.Strings
{
    public class StringUtils
    {

        /// <summary>
        /// 判断string是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(string str)
        {

            return string.IsNullOrEmpty(str);
        }
       
        /// <summary>
        /// 判断string 不为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotEmpty(string str)
        {

            return !IsEmpty(str);
        }
       
        /// <summary>
        /// 将string 转换成 数字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int StringConvertToInt(string str)
        {
            return int.Parse(str);
        }
        /// <summary>
        /// 生成UUID
        /// </summary>
        /// <returns></returns>
        public static string GetUUID()

        {
            byte[] byteArray = UTF8Encoding.UTF8.GetBytes(DateTime.Now.Ticks.ToString());
            MD5 md5 = MD5.Create("md5");
            return Convert.ToBase64String(md5.ComputeHash(byteArray));
           
        }

    }

}
