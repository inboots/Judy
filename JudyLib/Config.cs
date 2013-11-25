using System;
using System.Collections.Generic;
using System.Text;

namespace JudyLib
{
    public class Config
    {
        /// <summary>
        /// 密码
        /// </summary>
        public const string PASSWORD = "*****";
        /// <summary>
        /// 加密安全密钥
        /// </summary>
        public const string SYSKEY = "*****";

        /// <summary>
        /// Cookie有效时间
        /// </summary>
        public static int GetCookieTimeout
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["COOKIETIMEOUT"].ToString());
            }
        }

        /// <summary>
        /// 分页大小
        /// </summary>
        public static int PageSize
        {
            get
            {
                 return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PAGESIZE"].ToString());
            }
        }
    }
}
