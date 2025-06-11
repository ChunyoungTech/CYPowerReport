using cyc.Page;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;

namespace WebApp._edit
{
    public partial class SysUserEdit : cyc.Page.BasePageEdit
    {
        static string DefaultPWD = cyc.Comm.Login.CryptoPWD(cyc.Comm.SysQuery.GetSysSettingValue("DefaultPWD"));
        protected int iID = 0;
        #region #繼承
        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(ViewState["ID"] ?? Request.QueryString["pa"]);
            base.OnLoad(e);
        }
        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa" };
        }

        protected override void LoadData()
        {
            if (iID != 0)
            {
                var oUser = dDB.QueryOne<cyc.SysUser>("select * from SysUser where ID=@ID", new { ID = iID });
                if (oUser != null)
                {
                    this.txtName.Text = oUser.Name;
                    this.txtCode.Text = oUser.Code;
                    //this.txtPassword.Visible = false;
                    btnDefault.Visible = true;
                    this.ucDept.DeptID = oUser.DeptID;
                    this.chkEnabled.Checked = oUser.Enabled;
                    this.chkManager.Checked = oUser.isManager;
                }
                else
                    oResult.Error("查無資料");
            }
        }

        protected override void SaveData()
        {
            var oData = dDB.QueryOne<cyc.SysUser>("select * from SysUser where ID=@ID", new { ID = iID }) ?? new cyc.SysUser();
            oData.Code = txtCode.Text;
            oData.Name = txtName.Text;
            oData.DeptID = ucDept.DeptID;
            oData.Enabled = chkEnabled.Checked;
            oData.isManager = chkManager.Checked;
            if (oData.ID == 0) oData.Password = DefaultPWD;

            iID = dDB.Execute(cyc.DB.Shared.GetEditSQL("SysUser", "Code,Name,DeptID,Password,Enabled,isManager;;ID", iID == 0), oData, iID);
            if (oResult.Success) { ViewState["ID"] = iID; }
        }

        #endregion

        protected void btnDefault_Click(object sender, EventArgs e)
        {
            if (iID != 0) 
            {
                dDB.Execute("update SysUser set Password=@PW where ID=@ID", new { ID = iID, PW = DefaultPWD });
                ShowResult("回復成功", false, false);
            }
        }
    }
}