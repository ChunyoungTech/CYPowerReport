using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.IO;
using System.Configuration;

namespace WebApp._edit
{
    public class ElectricityBilling
    {
        public int EC_SEQ_ID { get; set; }

        //public string CBD_Case_Name { get; set; }
        public string CD_TYPE { get; set; }

        public int CBD_ID { get; set; }
        public string EC_Last_Meter_Reading { get; set; }
        public string EC_Current_Meter_Reading { get; set; }
        public string EC_Next_Meter_Reading { get; set; }
        public int EC_Days { get; set; }
        public decimal EC_Meter_Record { get; set; }

        public decimal EC_Calculation_Rate { get; set; }
        public decimal EC_Calculation_Record { get; set; }
        public decimal EC_Calculation_Amt { get; set; }
        public decimal EC_Daily_Amount { get; set; }

        public decimal EC_Duarantee_Rate { get; set; }
        public decimal EC_Daily_Billing { get; set; }
        public decimal EC_Check_Amount { get; set; }
        public decimal EC_Line_Loss { get; set; }

        public int Update_User { get; set; }
        public DateTime Update_Time { get; set; }

        public string InvoiceDate { get; set; }
        public decimal MeterRent { get; set; }
        public decimal RemittanceFee { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoicePrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal BusinessTax { get; set; }
        public decimal ActualIncome { get; set; }
        public string BankAccount { get; set; }

        public string LandOwner { get; set; }
        public string BankType { get; set; }
        public string MailDate { get; set; }
        public string ECDepartment { get; set; }
        public string ECMonth { get; set; }
        public string Comment { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public decimal SetAmount { get; set; }
        public decimal DayAvg { get; set; }
        public decimal ExtraPay { get; set; }
    }

    public partial class ElectricityBillingEdit : cyc.Page.BasePageEdit
    {
        private ElectricityBilling oData;
        protected int iID = 0;

        protected override void OnLoad(EventArgs e)
        {
            iID = Convert.ToInt32(ViewState["ID"] ?? Request.QueryString["pa"]);
            base.OnLoad(e);
        }

        //protected void Page_Init(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "CheckNum" + this.ClientID, "setV_" + this.ClientID + "();", true);
        //}

        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa" };
        }

        protected override void LoadData()
        {
            //this.hidID.Value = Request.QueryString["pa"].ToString();

            #region 業主類別

            using (var oDT = dDB.QueryDataTable("select distinct(CD_TYPE) as TypeName from vCaseDataALL where isnull(CD_TYPE,'')<>''"))
            {
                if (oResult.Success)
                {
                    ddlCD_TYPE.DataSource = oDT;
                    ddlCD_TYPE.DataTextField = "TypeName";
                    ddlCD_TYPE.DataValueField = "TypeName";
                    ddlCD_TYPE.DataBind();
                }
            }
            //bPara.Command = "select distinct(CD_TYPE) as TypeName" +
            //    " from vCaseDataALL" +
            //    " where isnull(CD_TYPE,'')<>''";
            //bPara.Parameter.Clear();
            //using (DataTable oDT = bDB.QueryDT(bPara))
            //{
            //    if (oResult.Success)
            //    {
            //        ddlCD_TYPE.DataSource = oDT;
            //        ddlCD_TYPE.DataTextField = "TypeName";
            //        ddlCD_TYPE.DataValueField = "TypeName";
            //        ddlCD_TYPE.DataBind();
            //    }
            //}

            #endregion 業主類別

            #region 案場名稱

            getCaseBaseData();

            #endregion 案場名稱

            if (iID != 0)
            {
                searchBar.Visible = false;
                //oData = bDB.oConn.Query<ElectricityBilling>("select EB.*,CD_TYPE" +
                //    " from ElectricityBilling as EB" +
                //    " inner join vCaseDataALL as CBD on CBD.CBD_SEQ_ID = EB.CBD_ID" +
                //    " where EC_SEQ_ID=@ID", new { ID = this.hidID.Value }).FirstOrDefault();
                var oData = dDB.QueryOne<ElectricityBilling>(@"
select EB.*,CD_TYPE from ElectricityBilling as EB
inner join vCaseDataALL as CBD on CBD.CBD_SEQ_ID = EB.CBD_ID where EC_SEQ_ID=@ID", new { ID = iID });
                if (oData == null)
                    oResult.Error("查無資料");
                else
                {
                    ddlCD_TYPE.SelectedValue = oData.CD_TYPE.ToString();
                    ddlCD_TYPE.Enabled = false;
                    getCaseBaseData();

                    ddlCBD_Case_Name.SelectedValue = oData.CBD_ID.ToString();
                    ddlCBD_Case_Name.Enabled = false;

                    if (oData.EC_Last_Meter_Reading != null) this.dteEC_Last_Meter_Reading.Text = oData.EC_Last_Meter_Reading.ToString();
                    if (oData.EC_Current_Meter_Reading != null) this.dteEC_Current_Meter_Reading.Text = oData.EC_Current_Meter_Reading.ToString();
                    if (oData.EC_Next_Meter_Reading != null) this.dteEC_Next_Meter_Reading.Text = oData.EC_Next_Meter_Reading.ToString();
                    this.txtEC_Days.Text = oData.EC_Days.ToString();
                    this.txtEC_Meter_Record.Text = oData.EC_Meter_Record.ToString();
                    this.txtEC_Calculation_Rate.Text = oData.EC_Calculation_Rate.ToString();
                    this.txtEC_Calculation_Record.Text = oData.EC_Calculation_Record.ToString();
                    this.txtEC_Calculation_Amt.Text = oData.EC_Calculation_Amt.ToString("F0");
                    this.txtEC_Daily_Amount.Text = oData.EC_Daily_Amount.ToString("F0");
                    this.txtEC_Duarantee_Rate.Text = oData.EC_Duarantee_Rate.ToString();
                    this.txtEC_Daily_Billing.Text = oData.EC_Daily_Billing.ToString();
                    this.txtEC_Check_Amount.Text = oData.EC_Check_Amount.ToString();
                    this.txtEC_Line_Loss.Text = oData.EC_Line_Loss.ToString();
                    if (oData.InvoiceDate != null) this.txtInvoiceDate.Text = oData.InvoiceDate.ToString();
                    if (oData.MeterRent != null) this.txtMeterRent.Text = oData.MeterRent.ToString("F0");
                    if (oData.RemittanceFee != null) this.txtRemittanceFee.Text = oData.RemittanceFee.ToString("F0");
                    if (oData.InvoiceNumber != null) this.txtInvoiceNumber.Text = oData.InvoiceNumber.ToString();
                    if (oData.InvoicePrice != null) this.txtInvoicePrice.Text = oData.InvoicePrice.ToString("F0");
                    if (oData.TotalPrice != null) this.txtTotalPrice.Text = oData.TotalPrice.ToString("F0");
                    if (oData.BusinessTax != null) this.txtBusinessTax.Text = oData.BusinessTax.ToString("F0");
                    if (oData.ActualIncome != null) this.txtActualIncome.Text = oData.ActualIncome.ToString("F0");
                    if (oData.BankAccount != null) this.txtBankAccount.Text = oData.BankAccount.ToString();
                    if (oData.LandOwner != null) this.txtLandOwner.Text = oData.LandOwner.ToString();
                    if (oData.BankType != null) this.txtBankType.Text = oData.BankType.ToString();
                    if (oData.MailDate != null) this.txtMailDate.Text = oData.MailDate.ToString();
                    if (oData.ECDepartment != null) this.txtECDepartment.Text = oData.ECDepartment.ToString();
                    if (oData.ECMonth != null) this.txtECMonth.Text = oData.ECMonth.ToString();
                    if (oData.Comment != null) this.txtComment.Text = oData.Comment.ToString();
                    if (oData.CompanyName != null) this.txtCompanyName.Text = oData.CompanyName.ToString();
                    if (oData.Address != null) this.txtAddress.Text = oData.Address.ToString();
                    if (oData.SetAmount != null) this.txtSetAmount.Text = oData.SetAmount.ToString();
                    if (oData.DayAvg != null) this.txtDayAvg.Text = oData.DayAvg.ToString();
                    if (oData.ExtraPay != null) this.txtExtraPay.Text = oData.ExtraPay.ToString("F0");
                }
            }
            else
            {
                preloadBillData();
            }

        }

        protected override void SaveCheck()
        {
            string sql = "select EC_SEQ_ID from ElectricityBilling where CBD_ID=@CBD_ID and EC_Current_Meter_Reading=@EC_Current_Meter_Reading";
            if (iID != 0) { sql += " and EC_SEQ_ID<>@ID"; }
            int intCBD_ID = (ddlCBD_Case_Name.SelectedValue == "") ? 0 : Convert.ToInt32(ddlCBD_Case_Name.SelectedValue);
            string strEC_Current_Meter_Reading = dteEC_Current_Meter_Reading.Text.Trim();
            var x = dDB.QueryOne<dynamic>(sql, new { CBD_ID = intCBD_ID, ID = iID, EC_Current_Meter_Reading = strEC_Current_Meter_Reading });
            if (x != null) { oResult.Error("已存在相同[電費報表]資料;"); }
        }

        protected override void SaveData()
        {
            try
            {
                oData = new ElectricityBilling
                {
                    EC_SEQ_ID = iID,
                    CBD_ID = (ddlCBD_Case_Name.SelectedValue == "") ? 0 : Convert.ToInt32(ddlCBD_Case_Name.SelectedValue),
                    EC_Last_Meter_Reading = dteEC_Last_Meter_Reading.Text.Trim(),
                    EC_Current_Meter_Reading = dteEC_Current_Meter_Reading.Text.Trim(),
                    EC_Next_Meter_Reading = dteEC_Next_Meter_Reading.Text.Trim(),
                    EC_Days = (txtEC_Days.Text.Trim() == "") ? 0 : Convert.ToInt32(txtEC_Days.Text.Trim()),
                    EC_Meter_Record = (txtEC_Meter_Record.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Meter_Record.Text.Trim()),
                    EC_Calculation_Rate = (txtEC_Calculation_Rate.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Calculation_Rate.Text.Trim()),
                    EC_Calculation_Record = (txtEC_Calculation_Record.Text.Trim() == "") ? 0 : Convert.ToDecimal(txtEC_Calculation_Record.Text.Trim()),
                    EC_Calculation_Amt = (txtEC_Calculation_Amt.Text.Trim() == "") ? 0 : Convert.ToDecimal(txtEC_Calculation_Amt.Text.Trim()),
                    EC_Daily_Amount = (txtEC_Daily_Amount.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Daily_Amount.Text.Trim()),
                    EC_Duarantee_Rate = (txtEC_Duarantee_Rate.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Duarantee_Rate.Text.Trim()),
                    EC_Daily_Billing = (txtEC_Daily_Billing.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Daily_Billing.Text.Trim()),
                    EC_Check_Amount = (txtEC_Check_Amount.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Check_Amount.Text.Trim()),
                    EC_Line_Loss = (txtEC_Line_Loss.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtEC_Line_Loss.Text.Trim()),
                    Update_User = bUser.User.ID,
                    Update_Time = DateTime.Now,
                    InvoiceDate = txtInvoiceDate.Text.Trim(),
                    MeterRent = (txtMeterRent.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtMeterRent.Text.Trim()),
                    RemittanceFee = (txtRemittanceFee.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtRemittanceFee.Text.Trim()),
                    InvoiceNumber = txtInvoiceNumber.Text.Trim(),
                    InvoicePrice = (txtInvoicePrice.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtInvoicePrice.Text.Trim()),
                    TotalPrice = (txtTotalPrice.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtTotalPrice.Text.Trim()),
                    BusinessTax = (txtBusinessTax.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtBusinessTax.Text.Trim()),
                    ActualIncome = (txtActualIncome.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtActualIncome.Text.Trim()),
                    BankAccount = txtBankAccount.Text.Trim(),
                    LandOwner = txtLandOwner.Text.Trim(),
                    BankType = txtBankType.Text.Trim(),
                    MailDate = txtMailDate.Text.Trim(),
                    ECDepartment = txtECDepartment.Text.Trim(),
                    ECMonth = txtECMonth.Text.Trim(),
                    Comment = txtComment.Text.Trim(),
                    CompanyName = txtCompanyName.Text.Trim(),
                    Address = txtAddress.Text.Trim(),
                    SetAmount = (txtSetAmount.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtSetAmount.Text.Trim()),
                    DayAvg = (txtDayAvg.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtDayAvg.Text.Trim()),
                    ExtraPay = (txtExtraPay.Text.Trim() == "") ? 0 : (decimal)Convert.ToDecimal(txtExtraPay.Text.Trim()),
                };

                //string strSQL = "";
                //if (iID != 0)
                //    strSQL="update ElectricityBilling set CBD_ID=@CBD_ID,EC_Last_Meter_Reading=@EC_Last_Meter_Reading," +
                //        "EC_Current_Meter_Reading=@EC_Current_Meter_Reading,EC_Next_Meter_Reading=@EC_Next_Meter_Reading," +
                //        "EC_Days=@EC_Days,EC_Meter_Record=@EC_Meter_Record," +
                //        "EC_Calculation_Rate=@EC_Calculation_Rate,EC_Calculation_Record=@EC_Calculation_Record," +
                //        "EC_Calculation_Amt=@EC_Calculation_Amt," +
                //        "EC_Daily_Amount=@EC_Daily_Amount,EC_Duarantee_Rate=@EC_Duarantee_Rate," +
                //        "EC_Daily_Billing=@EC_Daily_Billing,EC_Check_Amount=@EC_Check_Amount," +
                //        "EC_Line_Loss=@EC_Line_Loss,Update_User=@Update_User," +
                //        "Update_Time=@Update_Time" +
                //        " where EC_SEQ_ID=@EC_SEQ_ID";
                //else
                //    strSQL = "DECLARE @U_Table TABLE (C1 bigint);" +
                //        "insert into ElectricityBilling (CBD_ID,EC_Last_Meter_Reading," +
                //        "EC_Current_Meter_Reading,EC_Next_Meter_Reading," +
                //        "EC_Days,EC_Meter_Record," +
                //        "EC_Calculation_Rate,EC_Calculation_Record," +
                //        "EC_Calculation_Amt," +
                //        "EC_Daily_Amount,EC_Duarantee_Rate," +
                //        "EC_Daily_Billing,EC_Check_Amount," +
                //        "EC_Line_Loss," +
                //        "Update_User,Update_Time)" +
                //        " OUTPUT INSERTED.EC_SEQ_ID INTO @U_Table values (@CBD_ID,@EC_Last_Meter_Reading," +
                //        "@EC_Current_Meter_Reading,@EC_Next_Meter_Reading," +
                //        "@EC_Days,@EC_Meter_Record," +
                //        "@EC_Calculation_Rate,@EC_Calculation_Record," +
                //        "@EC_Calculation_Amt," +
                //        "@EC_Daily_Amount,@EC_Duarantee_Rate," +
                //        "@EC_Daily_Billing,@EC_Check_Amount," +
                //        "@EC_Line_Loss," +
                //        "@Update_User,@Update_Time);" +
                //        "select C1 from @U_Table;";

                //using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
                //{
                //    try
                //    {
                //        //bPara.Command = strSQL;
                //        if (this.hidID.Value != "0")
                //        {
                //            oDB.oConn.Execute(strSQL, oData);
                //            cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]), ExecType = "update", ExecDesc = string.Format("修改電費報表資料，ElectricityBilling ID={0} CBD_Case_Name={1}", oData.EC_SEQ_ID, ddlCBD_Case_Name.SelectedItem.Text.Trim()), UserID = bUser.User.ID, KeyValue = oData.EC_SEQ_ID.ToString() }, oDB);
                //        }
                //        else
                //        {
                //            oData.EC_SEQ_ID = bDB.oConn.Query<int>(strSQL, oData).Single();
                //            cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]), ExecType = "insert", ExecDesc = string.Format("新增電費報表資料，ElectricityBilling ID={0} CBD_Case_Name={1}", oData.EC_SEQ_ID, ddlCBD_Case_Name.SelectedItem.Text.Trim()), UserID = bUser.User.ID , KeyValue = oData.EC_SEQ_ID.ToString() }, oDB);
                //        }
                //    }
                //    catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult); }
                //}

                string sColumnKey = "CBD_ID,EC_Last_Meter_Reading,EC_Current_Meter_Reading,EC_Next_Meter_Reading,EC_Days,EC_Meter_Record,EC_Calculation_Rate,EC_Calculation_Record,EC_Calculation_Amt,EC_Daily_Amount,EC_Duarantee_Rate,EC_Daily_Billing,EC_Check_Amount,EC_Line_Loss,Update_User,Update_Time,InvoiceDate,MeterRent,RemittanceFee,InvoiceNumber,InvoicePrice,TotalPrice,BusinessTax,ActualIncome,BankAccount,LandOwner,BankType,MailDate,ECDepartment,ECMonth,Comment,SetAmount,Address,CompanyName,DayAvg,ExtraPay;;EC_SEQ_ID";
                oData.EC_SEQ_ID = dDB.Execute(cyc.DB.Shared.GetEditSQL("ElectricityBilling", sColumnKey, iID == 0), oData, iID);

                if (oResult.Success)
                {
                    ViewState["ID"] = oData.EC_SEQ_ID;
                    cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem()
                    {
                        ExecID = Convert.ToInt32(Request.QueryString["app"]),
                        ExecType = iID == 0 ? "insert" : "update",
                        ExecDesc = string.Format("{2}電費報表資料，ElectricityBilling ID={0} CBD_Case_Name={1}", oData.EC_SEQ_ID, ddlCBD_Case_Name.SelectedItem.Text.Trim(), iID == 0 ? "新增" : "修改"),
                        UserID = bUser.User.ID,
                        KeyValue = oData.EC_SEQ_ID.ToString()
                    }, dDB);
                }
            }
            catch (Exception ex)
            {
                cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult);
            }
        }

