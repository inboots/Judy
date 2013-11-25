using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Judy
{
    public partial class Login:System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text == JudyLib.Config.PASSWORD)
            {
                if (chkRemeberMe.Checked)
                {
                    int cv = JudyLib.Config.GetCookieTimeout;
                    HttpCookie cookie = new HttpCookie("login", "1");
                    cookie.Expires = DateTime.Now.AddSeconds(cv);
                    Response.Cookies.Add(cookie);
                }
                Session["login"] = "1";
               
                Response.Redirect("Index.aspx");
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "err", "alert('登录失败')", true);
            }
        }

      
    }
}