using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Judy
{
    public partial class Index :BasePage
    {
        private int _CurrentPage=1;
        private int _MaxPage;


        protected void Page_Load(object sender, EventArgs e)
        {
            GetCurrentPage();
            if (!IsPostBack)
            {
                BindData();
            }
            
        }

        void GetCurrentPage()
        {
            int p = 0;
            int.TryParse(Request.QueryString["page"], out p);
            if (p > 0)
            {
                this._CurrentPage = p;
            }
            
            double fp = (double)JudyCore.PHPBusiness.GetTotalCount() / (double)JudyLib.Config.PageSize;
            _MaxPage = (int)Math.Ceiling(fp);
 
        }

        void BindData()
        {
            if (_CurrentPage <= 1)
            {
                btnPrev.Enabled = false;
            }

            if (_CurrentPage >= _MaxPage)
            {
                btnNext.Enabled = false;
            }
            List<JudyCore.Model> dt = JudyCore.PHPBusiness.GetPage(_CurrentPage);//JudyCore.Business.GetPageTable(_CurrentPage);
            Twitters.DataSource = dt;
            Twitters.DataBind();
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int prev = this._CurrentPage-1;
            if (prev >= 0)
            {
                Response.Redirect("Index.aspx?page=" + prev);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

            int next = this._CurrentPage + 1;
            if (next <= this._MaxPage)
            {
                Response.Redirect("Index.aspx?page=" + next);
            }
        }
    }
}