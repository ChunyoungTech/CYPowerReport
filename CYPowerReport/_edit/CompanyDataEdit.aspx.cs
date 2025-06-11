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
    public class CompanyData
    {

        public int COD_SEQ_ID { get; set; }
        public string COD_NAME { get; set; }
        public string COD_ADDRESS { get; set; }
        public string COD_Uniform_Numbers { get; set; }
        public string COD_Tel { get; set; }
        public string COD_EMail { get; set; }
        public string COD_Contact { get; set; }
        public string COD_TYPE { get; set; }
        public int Update_User { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public partial class CompanyDataEdit : cyc.Page.BasePageEdit
    {
        //CompanyData oData;
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
                var oData = dDB.QueryOne<CompanyData>("select * from CompanyData where COD_SEQ_ID=@ID", new { ID = iID });
                if (oData == null)
                    oResult.Error("查無資料");
                else
                {
                    this.txtCOD_NAME.Text = oData.COD_NAME.ToString();
                    if (oData.COD_ADDRESS != null) this.txtCOD_ADDRESS.Text = oData.COD_ADDRESS.ToString();
                    if (oData.COD_Uniform_Numbers != null) this.txtNumbers.Text = oData.COD_Uniform_Numbers.ToString();
                    if (oData.COD_Tel != null) this.txtCOD_Tel.Text = oData.COD_Tel.ToString();
                    if (oData.COD_EMail != null) this.txtCOD_EMail.Text = oData.COD_EMail.ToString();
                    if (oData.COD_Contact != null) this.txtCOD_Contact.Text = oData.COD_Contact.ToString();
                }
            }
        }

        protected override void SaveCheck()
        {
            string sql = "select COD_NAME from CompanyData where COD_NAME=@Name";
            if (iID != 0) { sql += " and COD_SEQ_ID<>@ID"; }
            var x = dDB.QueryOne<dynamic>(sql, new { Name = txtCOD_NAME.Text.Trim(), ID = iID });
            if (x != null) { oResult.Error("已存在相同[公司名稱]資料;"); }

            sql = "select COD_Uniform_Numbers from CompanyData where COD_Uniform_Numbers=@COD_Uniform_Numbers";
            if (iID != 0) { sql += " and COD_SEQ_ID<>@ID"; }
             x = dDB.QueryOne<dynamic>(sql, new { COD_Uniform_Numbers = txtNumbers.Text.Trim(), ID = iID });
            if (x != null) { oResult.Error("已存在相同[統一編號]資料;"); }

        }

        protected override void SaveData()
        {
            var oData = new CompanyData
            {
                COD_SEQ_ID = iID,
                COD_NAME = txtCOD_NAME.Text.Trim(),
                COD_ADDRESS = txtCOD_ADDRESS.Text.Trim(),
                COD_Uniform_Numbers = txtNumbers.Text.Trim(),
                //Tag_Type = ddlTagType.SelectedValue;
                COD_Tel = txtCOD_Tel.Text.Trim(),
                COD_EMail = txtCOD_EMail.Text.Trim(),
                COD_Contact = txtCOD_Contact.Text.Trim(),
                Update_User = bUser.User.ID,
                Update_Time = DateTime.Now
            };

            oData.COD_SEQ_ID = dDB.Execute(cyc.DB.Shared.GetEditSQL("CompanyData", "COD_NAME,COD_ADDRESS,COD_Uniform_Numbers,COD_Tel,COD_EMail,COD_Contact,COD_TYPE,Update_User,Update_Time;;COD_SEQ_ID", iID == 0), oData, iID);

            if (oResult.Success)
                cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(Request.QueryString["app"]), ExecType = iID == 0 ? "insert" : "update", ExecDesc = string.Format("業主資料，CompanyData ID={0} COD_NAME={1}", oData.COD_SEQ_ID, oData.COD_NAME), UserID = bUser.User.ID, KeyValue = oData.COD_SEQ_ID.ToString() }, dDB);
        }


        #endregion

    }
}