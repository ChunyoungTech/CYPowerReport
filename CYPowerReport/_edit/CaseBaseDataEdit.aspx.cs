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
    public class CaseBaseData
    {
        public int CBD_SEQ_ID { get; set; }
        public string CBD_Case_Name { get; set; }
        public int CBD_Case_Owner { get; set; }
        public string CBD_Equipment_Brand { get; set; }
        public string CBD_County_ID { get; set; }
        public string CBD_TownShip { get; set; }
        public string CBD_Address { get; set; }
        public string CBD_GPS { get; set; }
        public decimal CBD_KW { get; set; }
        public int CBD_Slices { get; set; }
        public string CBD_Module_Model { get; set; }
        public string CBD_Bearing { get; set; }
        public string CBD_Case_Type { get; set; }
        public string CBD_Voltage_Type { get; set; }
        public decimal CBD_Set_Amount { get; set; }
        public decimal CBD_Deal { get; set; }
        public decimal CBD_Electricity_Fee { get; set; }
        public string CBD_Case_Code { get; set; }
        public DateTime CBD_Stop_Date { get; set; }
        public string CBD_Remarks { get; set; }
        public bool CBD_SunlightMeter { get; set; }
        public int Update_User { get; set; }
        public DateTime Update_Time { get; set; }

        public int CompanyID { get; set; }

        public decimal TotalChangeRate          { get; set; }
        public decimal TaxRate              { get; set; }
        public decimal RemittanceFee        { get; set; }
        public decimal MeterRent { get; set; }

        public DateTime MeterUpDate { get; set; }
        public string ElectricNumber { get; set; }
    }

    public partial class CaseBaseDataEdit : cyc.Page.BasePageEdit
    {
        CaseBaseData oData;
        int iID = 0, iApp = 0;
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //bPara.Command = "SELECT DISTINCT  CBD_Equipment_Brand FROM CaseBaseData WHERE CBD_Equipment_Brand IS NOT NULL AND CBD_Equipment_Brand<>''";
                //bPara.Parameter.Clear();
                //using (DataTable oDT = bDB.QueryDT(bPara))
                //{
                //    if (oResult.Success)
                //    {
                //        dllCBD_Equipment_Brand.DataSource = oDT;
                //        dllCBD_Equipment_Brand.DataBind();
                //        dllCBD_Equipment_Brand.Items.Insert(0, "");
                //    }
                //}
                dllCBD_Equipment_Brand.DataSource = dDB.QueryDataTable("SELECT DISTINCT  CBD_Equipment_Brand FROM CaseBaseData WHERE CBD_Equipment_Brand IS NOT NULL AND CBD_Equipment_Brand<>''");
                dllCBD_Equipment_Brand.DataBind();
                dllCBD_Equipment_Brand.Items.Insert(0, "");

                ucCBD_Stop_Date.Text = "9999-12-31";
                ucMeterUpDate.Text = "9999-12-31";
            }
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["pa"], out iID) || !int.TryParse(Request.QueryString["app"], out iApp))
                oResult.Error("參數錯誤");
            base.OnLoad(e);
        }
        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "app,pa" };
        }

        protected override void LoadData()
        {
            CompanyList.DataSource = dDB.QueryDataTable("SELECT COD_SEQ_ID,COD_NAME FROM CompanyData order by COD_SEQ_ID");
            CompanyList.DataBind();
            CompanyList.Items.Insert(0,new ListItem("","0"));

            ddlCBD_Case_Owner.DataSource = dDB.QueryDataTable("SELECT CD_SEQ_ID,CD_NAME FROM CutomerData order by CD_SEQ_ID");
            ddlCBD_Case_Owner.DataBind();
            ddlCBD_Case_Owner.Items.Insert(0, "");

            ddlCBD_County_ID.DataSource = dDB.QueryDataTable("SELECT C_City_ID,C_City_Name FROM City where LEN(C_City_ID)=2 order by C_City_ID");
            ddlCBD_County_ID.DataBind();
            ddlCBD_County_ID.Items.Insert(0, "");

            
            if (iID != 0)
            {
                oData = dDB.QueryOne<CaseBaseData>("select * from CaseBaseData where CBD_SEQ_ID=@ID", new { ID = iID });
                if (oData == null)
                {
                    LoadModule_ModelData(iID, "0");
                    oResult.Error("查無資料");
                }
                else
                {
                    this.txtCBD_Case_Name.Text = oData.CBD_Case_Name.ToString();
                    //if (oData.CBD_Case_Owner != null) this.ddlCBD_Case_Owner.Items.FindByValue(Convert.ToString(oData.CBD_Case_Owner)).Selected = true;
                    this.CompanyList.Items.FindByValue(oData.CompanyID.ToString()).Selected = true;
                    this.ddlCBD_Case_Owner.Items.FindByValue(oData.CBD_Case_Owner.ToString()).Selected = true;
                    if (oData.CBD_Equipment_Brand != null) this.txtCBD_Equipment_Brand.Text = oData.CBD_Equipment_Brand.ToString();
                    if (oData.CBD_County_ID != null) this.ddlCBD_County_ID.Items.FindByValue(Convert.ToString(oData.CBD_County_ID)).Selected = true;
                    //if (oData.CBD_TownShip != null && oData.CBD_TownShip != "")
                    if (!string.IsNullOrEmpty(oData.CBD_TownShip))
                    {

                        ddlCBD_TownShip.DataSource = dDB.QueryDataTable("SELECT C_City_ID,C_City_Name FROM City WHERE C_Upper_ID=@CBD_County_ID order by C_City_ID", new { oData.CBD_County_ID });
                        ddlCBD_TownShip.DataBind();
                        ddlCBD_TownShip.Items.Insert(0, "");
                        this.ddlCBD_TownShip.Items.FindByValue(Convert.ToString(oData.CBD_TownShip)).Selected = true;
                    }
                    if (oData.CBD_Address != null) this.txtCBD_Address.Text = oData.CBD_Address.ToString();
                    if (oData.CBD_GPS != null) this.txtCBD_GPS.Text = oData.CBD_GPS.ToString();
                    //if (oData.CBD_KW != null)
                    this.txtCBD_KW.Text = oData.CBD_KW.ToString();
                    //if (oData.CBD_Slices != null)
                    this.txtCBD_Slices.Text = oData.CBD_Slices.ToString();

                    LoadModule_ModelData(iID, oData.CBD_Module_Model);
                    if (oData.CBD_Module_Model != null && oData.CBD_Module_Model != "0") this.ddlCBD_Module_Model.Items.FindByValue(Convert.ToString(oData.CBD_Module_Model)).Selected = true;

                    if (oData.CBD_Bearing != null) this.ddlCBD_Bearing.Text = oData.CBD_Bearing.ToString();
                    if (oData.CBD_Case_Type != null) this.ddlCBD_Case_Type.Text = oData.CBD_Case_Type.ToString();
                    if (oData.CBD_Voltage_Type != null) this.ddlCBD_Voltage_Type.Text = oData.CBD_Voltage_Type.ToString();
                    //if (oData.CBD_Set_Amount != null)
                    this.txtCBD_Set_Amount.Text = oData.CBD_Set_Amount.ToString();
                    //if (oData.CBD_Deal != null) 
                    this.txtCBD_Deal.Text = oData.CBD_Deal.ToString();
                    //if (oData.CBD_Electricity_Fee != null) 
                    this.txtCBD_Electricity_Fee.Text = oData.CBD_Electricity_Fee.ToString();
                    if (oData.CBD_Case_Code != null) this.txtCBD_Case_Code.Text = oData.CBD_Case_Code.ToString();
                    if (oData.CBD_Stop_Date != null) this.ucCBD_Stop_Date.Text = oData.CBD_Stop_Date.ToString();
                    if (oData.CBD_Remarks != null) this.txtCBD_Remarks.Text = oData.CBD_Remarks.ToString();
                    ddlCBD_SunlightMeter.SelectedValue = oData.CBD_SunlightMeter ? "1" : "0";

                    if (oData.TotalChangeRate != null) this.txtTotalChangeRate.Text = oData.TotalChangeRate.ToString();
                    if (oData.TaxRate != null) this.txtTaxRate.Text = oData.TaxRate.ToString();
                    if (oData.RemittanceFee != null) this.txtRemittanceFee.Text = oData.RemittanceFee.ToString("F0");
                    if (oData.MeterRent != null) this.txtMeterRent.Text = oData.MeterRent.ToString("F0");

                    if (oData.MeterUpDate != null) this.ucMeterUpDate.Text = oData.MeterUpDate.ToString();
                    if (oData.ElectricNumber != null) this.txtElectricNumber.Text = oData.ElectricNumber.ToString();


                }
            }
            else
            {
                LoadModule_ModelData(iID, "0");
            }
        }

        protected void LoadModule_ModelData(int iID, string CBD_Module_Model)
        {
            string strSQL = "";
            strSQL = "SELECT * FROM Module_Model";
            if (iID == 0)
            {
                strSQL += " where MM_StopDate is null";
            }
            else
            {
                //if (CBD_Module_Model != "0")
                //{
                //    strSQL += " where (MM_SEQ_ID=" + CBD_Module_Model;
                //    strSQL += " or MM_StopDate is null)";
                //}
                //else
                //{
                //    strSQL += " where MM_StopDate is null";
                //}

            }
            strSQL += " order by MM_SEQ_ID";

            //ddlCBD_Module_Model.DataSource = dDB.QueryDataTable("SELECT * FROM Module_Model order by MM_SEQ_ID");
            //新增增加停用日期規則
            ddlCBD_Module_Model.DataSource = dDB.QueryDataTable(strSQL);
            ddlCBD_Module_Model.DataBind();
            ddlCBD_Module_Model.Items.Insert(0, "");

            //using (DataTable qDT = dDB.QueryDataTable("SELECT * FROM Module_Model order by MM_SEQ_ID"))
            using (DataTable qDT = dDB.QueryDataTable(strSQL))
            {
                if (qDT != null)
                {
                    string createArrayScript = "var mm_date = [";
                    for (var i = 0; i < qDT.Rows.Count; i++)
                    {
                        createArrayScript += "[" + qDT.Rows[i]["MM_SEQ_ID"] + ",\"" + qDT.Rows[i]["MM_Module_Model"] + "\"," + qDT.Rows[i]["MM_Kw_Pis"] + "],";
                    }
                    createArrayScript += "[99,\"null\",0]]";
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "registerVideoQArray", createArrayScript, true);
                }
            }
        }

        protected override void SaveCheck()
        {
            //string sql = "select CBD_Case_Name from CaseBaseData where CBD_Case_Name=@Name";
            //if (this.hidID.Value != "0") { sql += " and CBD_SEQ_ID<>@ID"; }
            //var x = bDB.oConn.Query<dynamic>(sql, new { Name = txtCBD_Case_Name.Text.Trim(), ID = this.hidID.Value }).FirstOrDefault();
            //if (x != null) { oResult.Error("已存在相同[案場名稱]資料;"); }
            if (dDB.QueryOne<dynamic>("select CBD_Case_Name from CaseBaseData where CBD_Case_Name=@Name and CBD_SEQ_ID<>@ID", new { Name = txtCBD_Case_Name.Text.Trim(), ID = iID }) != null)
                oResult.Error("已存在相同[案場名稱]資料;");
        }

        protected override void SaveData()
        {
            oData = new CaseBaseData
            {
                CBD_SEQ_ID = iID,
                CBD_Case_Name = (txtCBD_Case_Name.Text.Trim()),
                CBD_Case_Owner = int.Parse(ddlCBD_Case_Owner.SelectedValue),
                CBD_Equipment_Brand = (txtCBD_Equipment_Brand.Text.Trim()),
                CBD_County_ID = ddlCBD_County_ID.SelectedValue,
                CBD_TownShip = ddlCBD_TownShip.SelectedValue,
                CBD_Address = (txtCBD_Address.Text.Trim()),
                CBD_GPS = (txtCBD_GPS.Text.Trim()),
                CBD_KW = (txtCBD_KW.Text.Trim() == "") ? 0 : decimal.Parse(txtCBD_KW.Text.Trim()),
                CBD_Slices = (txtCBD_Slices.Text.Trim() == "") ? 0 : int.Parse(txtCBD_Slices.Text.Trim()),
                CBD_Module_Model = ddlCBD_Module_Model.SelectedValue,
                CBD_Bearing = ddlCBD_Bearing.SelectedValue,
                CBD_Case_Type = ddlCBD_Case_Type.Text.Trim(),
                CBD_Voltage_Type = ddlCBD_Voltage_Type.SelectedValue,
                CBD_Set_Amount = (txtCBD_Set_Amount.Text.Trim() == "") ? 0 : decimal.Parse(txtCBD_Set_Amount.Text.Trim()),
                CBD_Deal = (txtCBD_Deal.Text.Trim() == "") ? 0 : decimal.Parse(txtCBD_Deal.Text.Trim()),
                CBD_Electricity_Fee = (txtCBD_Electricity_Fee.Text.Trim() == "") ? 0 : decimal.Parse(txtCBD_Electricity_Fee.Text.Trim()),
                CBD_Case_Code = (txtCBD_Case_Code.Text.Trim()),
                CBD_Stop_Date = Convert.ToDateTime(ucCBD_Stop_Date.Text),
                CBD_Remarks = (txtCBD_Remarks.Text.Trim()),
                CBD_SunlightMeter = ddlCBD_SunlightMeter.SelectedValue == "1",
                Update_User = bUser.User.ID,
                Update_Time = DateTime.Now,
                CompanyID = int.Parse(CompanyList.SelectedValue),
                TotalChangeRate= (txtTotalChangeRate.Text.Trim() == "") ? 0 : decimal.Parse(txtTotalChangeRate.Text.Trim()),
                TaxRate= (txtTaxRate.Text.Trim() == "") ? 0 : decimal.Parse(txtTaxRate.Text.Trim()),
                RemittanceFee= (txtRemittanceFee.Text.Trim() == "") ? 0 : decimal.Parse(txtRemittanceFee.Text.Trim()),
                MeterRent= (txtMeterRent.Text.Trim() == "") ? 0 : decimal.Parse(txtMeterRent.Text.Trim()),

                MeterUpDate = Convert.ToDateTime(ucMeterUpDate.Text),
                ElectricNumber = (txtElectricNumber.Text.Trim()),

            };

            //if (this.hidID.Value != "0")
            //    bPara.Command = "update CaseBaseData set CBD_Case_Name=@CBD_Case_Name," +
            //        "CBD_Case_Owner=@CBD_Case_Owner," +
            //        "CBD_Equipment_Brand=@CBD_Equipment_Brand," +
            //        "CBD_County_ID=@CBD_County_ID," +
            //        "CBD_TownShip=@CBD_TownShip," +
            //        "CBD_Address=@CBD_Address," +
            //        "CBD_GPS=@CBD_GPS," +
            //        "CBD_KW=@CBD_KW," +
            //        "CBD_Slices=@CBD_Slices," +
            //        "CBD_Module_Model=@CBD_Module_Model," +
            //        "CBD_Bearing=@CBD_Bearing," +
            //        "CBD_Case_Type=@CBD_Case_Type," +
            //        "CBD_Voltage_Type=@CBD_Voltage_Type," +
            //        "CBD_Set_Amount=@CBD_Set_Amount," +
            //        "CBD_Deal=@CBD_Deal," +
            //        "CBD_Electricity_Fee=@CBD_Electricity_Fee," +
            //        "CBD_Case_Code=@CBD_Case_Code," +
            //        "CBD_Stop_Date=@CBD_Stop_Date," +
            //        "CBD_Remarks=@CBD_Remarks," +
            //        "Update_User=@Update_User,Update_Time=@Update_Time where CBD_SEQ_ID=@CBD_SEQ_ID";
            //else
            //    bPara.Command = "insert into CaseBaseData (CBD_Case_Name,CBD_Case_Owner,CBD_Equipment_Brand,CBD_County_ID,CBD_TownShip,CBD_Address,CBD_GPS,CBD_KW,CBD_Slices,CBD_Module_Model,CBD_Bearing,CBD_Case_Type,CBD_Voltage_Type,CBD_Set_Amount,CBD_Deal,CBD_Electricity_Fee,CBD_Case_Code,CBD_Stop_Date,CBD_Remarks,Update_User,Update_Time) " +
            //        "values (@CBD_Case_Name,@CBD_Case_Owner,@CBD_Equipment_Brand,@CBD_County_ID,@CBD_TownShip,@CBD_Address,@CBD_GPS,@CBD_KW,@CBD_Slices,@CBD_Module_Model,@CBD_Bearing,@CBD_Case_Type,@CBD_Voltage_Type,@CBD_Set_Amount,@CBD_Deal,@CBD_Electricity_Fee,@CBD_Case_Code,@CBD_Stop_Date,@CBD_Remarks,@Update_User,@Update_Time);SELECT CAST(SCOPE_IDENTITY() as int)";


            //if (this.hidID.Value != "0")
            //    bPara.Command = string.Format("update CaseBaseData set {0} where CBD_SEQ_ID=@CBD_SEQ_ID", string.Join(",", sColumn.Select(p => string.Format("{0}=@{0}", p))));
            //else
            //    bPara.Command = string.Format("insert into CaseBaseData ({0}) values (@{1});SELECT CAST(SCOPE_IDENTITY() as int)", string.Join(",", sColumn), string.Join(",@", sColumn));

            string sColumnKey = "CBD_Case_Name,CBD_Case_Owner,CBD_Equipment_Brand,CBD_County_ID,CBD_TownShip,CBD_Address,CBD_GPS,CBD_KW,CBD_Slices,CBD_Module_Model,CBD_Bearing,CBD_Case_Type,CBD_Voltage_Type,CBD_Set_Amount,CBD_Deal,CBD_Electricity_Fee,CBD_Case_Code,CBD_Stop_Date,CBD_Remarks,CBD_SunlightMeter,Update_User,Update_Time,CompanyID,TotalChangeRate,TaxRate,RemittanceFee,MeterRent,meterUpDate,electricNumber;;CBD_SEQ_ID";
            //string[] sColumn = "CBD_Case_Name,CBD_Case_Owner,CBD_Equipment_Brand,CBD_County_ID,CBD_TownShip,CBD_Address,CBD_GPS,CBD_KW,CBD_Slices,CBD_Module_Model,CBD_Bearing,CBD_Case_Type,CBD_Voltage_Type,CBD_Set_Amount,CBD_Deal,CBD_Electricity_Fee,CBD_Case_Code,CBD_Stop_Date,CBD_Remarks,CBD_SunlightMeter,Update_User,Update_Time".Split(',');
            string sql = cyc.DB.Shared.GetEditSQL("CaseBaseData", sColumnKey, oData.CBD_SEQ_ID == 0);
            oData.CBD_SEQ_ID = dDB.Execute(sql, oData, oData.CBD_SEQ_ID);

            if (oResult.Success)
            {
                cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem()
                {
                    ExecID = iApp,
                    ExecType = iID == 0 ? "insert" : "update",
                    ExecDesc = string.Format("{2}案場資料，CutomerData ID={0} CD_NAME={1}", oData.CBD_SEQ_ID, oData.CBD_Case_Name, iID == 0 ? "新增" : "修改"),
                    UserID = bUser.User.ID,
                    KeyValue = oData.CBD_SEQ_ID.ToString()
                }, dDB);
            }


            //using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
            //{
            //    try
            //    {
            //        if (this.hidID.Value != "0")
            //        {
            //            oDB.oConn.Execute(bPara.Command, oData);
            //            cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]), ExecType = "update", ExecDesc = string.Format("修改業主資料，CutomerData ID={0} CD_NAME={1}", oData.CBD_SEQ_ID, oData.CBD_Case_Name), UserID = bUser.User.ID , KeyValue = oData.CBD_SEQ_ID.ToString() }, oDB);
            //        }
            //        else
            //        {
            //            oData.CBD_SEQ_ID = bDB.oConn.Query<int>(bPara.Command, oData).Single();
            //            cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]), ExecType = "insert", ExecDesc = string.Format("新增業主，CutomerData ID={0} CD_NAME={1}", oData.CBD_SEQ_ID, oData.CBD_Case_Name), UserID = bUser.User.ID, KeyValue = oData.CBD_SEQ_ID.ToString() }, oDB);
            //        }

            //        ////發布 TagDataChange
            //        //CYCloud.gObj.AutoSignal.DoTagDataChangePublish(oData.CBD_Case_Name);
            //    }
            //    catch (Exception ex) { cyc.Global.WriteError(ex.Message + ":" + ex.StackTrace, oResult); }
            //}

        }

        #endregion
        protected void CBD_County_ID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bPara.Command = "SELECT C_City_ID,C_City_Name FROM City WHERE C_Upper_ID=" + ddlCBD_County_ID.SelectedValue+ " order by C_City_ID";
            //bPara.Parameter.Clear();
            //using (DataTable oDT = bDB.QueryDT(bPara))
            //{
            //    if (oResult.Success)
            //    {
            //        ddlCBD_TownShip.DataSource = oDT;
            //        ddlCBD_TownShip.DataBind();
            //        ddlCBD_TownShip.Items.Insert(0, "");
            //    }
            //}
            ddlCBD_TownShip.DataSource = dDB.QueryDataTable("SELECT C_City_ID,C_City_Name FROM City WHERE C_Upper_ID=" + ddlCBD_County_ID.SelectedValue + " order by C_City_ID");
            ddlCBD_TownShip.DataBind();
            ddlCBD_TownShip.Items.Insert(0, "");
        }
    }
}