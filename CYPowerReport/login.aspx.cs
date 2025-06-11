using CyLicenseKey;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApp._security;


namespace WebApp
{

    public partial class login : System.Web.UI.Page
    {
        cyc.ExeResult oResult = new cyc.ExeResult();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                TextBox1.Focus();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ChunyoungKey chunyoungKey = new ChunyoungKey();//宣告ChunyoungKey 物件
            string macs = "";
            bool isValid = false;
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            List<string> macList = new List<string>();
            foreach (var nic in nics)
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    macList.Add(nic.GetPhysicalAddress().ToString());
                }
            }
            string licNo = cyc.Comm.SysQuery.GetAppSettingValue("LicNo");
            foreach (string mac in macList)
            {


                string macdash = mac.Substring(0, 2) + "-" + mac.Substring(2, 2)
                      + "-" + mac.Substring(4, 2) + "-" + mac.Substring(6, 2)
                      + "-" + mac.Substring(8, 2) + "-" + mac.Substring(10, 2)
                      ;
                macs += macdash + ",\r\n";
                if (chunyoungKey.CheckLicense(licNo, macdash))
                {
                    isValid = true;
                }
            }

            if (!isValid)
            {
                Label1.Text = $"系統授權異常，無法登入，mcs:\r\n" + String.Join("\r\n", macs);

                return;
            }


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
                    //if (!string.IsNullOrEmpty(Request.QueryString["rtn"]))
                    //    Response.Redirect(Server.UrlDecode(Request.QueryString["rtn"]));
                    if (!string.IsNullOrEmpty(Request.QueryString["rtn"]))
                    {
                        string RtnUrl = Server.UrlDecode(Request.QueryString["rtn"]);
                        int idx = RtnUrl.IndexOf("?");
                        if (idx > 0)
                        {
                            var UrlRart = RtnUrl.Split('/');
                            UrlRart[UrlRart.Length - 1] = RtnUrl.Substring(idx);
                            RtnUrl = string.Join("/", UrlRart);
                        }

                        Response.Redirect(RtnUrl);
                    }

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