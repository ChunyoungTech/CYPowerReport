using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._edit
{
    public partial class open : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(string.IsNullOrEmpty(Request.QueryString["app"]) || string.IsNullOrEmpty(Request.QueryString["sub"]) || Session["uid"] == null))
            {
                cyc.UserInfo bUser = (cyc.UserInfo)Session["uid"];

                var r = (from lsU in bUser.Roles
                         join lsRS in cyc.Global.SysRoleProg.List on lsU equals lsRS.RoleID
                         join lsPS in cyc.Global.SysProgSub.List on lsRS.ProgID equals lsPS.UpperID
                         where lsPS.UpperID == Convert.ToInt16(Request.QueryString["app"]) && lsPS.ID == Convert.ToInt16(Request.QueryString["sub"])
                         select new { isAll = lsRS.isAllSub, Path = lsPS.Path }).FirstOrDefault();
                if (r != null && r.isAll) { Server.Transfer(r.Path); }

                var x = (from lsU in bUser.Roles
                         join lsP in cyc.Global.SysRoleProgSub.List on lsU equals lsP.RoleID
                         join lsPS in cyc.Global.SysProgSub.List on lsP.SubID equals lsPS.ID
                         where lsP.ProgID == Convert.ToInt16(Request.QueryString["app"]) && lsP.SubID == Convert.ToInt16(Request.QueryString["sub"])
                         select lsPS).FirstOrDefault();
                if (x != null) { Server.Transfer(r.Path); }
            }

            Session["invalid"] = "參數錯誤";
            Response.Redirect("~/invalid.aspx");
        }
    }
}