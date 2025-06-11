using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp._query
{
    /// <summary>
    /// GetUserMenu 的摘要描述
    /// </summary>
    public class GetUserMenu : cyc.Page.BaseHandler
    {
        protected override void DoHandler(HttpContext context)
        {
            //cyc.UserInfo oUser = (cyc.UserInfo)context.Session["uid"];
            MasterCont oCont = new MasterCont() { UserName = oUser.User.Name, UserDept = oUser.Dept.Name, List = cyc.Comm.Login.GetUserMenu(oUser) };
            if (!string.IsNullOrEmpty(context.Request.QueryString["app"]) && cyc.Comm.Check.IsInteger(context.Request.QueryString["app"]))
            {
                var prog = cyc.Global.SysProg.List.FirstOrDefault(p => p.ID == Convert.ToInt16(context.Request.QueryString["app"]));
            }
            context.Response.Write(this.SerializeObject(oCont));
        }

        protected override BaseHandlerOption SetBaseOption()
        {
            return new BaseHandlerOption() { Session = true };
        }

        /// <summary>
        /// 主頁面載入回傳資訊
        /// </summary>
        private class MasterCont
        {
            public string UserName { get; set; }
            public string UserDept { get; set; }
            public List<cyc.UIMenuMain> List { get; set; }
        }
    }
}