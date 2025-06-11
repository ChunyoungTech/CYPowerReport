using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;
using System.Data;

namespace WebApp._edit
{
    public partial class SysRoleEdit : cyc.Page.BasePageEdit
    {
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
                var role = cyc.Global.SysRole.List.FirstOrDefault(p => p.ID == iID);
                if (role != null)
                {
                    this.txtName.Text = role.Name;
                    this.ddlLevelNo.SelectedValue = role.LevelNo.ToString();
                    this.chkDefault.Checked = role.IsDefault;
                    this.chkEnabled.Checked = role.Enabled;
                }
                else
                    oResult.Error("查無資料");
            }
        }

        protected override void SaveData()
        {
            var oRole = new cyc.SysRole()
            {
                ID = iID,
                Name = txtName.Text.Trim(),
                LevelNo = Convert.ToInt32(ddlLevelNo.SelectedValue),
                Enabled = chkEnabled.Checked,
                IsDefault = chkDefault.Checked,
                User = bUser.User.ID
            };

            string sSQL = "insert into SysRole (Name,LevelNo,Enabled,isDefault,c_user) values (@Name,@LevelNo,@Enabled,@IsDefault,@User)";
            if (iID != 0) { sSQL = "update SysRole set Name=@Name,LevelNo=@LevelNo,Enabled=@Enabled,isDefault=@IsDefault,u_user=@User,u_date=getdate() where id=@ID"; }

            oRole.ID = dDB.Execute(sSQL, oRole, iID);
            if (oResult.Success)
            {
                var oData = cyc.Global.SysRole.List.FirstOrDefault(p => p.ID == iID);
                if (oData == null)
                    cyc.Global.SysRole.List.Add(oRole);
                else
                    cyc.Global.CopyObjectValues<cyc.SysRole>(oRole, oData);
            }

            //bPara.Object = oRole;
            //if (iID == 0)
            //{
            //    bPara.Command = "insert into SysRole (Name,LevelNo,Enabled,isDefault,c_user) values (@Name,@LevelNo,@Enabled,@IsDefault,@User)";
            //    oRole.ID = cyc.DB.Shared.ExecuteDapper(bPara, true, bDB);
            //    if (oResult.Success && oRole.ID > 0) { cyc.Global.SysRole.List.Add(oRole); }
            //}
            //else
            //{
            //    bPara.Command = "update SysRole set Name=@Name,LevelNo=@LevelNo,Enabled=@Enabled,isDefault=@IsDefault,u_user=@User,u_date=getdate() where id=@ID";
            //    cyc.DB.Shared.ExecuteDapper(bPara, false, bDB);
            //    if (oResult.Success)
            //    {
            //        var oData = cyc.Global.SysRole.List.FirstOrDefault(p => p.ID == iID);
            //        if (oData != null) { cyc.Global.CopyObjectValues<cyc.SysRole>(oRole, oData); }
            //    }
            //}
        }

        #endregion
    }
}