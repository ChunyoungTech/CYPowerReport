using cyc.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._edit
{
    public partial class SysRoleUser : cyc.Page.BasePageEdit
    {
        //protected override void OnInit(EventArgs e)
        //{
        //    base.OnInit(e);
        //    if (!cyc.Comm.Login.CheckSession()) { Session["invalid"] = "未登入或逾時登出"; Response.Redirect("~/invalid.aspx"); }
        //}
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(Request.QueryString["pa"]))
        //    {
        //        bPara.Command = "select A.UserID,B.ID,B.Name from (select distinct UserID from SysRoleUser where RoleID=@ID)A inner join SysUser B on A.UserID=B.ID";
        //        bPara.Parameter = new List<SqlParameter>() { new SqlParameter("ID", Request.QueryString["pa"]) };
        //        bPara.Result = oResult;
        //        using (DataTable oDT = bDB.QueryDT(bPara))
        //        {
        //            if (oResult.Success)
        //            {
        //                this.ListBox2.DataSource = oDT;
        //                this.ListBox2.DataBind();
        //            }
        //        }
        //    }
        //}
        int iID = 0;

        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(Request.QueryString["pa"]);
            base.OnLoad(e);
        }
        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa", IsIntPa = true };
        }
        protected override void LoadData()
        {
            var oData = cyc.Global.SysRole.List.FirstOrDefault(p => p.ID == iID);
            if (oData != null)
            {
                var oList = dDB.QueryList<BaseObj>("select B.ID,B.Name from (select distinct UserID from SysRoleUser where RoleID=@ID)A inner join SysUser B on A.UserID=B.ID", new { ID = iID });
                if (oResult.Success)
                    ltlSelect.Text = string.Join("", oList.Select(p => string.Format("<option value='{0}'>{1}</option>", p.ID, p.Name)));
            }
            else
                oResult.Error("查無資料");
        }
        protected override void SaveData()
        {
            var nUser = hidSelect.Value.Split(',').Where(p => cyc.Comm.Check.IsInteger(p)).Select(p => Convert.ToInt32(p));

            var dList = from ls in cyc.Global.SysRoleUser.List.Where(p => p.RoleID == iID)
                        join lu in nUser on ls.UserID equals lu into UU
                        from lu in UU.DefaultIfEmpty()
                        where lu == 0
                        select ls;

            var iList = from lu in nUser
                        join ls in cyc.Global.SysRoleUser.List.Where(p => p.RoleID == iID) on lu equals ls.UserID into SS
                        from ls in SS.DefaultIfEmpty()
                        where ls == null
                        select new cyc.SysRoleUser { RoleID = iID, UserID = lu };

            if (dList.Count() > 0 || iList.Count() > 0)
            {
                using (var oDB = new cyc.DB.SqlDapperConn(oResult, null, true))
                {
                    try
                    {
                        if (oResult.Success && iList.Count() > 0)
                            oDB.Execute("insert into SysRoleUser (RoleID,UserID) values (@RoleID,@UserID)", iList);
                        if (oResult.Success && dList.Count() > 0)
                            oDB.Execute("delete from SysRoleUser where RoleID=@RoleID and UserID=@UserID", dList);
                    }
                    catch (Exception ex) { cyc.Global.WriteSysError(ex.Message, oResult); }

                    oDB.ResultTransaction();
                }

                if (oResult.Success)
                {
                    LoadData();
                    cyc.Global.SysRoleUser.List.RemoveAll(p => p.RoleID == iID);
                    cyc.Global.SysRoleUser.List.AddRange(dDB.QueryList<cyc.SysRoleUser>("select * from SysRoleUser where RoleID=@ID", new { ID = iID }));
                }
            }
        }

        class BaseObj
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }
    }
}