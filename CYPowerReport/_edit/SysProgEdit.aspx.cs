using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dapper;
using cyc.Page;

namespace WebApp._edit
{
    public partial class SysProgEdit : cyc.Page.BasePageEdit
    {
        protected int iID = 0;
        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(ViewState["ID"] ?? Request.QueryString["pa"]);
            base.OnLoad(e);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                this.ddlDir.DataSource = cyc.Global.SysDir.List;
                this.ddlDir.DataBind();
            }
        }

        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa" };
        }

        protected override void LoadData()
        {
            var prog = cyc.Global.SysProg.List.FirstOrDefault(p => p.ID == iID);
            if (prog != null)
            {
                txtName.Text = prog.Name;
                ddlDir.SelectedValue = prog.DirID.ToString();
                chkEnabled.Checked = prog.Enabled;
                txtSeq.Text = prog.Seq.ToString();
            }
            else
                oResult.Error("查無資料");
        }

        protected override void SaveCheck()
        {
            string sMsg = "";
            if (txtName.Text.Trim().Length == 0)
                sMsg += "[功能名稱]不可空白;";
            if (!cyc.Comm.Check.IsInteger(txtSeq.Text.Trim()))
                sMsg += "[排序]必須是數字且不可空白;";
            if (sMsg.Length > 0)
                oResult.Error(sMsg);
        }

        protected override void SaveData()
        {
            var oProg = new ProgEdit()
            {
                ID = iID,
                Name = txtName.Text.Trim(),
                Enabled = chkEnabled.Checked,
                DirID = Convert.ToInt32(ddlDir.SelectedItem.Value),
                DirName = ddlDir.SelectedItem.Text,
                Seq = Convert.ToInt32(txtSeq.Text),
                UpdUser = bUser.User.ID
            };

            dDB.Execute("update SysProg set Name=@Name,DirID=@DirID,Enabled=@Enabled,Seq=@Seq,u_user=@UpdUser,u_date=getdate() where id=@ID", oProg);
            if (oResult.Success)
            {
                var oData = cyc.Global.SysProg.List.FirstOrDefault(p => p.ID == iID);
                if (oData != null)
                {
                    oProg.Folder = oData.Folder;
                    oProg.Path = oData.Path;
                    cyc.Global.CopyObjectValues<cyc.SysProg>(oProg, oData);
                }
            }
            //using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
            //{
            //    try
            //    {
            //        oDB.oConn.Execute("update SysProg set Name=@Name,DirID=@DirID,Enabled=@Enabled,Seq=@Seq,u_user=@UpdUser,u_date=getdate() where id=@ID", oProg);

            //        var data = cyc.Global.SysProg.List.FirstOrDefault(p => p.ID == Convert.ToInt32(this.hidID.Value));
            //        if (data != null)
            //        {
            //            oProg.Folder = data.Folder;
            //            oProg.Path = data.Path;
            //            cyc.Global.CopyObjectValues<cyc.SysProg>(oProg, data);
            //        }
            //    }
            //    catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult); }
            //}
        }

        #endregion

        class ProgEdit : cyc.SysProg
        {
            public int UpdUser { get; set; }
            //public DateTime UpdDate { get; set; }
        }
    }
}