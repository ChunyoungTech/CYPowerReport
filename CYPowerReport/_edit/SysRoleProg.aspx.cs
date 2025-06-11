using cyc.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._edit
{
    public partial class SysRoleProg : cyc.Page.BasePageEdit
    {
        int iID = 0;
        #region #繼承
        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(Request.QueryString["pa"]);
            base.OnLoad(e);
        }
        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa" };
        }

        protected override void LoadData()
        {
            bindGrid();
        }

        protected override void SaveData()
        {
            List<cyc.SysRoleProg> insDataP = new List<cyc.SysRoleProg>(), delDataP = new List<cyc.SysRoleProg>();
            List<cyc.SysRoleProgSub> insDataS = new List<cyc.SysRoleProgSub>(), delDataS = new List<cyc.SysRoleProgSub>();

            var oldDataP = cyc.Global.SysRoleProg.List.Where(p => p.RoleID == iID);
            var oldDataS = cyc.Global.SysRoleProgSub.List.Where(p => p.RoleID == iID);
            List<cyc.SysRoleProg> newDataP = new List<cyc.SysRoleProg>();
            List<cyc.SysRoleProgSub> newDataS = new List<cyc.SysRoleProgSub>();

            foreach (GridViewRow gRow in GridView1.Rows)
            {
                if (gRow.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkID = (CheckBox)(gRow.FindControl("chkID"));
                    HiddenField hidID = (HiddenField)(gRow.FindControl("hidMainID"));
                    CheckBoxList chkList = (CheckBoxList)(gRow.FindControl("chkSubList"));
                    int pID = Convert.ToInt32(hidID.Value);

                    if (chkID.Checked)
                        newDataP.Add(new cyc.SysRoleProg() { RoleID = iID, ProgID = pID, isAllSub = true });
                    else if (chkList.SelectedValue.Length > 0)
                    {
                        newDataP.Add(new cyc.SysRoleProg() { RoleID = iID, ProgID = pID, isAllSub = false });
                        foreach (ListItem item in chkList.Items.Cast<ListItem>().Where(li => li.Selected))
                            newDataS.Add(new cyc.SysRoleProgSub() { RoleID = iID, ProgID = pID, SubID = Convert.ToInt32(item.Value) });
                    }
                }
            }

            foreach (var n in newDataP)
                if (!oldDataP.Any(p => p.ProgID == n.ProgID && p.isAllSub == n.isAllSub)) { insDataP.Add(n); }

            foreach (var o in oldDataP)
                if (!newDataP.Any(p => p.ProgID == o.ProgID && p.isAllSub == o.isAllSub)) { delDataP.Add(o); }

            foreach (var n in newDataS)
                if (!oldDataS.Any(p => p.ProgID == n.ProgID && p.SubID == n.SubID)) { insDataS.Add(n); }

            foreach (var o in oldDataS)
                if (!newDataS.Any(p => p.ProgID == o.ProgID && p.SubID == o.SubID)) { delDataS.Add(o); }

            if (insDataP.Count > 0 || insDataS.Count > 0 || delDataP.Count > 0 || delDataS.Count > 0)
            {
                using (cyc.DB.SqlDapperConn oDB = new cyc.DB.SqlDapperConn(oResult, "", true))
                {
                    if (oResult.Success && delDataP.Count > 0)
                        oDB.Execute("delete from SysRoleProg where RoleID=@RoleID and ProgID=@ProgID and isAllSub=@isAllSub", delDataP);

                    if (oResult.Success && insDataP.Count > 0)
                        oDB.Execute("insert into SysRoleProg (RoleID,ProgID,isAllSub) values (@RoleID,@ProgID,@isAllSub)", insDataP);

                    if (oResult.Success && delDataS.Count > 0)
                        oDB.Execute("delete from SysRoleProgSub where RoleID=@RoleID and ProgID=@ProgID and SubID=@SubID", delDataS);

                    if (oResult.Success && insDataS.Count > 0)
                        oDB.Execute("insert into SysRoleProgSub (RoleID,ProgID,SubID) values (@RoleID,@ProgID,@SubID)", insDataS);

                    oDB.ResultTransaction();

                    if (oResult.Success)
                    {
                        cyc.Global.SysRoleProg.Init(oDB, true);
                        cyc.Global.SysRoleProgSub.Init(oDB, true);
                    }
                }
            }
        }

        #endregion

        private void bindGrid()
        {
            this.GridView1.DataSource = cyc.Global.SysProg.List;
            this.GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkID = (CheckBox)(e.Row.FindControl("chkID"));
                HiddenField hidID = (HiddenField)(e.Row.FindControl("hidMainID"));
                CheckBoxList chkList = (CheckBoxList)(e.Row.FindControl("chkSubList"));
                if (chkID != null && hidID != null && chkList != null && int.TryParse(hidID.Value, out int pID))
                {
                    chkID.Checked = cyc.Global.SysRoleProg.List.Any(p => p.RoleID == iID && p.ProgID == pID && p.isAllSub);
                    
                    chkList.DataSource = cyc.Global.SysProgSub.List.Where(p => p.UpperID == pID && p.isShow);
                    chkList.DataBind();

                    var subList = cyc.Global.SysRoleProgSub.List.Where(p => p.RoleID == iID && p.ProgID == pID);
                    if (subList.Count() > 0)
                    {
                        foreach (ListItem item in chkList.Items)
                            item.Selected = subList.Any(p => p.SubID == Convert.ToInt16(item.Value));
                    }
                }
            }
        }
    }
}