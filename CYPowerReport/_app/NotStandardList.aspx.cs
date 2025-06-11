using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using Spire.Xls;
using System.Drawing;
using System.Threading;

namespace WebApp._app
{
    public partial class NotStandardList : BasePageGridMulti
    {


        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");
                dteDateE.Text = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");

                bPara.Command = "SELECT sysType FROM vSysType where sysType is not null";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_SysType.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }

                bPara.Command = "SELECT distinct CBD_Bearing FROM CaseBaseData ";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_Bearing.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }

                bPara.Command = "SELECT distinct CBD_Equipment_Brand FROM CaseBaseData ";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_Equipment.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }

                bPara.Command = "SELECT CD_SEQ_ID,CD_NAME,CD_TYPE FROM CutomerData order by CD_SEQ_ID";
                bPara.Parameter.Clear();
                DataTable dt_questionVideo = dDB.QueryDataTable(bPara.Command);
                string createArrayScript = "var cd_date = [";
                for (var i = 0; i <= dt_questionVideo.Rows.Count - 1; i++)
                {
                    if (i == dt_questionVideo.Rows.Count - 1)
                    {
                        createArrayScript += "[" + dt_questionVideo.Rows[i]["CD_SEQ_ID"] + ",\"" + dt_questionVideo.Rows[i]["CD_NAME"] + "\",\"" + dt_questionVideo.Rows[i]["CD_TYPE"] + "\"]];";
                    }
                    else
                    {
                        createArrayScript += "[" + dt_questionVideo.Rows[i]["CD_SEQ_ID"] + ",\"" + dt_questionVideo.Rows[i]["CD_NAME"] + "\",\"" + dt_questionVideo.Rows[i]["CD_TYPE"] + "\"],";
                    }
                }
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "registerVideoQArray", createArrayScript, true);

                //VStandard.Checked = true;
            }


            Chart1.ChartAreas.Clear();
            Chart1.Legends.Clear();
            Chart1.ChartAreas.Add("ChartArea1");
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisY2.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = true;
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart1.Legends.Add("Legends1");
            Chart1.Legends["Legends1"].Font = new Font(Chart1.Legends["Legends1"].Font.Name, 10);


            base.OnInit(e);
        }

        protected override void OnUnload(EventArgs e)
        {

            base.OnUnload(e);
        }


        protected override DataTable QuerySourceData(int idx)
        {
            DateTime dDateS = Convert.ToDateTime(dteDateS.Text);
            DateTime dDateE = (Convert.ToDateTime(dteDateE.Text)).AddDays(1).AddMilliseconds(-1);

            int CBL_SysTypeCont = 0;
            int CBL_BearingCont = 0;
            int CBL_EquipmentCont = 0;
            for (int i = 0; i < CBL_SysType.Items.Count; i++)
            {
                if (CBL_SysType.Items[i].Selected)
                {
                    CBL_SysTypeCont += 1;
                }
            }
            for (int i = 0; i < CBL_Bearing.Items.Count; i++)
            {
                if (CBL_Bearing.Items[i].Selected)
                {
                    CBL_BearingCont += 1;
                }
            }
            for (int i = 0; i < CBL_Equipment.Items.Count; i++)
            {
                if (CBL_Equipment.Items[i].Selected)
                {
                    CBL_EquipmentCont += 1;
                }
            }

            #region 業主類別複選
            string sql1 = "";
            if (CBL_SysTypeCont != 0)
            {
                for (int i = 0; i < CBL_SysType.Items.Count; i++)
                {
                    if (CBL_SysType.Items[i].Selected)
                    {
                        if (sql1 == "")
                        {
                            sql1 = sql1 + @" and (";
                        }
                        else
                        {
                            sql1 = sql1 + @" or ";
                        }
                        sql1 = sql1 + @"CD_TYPE = '" + CBL_SysType.Items[i].Value.Trim() + "'";
                    }
                    if (i == CBL_SysType.Items.Count - 1)
                    {
                        sql1 = sql1 + @")";
                    }
                }
            }
            #endregion


            #region 縣市改複選
            string sql2 = "";

            if (ddlCity.Items.Count > 0)
            {
                sql2 = sql2 + @" and CBD_TownShip in (";
                for (int i = 0; i < ddlCity.Items.Count; i++)
                {
                    sql2 += "'" + ddlCity.Items[i].Value + "',";
                }

                sql2 = sql2.Substring(0, sql2.Length - 1);

                sql2 += ")";
            }
            #endregion

            #region 方向複選
            string sql3 = "";
            if (CBL_BearingCont != 0)
            {
                for (int i = 0; i < CBL_Bearing.Items.Count; i++)
                {
                    if (CBL_Bearing.Items[i].Selected)
                    {
                        if (sql3 == "")
                        {
                            sql3 = sql3 + @" and (";
                        }
                        else
                        {
                            sql3 = sql3 + @" or ";
                        }
                        sql3 = sql3 + @"CBD_Bearing = '" + CBL_Bearing.Items[i].Value.Trim() + "'";
                    }
                    if (i == CBL_Bearing.Items.Count - 1)
                    {
                        sql3 = sql3 + @")";
                    }
                }
            }
            #endregion

            #region 監控商複選
            string sql5 = "";
            if (CBL_EquipmentCont != 0)
            {
                for (int i = 0; i < CBL_Equipment.Items.Count; i++)
                {
                    if (CBL_Equipment.Items[i].Selected)
                    {
                        if (sql5 == "")
                        {
                            sql5 = sql5 + @" and (";
                        }
                        else
                        {
                            sql5 = sql5 + @" or ";
                        }
                        sql5 = sql5 + @"CBD_Equipment_Brand = '" + CBL_Equipment.Items[i].Value.Trim() + "'";
                    }
                    if (i == CBL_Equipment.Items.Count - 1)
                    {
                        sql5 = sql5 + @")";
                    }
                }
            }
            #endregion

            bPara.Command = @"select *,case when (Standard_1+Standard_2+Standard_3)=0 then '正常' 
                                when (Standard_1+Standard_2+Standard_3)=1 then '發電量未上傳' 
                                when (Standard_1+Standard_2+Standard_3)=2 then '發電小時<3.3' 
                                when (Standard_1+Standard_2+Standard_3)=4 then 'PR值<80' 
                                when (Standard_1+Standard_2+Standard_3)=3 then '發電量未上傳、發電小時<3.3' 
                                when (Standard_1+Standard_2+Standard_3)=5 then '發電量未上傳、PR值<80' 
                                when (Standard_1+Standard_2+Standard_3)=6 then '發電小時<3.3、PR值<80' 
                                when (Standard_1+Standard_2+Standard_3)=7 then '發電量未上傳、發電小時<3.3、PR值<80' 
                                end AS StandardTxt,
                                case when (Standard_4+Standard_5)=0 then '正常' else '平均發電小時比較異常' end AS StandardTxt2,
                                CONCAT(convert(varchar, DATA_DATE, 120),'_',CBD_SEQ_ID) as SID from vNotStandardList where 1=1";

            if (txtCaseName.Text.Trim().Length > 0) { bPara.Command += " and CBD_Case_Name like '%'+@Name+'%'"; }
            if (VStandard.SelectedValue == "1") bPara.Command += " and Standard_1=1";
            if (VStandard.SelectedValue == "2") bPara.Command += " and Standard_2=2";
            if (VStandard.SelectedValue == "3") bPara.Command += " and Standard_3=4";
            if (VStandard.SelectedValue == "4") bPara.Command += " and (Standard_4+Standard_5)>0";
            if (VStandard.SelectedValue == "5") bPara.Command += " and (Standard_1+Standard_2+Standard_3+Standard_4+Standard_5)>0";
            bPara.Command += " and DATA_DATE between '" + dDateS.ToString("yyyy-MM-dd HH:mm:ss") + "' and '" + dDateE.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            bPara.Command += sql1;
            bPara.Command += sql2;
            bPara.Command += sql3;
            bPara.Command += sql5;
            bPara.Command += " order by DATA_DATE,city_name,town_name";

            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtCaseName.Text.Trim()));

            //DataTable dt = bDB.QueryDT(bPara);
            DataTable dt = dDB.QueryDataTable(bPara);

            drawChart1(dt);

            return dt;

        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh, Excel = btnExport, AutoBind = true } } };
        }

        #region 複選功能
        protected void lkbReCityData_Click(object sender, EventArgs e)
        {
            string strValue = "";
            strValue = hidCity.Value;
            string[] arrCityValue;
            string strSQL = "";
            string strWhereSQL = "";
            try
            {
                if (strValue != "")
                {
                    arrCityValue = strValue.Split('*');
                    strSQL = "select C_City_ID,C_City_Name from City";
                    for (int i = 0; i < arrCityValue.Length; i++)
                    {
                        if (i == 0) { strWhereSQL += " where C_City_ID in ("; }
                        strWhereSQL += "'" + arrCityValue[i] + "',";
                    }
                    if (strWhereSQL != "")
                    {
                        strWhereSQL = strWhereSQL.Substring(0, strWhereSQL.Length - 1);
                        strWhereSQL += ")";
                    }
                    strSQL += strWhereSQL;

                    //bPara.Command = strSQL;
                    //bPara.Result = oResult;
                    ddlCity.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(strSQL))
                    {
                        if (oResult.Success)
                        {
                            this.ddlCity.DataSource = oDT;
                            ddlCity.DataTextField = "C_City_Name";
                            ddlCity.DataValueField = "C_City_ID";
                            this.ddlCity.DataBind();
                        }
                    }

                }
                else
                {
                    ddlCity.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                WriteErr(ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
            }

        }

        protected void lkbReTownData_Click(object sender, EventArgs e)
        {
            string strValue = "";
            strValue = hidCity.Value;
            string[] arrCityValue;
            string strSQL = "";
            string strWhereSQL = "";
            try
            {
                if (strValue != "")
                {
                    arrCityValue = strValue.Split('*');
                    strSQL = "select C_City_ID,C_City_Name from City";
                    for (int i = 0; i < arrCityValue.Length; i++)
                    {
                        if (i == 0) { strWhereSQL += " where C_City_ID in ("; }
                        strWhereSQL += "'" + arrCityValue[i] + "',";
                    }
                    if (strWhereSQL != "")
                    {
                        strWhereSQL = strWhereSQL.Substring(0, strWhereSQL.Length - 1);
                        strWhereSQL += ")";
                    }
                    strSQL += strWhereSQL;

                    //bPara.Command = strSQL;
                    //bPara.Result = oResult;
                    ddlCity.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(strSQL))
                    {
                        if (oResult.Success)
                        {
                            this.ddlCity.DataSource = oDT;
                            ddlCity.DataTextField = "C_City_Name";
                            ddlCity.DataValueField = "C_City_ID";
                            this.ddlCity.DataBind();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteErr(ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
            }

        }



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

        protected override ExportOption GetExportOption(int idx)
        {
            return new ExportOption()
            {   //"s":文字、"i":數字、"t":日期、"p":百分比、"y":是否
                Mapping = new string[] { "序號", "監控廠商", "業主類別", "案場業主", "案場名稱", "縣市", "鄉鎮", "資料日期", "設置容量", "發電量", "發電小時", "PR值", "7日均值", "7日鄉鎮均值", "月均值", "月鄉鎮均值", "年均值", "年鄉鎮均值", "異常說明", "異常說明二", "異動人員", "異動時間", "異動說明" },
                Column = new string[] { "CBD_SEQ_ID", "CBD_Equipment_Brand", "CD_TYPE", "CD_NAME", "CBD_Case_Name", "City_Name", "Town_Name", "DATA_DATE", "CBD_KW", "UPLOAD_AMT", "UPLOAD_RATE", "UPLOAD_PR", "CM_AVG_7", "CM_TS_AVG_7", "CM_AVG_MONTH", "CM_TS_AVG_MONTH", "CM_AVG_YEAR", "CM_TS_AVG_YEAR", "StandardTxt", "StandardTxt2", "REMARK_User", "REMARK_Time", "REMARK" },
                ColType = new string[] { "i", "s", "s", "s", "s", "s", "s", "t", "i", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s", "s" },
                FileName = "發電量未達標清單"
            };
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (decimal.Parse(drv["Standard_1"].ToString()) + decimal.Parse(drv["Standard_2"].ToString()) + decimal.Parse(drv["Standard_3"].ToString()) + decimal.Parse(drv["Standard_4"].ToString()) + decimal.Parse(drv["Standard_5"].ToString()) > 0)
                    e.Row.BackColor = System.Drawing.Color.LightPink;
            }
        }

        protected void btnExport2_Click(object sender, EventArgs e)
        {

            var dt = QuerySourceData(0);
            drawChart1(dt);
            //dt = dt.Select("City_Name","Town_Name", "CBD_Case_Name","").CopyToDataTable();

            ToExcel(dt, "發電量未達標清單_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }

        public void drawChart1(DataTable dt)
        {
            //Chart1.Visible = true;
            Chart1.Series.Clear();
            Chart1.Series.Add("當日發電小時");
            Chart1.Series.Add("7日平均發電小時");
            Chart1.Series.Add("上月平均發電小時");
            Chart1.Series.Add("去年平均發電小時");
            Chart1.Series.Add("7日鄉鎮平均發電小時");
            Chart1.Series.Add("上月鄉鎮平均發電小時");
            Chart1.Series.Add("去年鄉鎮平均發電小時");
            Chart1.Series[0].LegendText = "當日發電小時";
            Chart1.Series[1].LegendText = "7日平均發電小時";
            Chart1.Series[2].LegendText = "上月平均發電小時";
            Chart1.Series[3].LegendText = "去年平均發電小時";
            Chart1.Series[4].LegendText = "7日鄉鎮平均發電小時";
            Chart1.Series[5].LegendText = "上月鄉鎮平均發電小時";
            Chart1.Series[6].LegendText = "去年鄉鎮平均發電小時";
            Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Chart1.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Chart1.Series[2].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Chart1.Series[3].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
            Chart1.Series[4].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            Chart1.Series[5].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            Chart1.Series[6].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
            Chart1.Series[4].BorderWidth = 4;
            Chart1.Series[5].BorderWidth = 4;
            Chart1.Series[6].BorderWidth = 4;

            string x;
            foreach (DataRow d in dt.Rows)
            {
                x = d["CBD_Case_Name"].ToString();
                Chart1.Series[0].Points.AddXY(x, d["UPLOAD_RATE"].ToString());
                Chart1.Series[1].Points.AddXY(x, d["CM_AVG_7"].ToString());
                Chart1.Series[2].Points.AddXY(x, d["CM_AVG_MONTH"].ToString());
                Chart1.Series[3].Points.AddXY(x, d["CM_AVG_YEAR"].ToString());
                Chart1.Series[4].Points.AddXY(x, d["CM_TS_AVG_7"].ToString());
                Chart1.Series[5].Points.AddXY(x, d["CM_TS_AVG_MONTH"].ToString());
                Chart1.Series[6].Points.AddXY(x, d["CM_TS_AVG_YEAR"].ToString());
            }


        }

        public void ToExcel(DataTable dt, string fileName)
        {


            string sTemplateFile = Server.MapPath("~/ReportTemplate/發電量未達標清單_範本.xlsx");
            if (!File.Exists(sTemplateFile))
            {
                oResult.Error("範本檔案不存在");
            }

            else
            {

                try
                {

                    MemoryStream pic = new MemoryStream();
                    Chart1.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);

                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(sTemplateFile);
                    Worksheet sheet = workbook.Worksheets[0];
                    sheet.Pictures.Add(18, 1, pic, 65, 69, ImageFormatType.Png);
                    //sheet.InsertDataTable(dt, true, 5, 14);
                    int i = 2;
                    double temp1;
                    double temp2;
                    int startCityIndex = i;
                    int curCityIndex = 0;
                    string startCityVal = dt.Rows[0]["City_Name"].ToString();
                    string curCityVal = "";
                    int startTownIndex = i;
                    int curTownIndex = 0;
                    string startTownVal = dt.Rows[0]["Town_Name"].ToString();
                    string curTownVal = "";

                    foreach (DataRow d in dt.Rows)
                    {
                        int j = 1;
                        curCityVal = d["City_Name"].ToString();
                        sheet.Range[j++, i].Value = curCityVal;
                        curCityIndex = dealMerge(sheet, i, ref startCityIndex, ref startCityVal, curCityVal, 1);

                        curTownVal = d["Town_Name"].ToString();
                        sheet.Range[j++, i].Value = curTownVal;
                        curTownIndex = dealMerge(sheet, i, ref startTownIndex, ref startTownVal, curTownVal, 2);




                        sheet.Range[j++, i].Value = d["CBD_Case_Name"].ToString();

                        double.TryParse(d["UPLOAD_RATE"].ToString(), out temp1);

                        double.TryParse(d["TS_UPLOAD_RATE"].ToString(), out temp2);
                        sheet.Range[j++, i].Value2 = temp1;
                        sheet.Range[j++, i].Value = d["CBD_Deal"].ToString();
                        sheet.Range[j++, i].Value2 = temp2;
                        sheet.Range[j++, i].Value2 = (temp1 - temp2) / temp2;

                        double.TryParse(d["CM_AVG_7"].ToString(), out temp1);
                        double.TryParse(d["CM_TS_AVG_7"].ToString(), out temp2);
                        sheet.Range[j++, i].Value2 = temp1;
                        sheet.Range[j++, i].Value2 = temp2;
                        sheet.Range[j++, i].Value2 = (temp1 - temp2) / temp2;

                        double.TryParse(d["CM_AVG_MONTH"].ToString(), out temp1);
                        double.TryParse(d["CM_TS_AVG_MONTH"].ToString(), out temp2);
                        sheet.Range[j++, i].Value2 = temp1;
                        sheet.Range[j++, i].Value2 = temp2;
                        sheet.Range[j++, i].Value2 = (temp1 - temp2) / temp2;

                        double.TryParse(d["CM_AVG_YEAR"].ToString(), out temp1);
                        double.TryParse(d["CM_TS_AVG_YEAR"].ToString(), out temp2);
                        sheet.Range[j++, i].Value2 = temp1;
                        sheet.Range[j++, i].Value2 = temp2;
                        sheet.Range[j++, i].Value2 = (temp1 - temp2) / temp2;

 
                        i++;
                    }
                    //last merge
                    if (startCityIndex != curCityIndex)
                    {
                        sheet.Range[1, startCityIndex, 1, curCityIndex].Merge();
                    }
                    if (startTownIndex != curTownIndex)
                    {
                        sheet.Range[2, startTownIndex, 2, curTownIndex].Merge();
                    }

                    //save and launch the file
                    string strFileName = "發電量未達標清單_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                    //Thread thread = new Thread(() => Utils.OutputExcelToDownloadSpire(workbook, strFileName));
                    //thread.Start();
                    Utils.OutputExcelToDownloadSpire(workbook, strFileName); //匯出
                }

                catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult); }
            }
        }

        private static int dealMerge(Worksheet sheet, int i, ref int startIndex, ref string startVal, string curVal, int rowIndex)
        {
            int curIndex = i;
            try
            {

                if (startVal != curVal)
                {
                    sheet.Range[rowIndex, startIndex, rowIndex, curIndex - 1].Merge();
                    startVal = curVal;
                    startIndex = curIndex;
                }

            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace); }
            return curIndex;

        }
    }
    #endregion
}