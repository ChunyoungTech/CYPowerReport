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
    public class CutomerData
    {

        public int CD_SEQ_ID { get; set; }
        public string CD_NAME { get; set; }
        public string CD_ADDRESS { get; set; }
        public string CD_Uniform_Numbers { get; set; }
        public string CD_Tel { get; set; }
        public string CD_EMail { get; set; }
        public string CD_Contact { get; set; }
        public string CD_TYPE { get; set; }
        public int Update_User { get; set; }
        public DateTime Update_Time { get; set; }
    }

    public partial class CutomerDataEdit : cyc.Page.BasePageEdit
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
                using (var oDT = dDB.QueryDataTable("SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''"))
                {
                    if (oResult.Success)
                    {
                        VCDType.DataSource = oDT;
                        VCDType.DataBind();
                        VCDType.Items.Insert(0, "");
                    }
                }
                //bPara.Command = "SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''";
                //bPara.Parameter.Clear();
                //using (DataTable oDT = bDB.QueryDT(bPara))
                //{
                //    if (oResult.Success)
                //    {
                //        VCDType.DataSource = oDT;
                //        VCDType.DataBind();
                //        VCDType.Items.Insert(0, "");
                //    }
                //}
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
                var oData = dDB.QueryOne<CutomerData>("select * from CutomerData where CD_SEQ_ID=@ID", new { ID = iID });
                if (oData == null)
                    oResult.Error("查無資料");
                else
                {
                    this.txtCD_NAME.Text = oData.CD_NAME.ToString();
                    if (oData.CD_ADDRESS != null) this.txtCD_ADDRESS.Text = oData.CD_ADDRESS.ToString();
                    if (oData.CD_Uniform_Numbers != null) this.txtNumbers.Text = oData.CD_Uniform_Numbers.ToString();
                    if (oData.CD_Tel != null) this.txtCD_Tel.Text = oData.CD_Tel.ToString();
                    if (oData.CD_EMail != null) this.txtCD_EMail.Text = oData.CD_EMail.ToString();
                    if (oData.CD_Contact != null) this.txtCD_Contact.Text = oData.CD_Contact.ToString();
                    if (oData.CD_TYPE != null) this.txtCD_TYPE.Text = oData.CD_TYPE.ToString();
                    if (oData.CD_TYPE != null) this.VCDType.Items.FindByValue(Convert.ToString(oData.CD_TYPE)).Selected = true;
                }
            }
        }

        protected override void SaveCheck()
        {
            string sql = "select CD_NAME from CutomerData where CD_NAME=@Name and CD_Type=@Type";
            if (iID != 0) { sql += " and CD_SEQ_ID<>@ID"; }
            var x = dDB.QueryOne<dynamic>(sql, new { Name = txtCD_NAME.Text.Trim(), ID = iID, Type = this.txtCD_TYPE.Text.Trim() });
            if (x != null) { oResult.Error("已存在相同[業主名稱]資料;"); }

        }

        protected override void SaveData()
        {
            var oData = new CutomerData
            {
                CD_SEQ_ID = iID,
                CD_NAME = txtCD_NAME.Text.Trim(),
                CD_ADDRESS = txtCD_ADDRESS.Text.Trim(),
                CD_Uniform_Numbers = txtNumbers.Text.Trim(),
                //Tag_Type = ddlTagType.SelectedValue;
                CD_Tel = txtCD_Tel.Text.Trim(),
                CD_EMail = txtCD_EMail.Text.Trim(),
                CD_Contact = txtCD_Contact.Text.Trim(),
                CD_TYPE = txtCD_TYPE.Text.Trim(),
                Update_User = bUser.User.ID,
                Update_Time = DateTime.Now
            };

            oData.CD_SEQ_ID = dDB.Execute(cyc.DB.Shared.GetEditSQL("CutomerData", "CD_NAME,CD_ADDRESS,CD_Uniform_Numbers,CD_Tel,CD_EMail,CD_Contact,CD_TYPE,Update_User,Update_Time;;CD_SEQ_ID", iID == 0), oData, iID);

            if (oResult.Success)
                cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(Request.QueryString["app"]), ExecType = iID == 0 ? "insert" : "update", ExecDesc = string.Format("業主資料，CutomerData ID={0} CD_NAME={1}", oData.CD_SEQ_ID, oData.CD_NAME), UserID = bUser.User.ID, KeyValue = oData.CD_SEQ_ID.ToString() }, dDB);
        }

        //protected override void LoadData()
        //{
        //    this.hidID.Value = Request.QueryString["pa"].ToString();
        //    if (this.hidID.Value != "0")
        //    {

        //        oData = bDB.oConn.Query<CutomerData>("select * from CutomerData where CD_SEQ_ID=@ID", new { ID = this.hidID.Value }).FirstOrDefault();
        //        if (oData == null)
        //            oResult.Error("查無資料");
        //        else
        //        {
        //            this.txtCD_NAME.Text = oData.CD_NAME.ToString();
        //            if (oData.CD_ADDRESS != null) this.txtCD_ADDRESS.Text = oData.CD_ADDRESS.ToString();
        //            if (oData.CD_Uniform_Numbers != null) this.txtNumbers.Text = oData.CD_Uniform_Numbers.ToString();
        //            if (oData.CD_Tel != null) this.txtCD_Tel.Text = oData.CD_Tel.ToString();
        //            if (oData.CD_EMail != null) this.txtCD_EMail.Text = oData.CD_EMail.ToString();
        //            if (oData.CD_Contact != null) this.txtCD_Contact.Text = oData.CD_Contact.ToString();
        //            if (oData.CD_TYPE != null) this.txtCD_TYPE.Text = oData.CD_TYPE.ToString();
        //            if (oData.CD_TYPE != null) this.VCDType.Items.FindByValue(Convert.ToString(oData.CD_TYPE)).Selected = true;
        //        }
        //    }
        //}

        //protected override void SaveCheck()
        //{

        //    string sql = "select CD_NAME from CutomerData where CD_NAME=@Name and CD_Type=@Type";
        //    if (this.hidID.Value != "0") { sql += " and CD_SEQ_ID<>@ID"; }
        //    var x = bDB.oConn.Query<dynamic>(sql, new { Name = txtCD_NAME.Text.Trim(), ID = this.hidID.Value,Type= this.txtCD_TYPE.Text.Trim() }).FirstOrDefault();
        //    if (x != null) { oResult.Error("已存在相同[業主名稱]資料;"); }

        //}

        //protected override void SaveData()
        //{

        //    oData = new CutomerData
        //    {
        //        CD_SEQ_ID = Convert.ToInt32(hidID.Value),
        //        CD_NAME = txtCD_NAME.Text.Trim(),
        //        CD_ADDRESS = txtCD_ADDRESS.Text.Trim(),
        //        CD_Uniform_Numbers = txtNumbers.Text.Trim(),
        //        //Tag_Type = ddlTagType.SelectedValue;
        //        CD_Tel = txtCD_Tel.Text.Trim(),
        //        CD_EMail = txtCD_EMail.Text.Trim(),
        //        CD_Contact = txtCD_Contact.Text.Trim(),
        //        CD_TYPE = txtCD_TYPE.Text.Trim(),
        //        Update_User = bUser.User.ID,
        //        Update_Time = DateTime.Now
        //    };


        //    if (this.hidID.Value != "0")
        //        bPara.Command = "update CutomerData set CD_NAME=@CD_NAME,CD_ADDRESS=@CD_ADDRESS,CD_Uniform_Numbers=@CD_Uniform_Numbers,CD_Tel=@CD_Tel,CD_EMail=@CD_EMail,CD_Contact=@CD_Contact,CD_TYPE=@CD_TYPE,Update_User=@Update_User,Update_Time=@Update_Time where CD_SEQ_ID=@CD_SEQ_ID";
        //    else
        //        bPara.Command = "insert into CutomerData (CD_NAME,CD_ADDRESS,CD_Uniform_Numbers,CD_Tel,CD_EMail,CD_Contact,CD_TYPE,Update_User,Update_Time) values (@CD_NAME,@CD_ADDRESS,@CD_Uniform_Numbers,@CD_Tel,@CD_EMail,@CD_Contact,@CD_TYPE,@Update_User,@Update_Time);SELECT CAST(SCOPE_IDENTITY() as int)";

        //    //MsgBox(bPara.Command);

        //    using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
        //    {
        //        try
        //        {
        //            if (this.hidID.Value != "0")
        //            {
        //                oDB.oConn.Execute(bPara.Command, oData);
        //                cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]), ExecType = "update", ExecDesc = string.Format("修改業主資料，CutomerData ID={0} CD_NAME={1}", oData.CD_SEQ_ID, oData.CD_NAME), UserID = bUser.User.ID, KeyValue = oData.CD_SEQ_ID.ToString() }, oDB);
        //            }
        //            else
        //            {
        //                oData.CD_SEQ_ID = bDB.oConn.Query<int>(bPara.Command, oData).Single();
        //                cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]), ExecType = "insert", ExecDesc = string.Format("新增業主，CutomerData ID={0} CD_NAME={1}", oData.CD_SEQ_ID, oData.CD_NAME), UserID = bUser.User.ID, KeyValue = oData.CD_SEQ_ID.ToString() }, oDB);
        //            }

        //            ////發布 TagDataChange
        //            //CYCloud.gObj.AutoSignal.DoTagDataChangePublish(oData.CD_NAME);
        //        }
        //        catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult); }
        //    }

        //}

        #endregion

    }
}