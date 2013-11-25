using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI;

namespace Judy
{
    public class BasePage : System.Web.UI.Page
    {

        /// <summary>
        /// 验证登陆
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInitComplete(EventArgs e)
        {
            //base.OnInitComplete(e);
            bool flag = false;
            if (Session["login"] == null)
            {
                if (Request.Cookies["login"] != null)
                {
                    if (Request.Cookies["login"].Value == "1")
                    {
                        flag = true;

                    }
                }
            }
            else
            {
                if (Session["login"].ToString() == "1") flag = true;
            }
            if (!flag) Response.Redirect("Login.aspx");
        }

        ///// <summary>
        ///// 去掉头部空白行
        ///// </summary>
        ///// <param name="writer"></param>
        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //    StringWriter sw = new StringWriter();
        //    System.Web.UI.HtmlTextWriter HtmlWriter = new HtmlTextWriter(sw);
        //    //将内容渲染到HtmlWriter中
        //    base.Render(HtmlWriter);
        //    HtmlWriter.Flush();
        //    HtmlWriter.Close();
        //    //获取内容
        //    string html = sw.ToString();
        //    //移除空白行
        //    html = html.Trim();
        //    Response.Write(html+"<!--"+DateTime.Now.ToString()+"-->");
        //}

        protected override void OnError(EventArgs e)
        {

            HttpContext ctx = HttpContext.Current;
            Exception exception = ctx.Server.GetLastError();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head");
            sb.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1\" />");
            sb.AppendLine("<title>Error</title>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h3>:( SOMETHING ERROR</h3>");
            sb.AppendLine("<br/><p>" + exception.Message + "</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            string errorString = sb.ToString();

            ctx.Response.Write(errorString);

            ctx.Server.ClearError();

            base.OnError(e);
        }
    }
}