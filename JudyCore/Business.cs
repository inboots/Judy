//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace JudyCore
//{
//    public class Business
//    {
        

//        /// <summary>
//        /// 添加一条记录
//        /// </summary>
//        /// <param name="m"></param>
//        /// <returns></returns>
//        public static int Add(Model m)
//        {
//            string sql = "INSERT INTO `judy` (`t_text`, `t_lng`, `t_lat`,`t_address`, `t_device`) VALUES ('" + m.Text + "'," + m.Longitude + ",'" + m.Latitude + "','"+m.Address+"','" + m.Device + "')";
//            return MYSQLHelper.Execute(sql, null);
//        }

//        /// <summary>
//        /// 获取记录总数
//        /// </summary>
//        /// <returns></returns>
//        public static int GetTotalCount()
//        {
//            int n = -1;
//            string sql = "select count(*) from judy";
//            System.Data.DataTable dt = MYSQLHelper.Query(sql, null).Tables[0];
//            if (dt.Rows.Count > 0)
//            {
//                string str_len = dt.Rows[0][0].ToString();
//                n=int.Parse(str_len);
//            }
//            return n;

//        }

//        /// <summary>
//        /// 获取分页数据
//        /// </summary>
//        /// <param name="page">页</param>
//        /// <returns></returns>
//        public static List<Model> GetPageList(int page)
//        {
//            List<Model> list = new List<Model>();
//            int pageSize = JudyLib.Config.PageSize;
//            int start = pageSize*(page-1);
//            string sql = "select * from judy order by t_id desc limit " + start + "," + pageSize + "";
//            System.Data.DataTable dt = MYSQLHelper.Query(sql,null).Tables[0];
//            if (dt.Rows.Count > 0)
//            {
//                foreach (System.Data.DataRow dr in dt.Rows)
//                {
//                    Model m = new Model();
//                    m.Datetime = DateTime.Parse(dr["t_datetime"].ToString());
//                    m.Device = dr["t_device"].ToString();
//                    m.Id = int.Parse(dr["t_id"].ToString());
//                    m.Latitude = double.Parse(dr["t_lat"].ToString());
//                    m.Longitude = double.Parse(dr["t_lng"].ToString());
//                    m.Text = dr["t_text"].ToString();
//                    m.Address = dr["t_address"].ToString();

//                    list.Add(m);
//                }
//            }
//            return list;
//        }

//        /// <summary>
//        /// 获取分页数据
//        /// </summary>
//        /// <param name="page">页</param>
//        /// <returns></returns>
//        public static System.Data.DataTable GetPageTable(int page)
//        {
//            int pageSize = JudyLib.Config.PageSize;
//            int start = pageSize * (page - 1);
//            string sql = "select * from judy order by t_id desc limit " + start + "," + pageSize + "";
//            System.Data.DataTable dt = MYSQLHelper.Query(sql, null).Tables[0];
//            return dt;
//        }
//    }
//}
