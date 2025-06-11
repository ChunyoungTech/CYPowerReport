using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;

namespace WebApp._edit
{
    public partial class SysDeptEdit : cyc.Page.BasePageEdit
    {
        protected int iID = 0;
        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(ViewState["ID"] ?? Request.QueryString["pa"]);
            base.OnLoad(e);
        }
        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa" };
        }

        protected override void LoadData()
        {
            if (iID != 0)
            {
                var dept = cyc.Global.SysDept.List.FirstOrDefault(p => p.ID == iID);
                if (dept != null)
                {
                    this.txtName.Text = dept.Name;
                    this.txtCode.Text = dept.Code;
                    this.ucDept.DeptID = dept.UpperID;
                }
                else
                    oResult.Error("查無資料");
            }
        }

        protected override void SaveCheck()
        {
            if (iID != 0)
            {
                var old = cyc.Global.SysDept.List.FirstOrDefault(p => p.ID == iID);
                if (ucDept.DeptRange(old.ID).Any(p => p == ucDept.DeptID))
                {
                    oResult.Error("[上級單位]不可指定自己或下屬單位");
                }
            }
        }

        protected override void SaveData()
        {
            var oData = new cyc.SysDept { ID = iID, Code = txtCode.Text.Trim(), Name = txtName.Text.Trim(), UpperID = ucDept.DeptID };
            oData.ID = dDB.Execute(cyc.DB.Shared.GetEditSQL("SysDept", "Code,Name,UpperID;;ID", iID == 0), oData, iID);

            if (oResult.Success)
            {
                ViewState["ID"] = oData.ID;
                var dept = cyc.Global.SysDept.List.FirstOrDefault(p => p.ID == oData.ID);
                if (dept != null)
                {
                    dept.Code = oData.Code;
                    dept.Name = oData.Name;
                    dept.UpperID = oData.UpperID;
                }
                else
                    cyc.Global.SysDept.List.Add(oData);
            }
        }

        #endregion
    }
}