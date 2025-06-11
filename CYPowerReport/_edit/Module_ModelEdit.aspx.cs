using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;
using Dapper;
using System.Data;

namespace WebApp._edit
{
    public class Module_Model
    {

        public int MM_SEQ_ID { get; set; }
        public string MM_Module_Model { get; set; }
        public string MM_Kw_Pis { get; set; }
        public DateTime? MM_StopDate { get; set; }
    }

    public partial class Module_ModelEdit : cyc.Page.BasePageEdit
    {
        //CutomerData oData;
        protected int iID = 0;
        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(ViewState["ID"] ?? Request.QueryString["pa"]);
            base.OnLoad(e);
        }
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {


            }
            base.OnInit(e);
        }

        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "app,pa" };
        }

        protected override void LoadData()
        {
            if (iID != 0)
            {
                var oData = dDB.QueryOne<Module_Model>("select * from Module_Model where MM_SEQ_ID=@ID", new { ID = iID });
                if (oData == null)
                    oResult.Error("查無資料");
                else
                {
                    this.txtModule_Model.Text = oData.MM_Module_Model.ToString();
                    if (oData.MM_Kw_Pis != null) this.txtKw_Pis.Text = oData.MM_Kw_Pis.ToString();
                    if (oData.MM_StopDate != null) this.ucMM_StopDate.Text = oData.MM_StopDate.ToString();
                }
            }
        }

        protected override void SaveCheck()
        {
            string sql = "select MM_Module_Model from Module_Model where MM_Module_Model=@Name";
            if (iID != 0) { sql += " and MM_SEQ_ID<>@ID"; }
            var x = dDB.QueryOne<dynamic>(sql, new { Name = txtModule_Model.Text.Trim(), ID = iID });
            if (x != null) { oResult.Error("已存在相同[模組型號名稱]資料;"); }

        }

        protected override void SaveData()
        {
            var oData = new Module_Model
            {
                MM_SEQ_ID = iID,
                MM_Module_Model = txtModule_Model.Text.Trim(),
                MM_Kw_Pis = txtKw_Pis.Text.Trim()
            };
            if (ucMM_StopDate.Text != "") { oData.MM_StopDate = Convert.ToDateTime(ucMM_StopDate.Text); }

            oData.MM_SEQ_ID = dDB.Execute(cyc.DB.Shared.GetEditSQL("Module_Model", "MM_Module_Model,MM_Kw_Pis,MM_StopDate;;MM_SEQ_ID", iID == 0), oData, iID);

            if (oResult.Success)
                cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(Request.QueryString["app"]), ExecType = iID == 0 ? "insert" : "update", ExecDesc = string.Format("模組型號資料，Module_Model ID={0} MM_Module_Model={1}", oData.MM_SEQ_ID, oData.MM_Module_Model), UserID = bUser.User.ID, KeyValue = oData.MM_SEQ_ID.ToString() }, dDB);
        }

        #endregion

    }
}