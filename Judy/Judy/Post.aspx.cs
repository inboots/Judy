using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Judy
{
    public partial class Post :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            JudyCore.Model m = new JudyCore.Model();
            m.jdevice = hidDevc.Value;
            if (hidLat.Value.Length > 0)
            {
                double t = 0;
                double.TryParse(hidLat.Value, out t);
                m.jlatitude = t.ToString();
            }
            else
            {
                m.jlatitude = "0";
            }

            if (hidLng.Value.Length > 0)
            {

                double t = 0;
                double.TryParse(hidLng.Value, out t);
                m.jlongitude = t.ToString();

            }
            else
            {
                m.jlongitude = "0";
            }
            
            m.jaddress = hidAddress.Value;
            m.jtext = txtTwitter.Text;

            int x = JudyCore.PHPBusiness.Add(m);
          
            if (x > 0)
            {
                this.Response.Redirect("Index.aspx");
            }
            else
            {
                this.ClientScript.RegisterStartupScript(this.GetType(), "err", "alert('发布失败')", true);
            }
        }
    }
}