        #endregion #繼承

        #region 控制項事件

        protected void ddlCD_TYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCaseBaseData();
            preloadBillData();
        }

        protected void ddlCBD_Case_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            preloadBillData();
        }

        protected void getCaseBaseData()
        {
            try
            {
                bPara.Command = "select CBD_SEQ_ID,CBD_Case_Name" +
                    " from vCaseDataALL" +
                    " where CD_TYPE=@CD_TYPE and isnull(CBD_Case_Name,'')<>''";
                if (iID == 0)
                {
                    bPara.Command += " and CBD_Stop_Date>=convert(varchar(10),getdate(),111)";
                }
                bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@CD_TYPE", ddlCD_TYPE.SelectedValue) };
                bPara.Result = oResult;
                using (DataTable oDT = dDB.QueryDataTable(bPara))
                {
                    if (oResult.Success)
                    {
                        this.ddlCBD_Case_Name.DataSource = oDT;
                        ddlCBD_Case_Name.DataTextField = "CBD_Case_Name";
                        ddlCBD_Case_Name.DataValueField = "CBD_SEQ_ID";
                        this.ddlCBD_Case_Name.DataBind();
                    }
                }
            }
            catch
            {
            }
        }

        protected void preloadBillData()
        {
            try
            {
                //4.電費計算費率→帶入上次維護記錄
                DataTable dt = getPreviousData();
                if (dt.Rows.Count > 0)
                {
                    txtEC_Line_Loss.Text = dt.Rows[0]["EC_Line_Loss"].ToString();
                    txtEC_Calculation_Rate.Text = dt.Rows[0]["EC_Calculation_Rate"].ToString();
                    txtLandOwner.Text = dt.Rows[0]["LandOwner"].ToString();
                    txtBankType.Text = dt.Rows[0]["BankType"].ToString();
                    txtBankAccount.Text = dt.Rows[0]["BankAccount"].ToString();
                    //txtMailDate.Text = dt.Rows[0]["MailDate"].ToString();
                    txtECDepartment.Text = dt.Rows[0]["ECDepartment"].ToString();
                    //txtECMonth.Text = dt.Rows[0]["ECMonth"].ToString();
                    txtComment.Text = dt.Rows[0]["Comment"].ToString();
                    txtCompanyName.Text = dt.Rows[0]["CompanyName"].ToString();
                    txtAddress.Text = dt.Rows[0]["Address"].ToString();
                    txtSetAmount.Text = dt.Rows[0]["SetAmount"].ToString();
                    //txtDayAvg.Text = dt.Rows[0]["DayAvg"].ToString();
                    //要求改為帶上期 20230413
                    txtMeterRent.Text = dt.Rows[0]["MeterRent"].ToString().Split('.')[0];
                    //要求改為帶上期 20231110
                    txtRemittanceFee.Text = dt.Rows[0]["RemittanceFee"].ToString().Split('.')[0];
                }

            }
            catch (Exception ex)
            {
                cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult);
            }
        }

        protected DataRow getCaseBasePreloadData()
        {
            string strSQL = "select top 1 CBD_SEQ_ID,TotalChangeRate,TaxRate,RemittanceFee,MeterRent,CBD_KW" +
                    " from CaseBaseData" +
                    " where CBD_SEQ_ID=@CBD_ID" +
                    " order by Update_Time desc";
            bPara.Command = strSQL;
            bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@CBD_ID", ddlCBD_Case_Name.SelectedValue) };
            bPara.Result = oResult;
            using (DataTable oDT = dDB.QueryDataTable(bPara))
            {
                if (oResult.Success)
                {
                    for (int i = 0; i < oDT.Rows.Count; i++)
                    {
                        return oDT.Rows[i];
                    }
                }
            }
            return null;
        }

        protected DataTable getPreviousData()
        {
            string strSQL = @"select top 1 *
                     from ElectricityBilling
                     where CBD_ID=@CBD_ID
                     order by EC_Last_Meter_Reading desc";
            bPara.Command = strSQL;
            bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@CBD_ID", ddlCBD_Case_Name.SelectedValue) };
            bPara.Result = oResult;
            return dDB.QueryDataTable(bPara);

        }


        #endregion 控制項事件

        /// <summary>
        ///  將錯誤訊息寫到Log檔案，放到專案底下的\ErrLog
        /// </summary>
        /// <param name="strError">錯誤訊息</param>
        protected void WriteErr(string strError)
        {
            string strFilePath = "";
            try
            {
                strFilePath = ConfigurationManager.AppSettings["ErrLogPath"].ToString() + "/ErrLog/";

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                System.Web.HttpRequest request = HttpContext.Current.Request;
                string strRawUrl = request.RawUrl;
                strError = strRawUrl + "\r\n" + strError;

                File.WriteAllText(strFilePath + "/Err_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", strError);
            }
            catch (Exception ex)
            {
                strFilePath = "c:/ErrLog/";

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                System.Web.HttpRequest request = HttpContext.Current.Request;
                string strRawUrl = request.RawUrl;
                strError = strRawUrl + "\r\n" + strError + "\r\n" + ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString();

                File.WriteAllText(strFilePath + "/Err_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", strError);
            }
        }
    }
}