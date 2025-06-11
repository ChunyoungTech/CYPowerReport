using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp
{
    public partial class backdoor : System.Web.UI.Page
    {
        cyc.ExeResult oResult = new cyc.ExeResult();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                TextBox1.Focus();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (TextBox1.Text.Trim().Length > 0 && TextBox2.Text.Trim().Length > 0)
            {
                //cyc.DB.DapperDBPara oPara = new cyc.DB.DapperDBPara()
                //{
                //    Command = "select * from SysUser where Code=@Code and Password=@PW",
                //    Parameter = new { Code = TextBox1.Text.Trim(), PW = cyc.Comm.Login.CryptoPWD(TextBox2.Text.Trim()) },
                //    Result = oResult
                //};

                //var user = cyc.Global.gDB.Query<cyc.SysUser>(oPara);
                var user = cyc.Comm.Login.GetUser(TextBox1.Text, TextBox2.Text);

                //if (oResult.Success && user != null && user.Count() > 0)
                if (oResult.Success && user != null)
                {
                    //cyc.UserInfo oUser = new cyc.UserInfo() { User = (cyc.SysUser)user.First().Clone() } ;
                    //cyc.UserInfo oUser = new cyc.UserInfo() { User = (cyc.SysUser)user.Clone() };
                    //oUser.Dept = (cyc.SysDept)cyc.Global.SysDept.FirstOrDefault(p => p.ID == oUser.User.DeptID).Clone();
                    //oUser.UserRole = (from lsRU in cyc.Global.SysRoleUser
                    //                  join lsR in cyc.Global.SysRole on lsRU.RoleID equals lsR.ID
                    //                  where lsRU.UserID == oUser.User.ID 
                    //                  select lsR).ToList();
                    //if (oUser.UserRole == null || oUser.UserRole.Count() == 0)
                    //    oUser.UserRole = (from lsR in cyc.Global.SysRole where lsR.isDefault == true select lsR).ToList();

                    var oUser = cyc.Comm.Login.GetUserInfo(user);

                    Session["uid"] = oUser;
                    if (!string.IsNullOrEmpty(Request.QueryString["rtn"]))
                        Response.Redirect(Server.UrlDecode(Request.QueryString["rtn"]));
                    else
                    {
                        foreach (var m in cyc.Global.SysDir.List)
                        {
                            var x = from lsU in oUser.Roles
                                    join lsRP in cyc.Global.SysRoleProg.List on lsU equals lsRP.RoleID
                                    join lsP in cyc.Global.SysProg.List on lsRP.ProgID equals lsP.ID
                                    where lsP.DirID == m.ID && lsP.Enabled == true
                                    select new cyc.UIMenuItem() { ID = lsP.ID, Name = lsP.Name, Dir = lsP.Folder, Seq = lsP.Seq };
                            var z = x.GroupBy(o => o.ID).Select(o => o.FirstOrDefault()).OrderBy(p => p.Seq).FirstOrDefault();
                            if (z != null)
                                Response.Redirect(string.Format("~/{0}/?app={1}", z.Dir, z.ID.ToString()));
                        }

                    }
                    //Response.Redirect("index.aspx");
                    oResult.Error("無系統權限");
                }
                else
                {
                    oResult.Error("帳號或密碼輸入錯誤");
                }
            }
            else
            {
                oResult.Error("帳號、密碼均不可空白");
            }

            Label1.Text = oResult.Message;
        }
    }
}