using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace JudyCore
{
    public class PHPBusiness
    {

        /// <summary>
        /// 外部PHPAPI
        /// </summary>
        private static readonly string API = System.Configuration.ConfigurationManager.AppSettings["API"].ToString();
        private static readonly string DB = System.Configuration.ConfigurationManager.AppSettings["DB"].ToString();
        private static readonly string USERAGENT = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.31 (KHTML, like Gecko) Chrome/26.0.1410.64 Safari/537.31";
        /// <summary>
        /// 创建安全连接字符串类型
        /// </summary>
        private enum SECURETYPE
        {
            Add,
            Insert,
            Update,
            Delete
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static int Add(Model m)
        {
            //Http.WebClient wc = new Http.WebClient();
            int x = 0;
            string sql=GetSecureSQLString(m,SECURETYPE.Add);
            string json ="data={\"db\":\""+DB+"\",\"sql\":\""+sql+"\",\"type\":\"insertorupdate\"}";
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            string result = Http.Post(API, jsonBytes, USERAGENT);

            if (result.Contains("-")) x = 0;
            else if (result.Contains("1")) x = 1;

            return x;
        }


        public static int GetTotalCount()
        {
            int c = 0;
            try
            {
                string json = "data={\"db\":\""+DB+"\",\"sql\":\"select count(*) as total from judy\",\"type\":\"query\"}";
                byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
                string result = Http.Post(API, jsonBytes, USERAGENT);
                result = result.Replace("[", "").Replace("]", "");

                CommonEntity cm = LitJson.JsonMapper.ToObject<CommonEntity>(result);
                c = int.Parse(cm.total);

            }
            catch
            {
                c = -1;
            }

            return c;

        }


        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="page">页</param>
        /// <returns></returns>
        public static List<Model> GetPage(int page)
        {
            List<Model> rawData = new List<Model>();
            int pageSize = JudyLib.Config.PageSize;
            int start = pageSize * (page - 1);
            string sql = "select * from judy order by jId desc limit " + start + "," + pageSize + "";
            //System.Data.DataTable dt = MYSQLHelper.Query(sql, null).Tables[0];
            string json = "data={\"db\":\""+DB+"\",\"sql\":\"" + sql + "\",\"type\":\"query\"}";
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            string result = Http.Post(API, jsonBytes, USERAGENT);


            rawData = LitJson.JsonMapper.ToObject<List<Model>>(result);

            List<Model> data = new List<Model>();

            AESEncryptor aes =new AESEncryptor(JudyLib.Config.SYSKEY,AESBits.BITS128);

            foreach (Model item in rawData)
            {
                Model m = new Model();
                m.jaddress = aes.Decrypt(item.jaddress);
                m.jdevice = aes.Decrypt(item.jdevice);
                m.jtext = aes.Decrypt(item.jtext);
                m.jdatetime = item.jdatetime;
                m.jid = item.jid;
                m.jlatitude = item.jlatitude;
                m.jlongitude = item.jlongitude;

                data.Add(m);
            }

            return data;
        }


        /// <summary>
        /// 创建安全字符串
        /// </summary>
        /// <param name="m"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private static string GetSecureSQLString(Model m,SECURETYPE type)
        {
            string result = string.Empty;
            AESEncryptor aes = new AESEncryptor(JudyLib.Config.SYSKEY, AESBits.BITS128);

            string text = aes.Encrypt(m.jtext);
            string device = aes.Encrypt(m.jdevice);
            string address = aes.Encrypt(m.jaddress);

            switch (type)
            {
                case SECURETYPE.Add:

                    string sql = "INSERT INTO `judy` (`jtext`, `jlongitude`, `jlatitude`,`jaddress`, `jdevice`) VALUES ('" + text + "'," + m.jlongitude + ",'" + m.jlatitude + "','" + address + "','" + device + "')";
                    result = sql;
                    break;
                default: break;
            }
            return result;
        }
    }
}
