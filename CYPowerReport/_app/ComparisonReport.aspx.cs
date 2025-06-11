using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data;
using System.Globalization;
using System.IO;
using System.Configuration;
using ClosedXML.Excel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using Spire.Xls;
using System.Drawing;

namespace WebApp._app
{
    public partial class ComparisonReport : BasePageGridMulti
    {
        GregorianCalendar Getweek = new GregorianCalendar();
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
                dteDateE.Text = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");

                //bPara.Command = "SELECT sysType FROM vSysType WHERE sysType IS NOT NULL ";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT sysType FROM vSysType WHERE sysType IS NOT NULL"))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_SysType.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }
                //bPara.Command = "SELECT C_City_Name FROM City WHERE len(C_City_ID) = 2 ";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT C_City_Name FROM City WHERE len(C_City_ID) = 2"))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_CountyID.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }
                //bPara.Command = "SELECT distinct CBD_Bearing FROM CaseBaseData ";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT distinct CBD_Bearing FROM CaseBaseData"))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_Bearing.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }
                //bPara.Command = "SELECT distinct CBD_Equipment_Brand FROM CaseBaseData ";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT distinct CBD_Equipment_Brand FROM CaseBaseData"))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_Equipment.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }
                RBL_Type.SelectedIndex = 0;
            }
            Chart1.ChartAreas.Clear();
            Chart1.Legends.Clear();
            Chart1.ChartAreas.Add("ChartArea1");
            Chart1.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 90;
            Chart1.Legends.Add("Legends1");
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {
            try
            {
                string sql = "";
                string groupSql = "";
                switch (RBL_Type.SelectedItem.ToString())
                {
                    case "每日":
                        /* sql = @"  SELECT CONVERT(date, DATA_DATE) as Date, CaseBaseData.[CBD_SEQ_ID], CaseBaseData.[CBD_Case_Name], CaseBaseData.[CBD_KW], [CBD_Deal] , cast(round(avg([UPLOAD_AMT] / [CBD_KW]),3) as numeric(20,3)) as UPLOAD_RATE, avg(fee) as fee
    FROM CaseBaseData inner join CaseUpload on CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID
    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
    inner join v_Fee fee on fee.[CBD_ID]=CaseUpload.CBD_SEQ_ID and CaseUpload.[DATA_DATE]=fee.[CM_DATE]
    where CaseUpload.DATA_DATE >= @DateS and CaseUpload.DATA_DATE <= @DateE ";
                        groupSql = @" group by CaseBaseData.[CBD_SEQ_ID],CaseBaseData.[CBD_Case_Name], [CBD_KW], [CBD_Deal],DATA_DATE";*/
                        sql = @"  SELECT CONVERT(date, CM_DATE) as Date,CaseBaseData.CBD_SEQ_ID, CaseBaseData.CBD_Case_Name, CaseBaseData.CBD_KW, CaseBaseData.CBD_Deal
, ISNULL(CAST(ROUND(AVG(UPLOAD_AMT / CBD_KW),3) AS NUMERIC(20,3)),0) AS UPLOAD_RATE
, ISNULL(AVG(fee),0) AS fee
FROM CaseBaseData RIGHT JOIN (SELECT * FROM v_Fee where CM_DATE >=@DateS and CM_DATE <= @DateE) v_Fee ON v_Fee.CBD_ID=CaseBaseData.CBD_SEQ_ID
LEFT JOIN (SELECT * FROM CaseUpload where DATA_DATE >=@DateS and DATA_DATE <= @DateE) CaseUpload ON CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID and v_Fee.CM_DATE=CaseUpload.DATA_DATE
INNER JOIN CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
INNER JOIN City on CaseBaseData.CBD_TownShip = City.C_City_ID
WHERE 1=1";
                        groupSql = @" group by CaseBaseData.[CBD_SEQ_ID],CaseBaseData.[CBD_Case_Name], [CBD_KW], [CBD_Deal],CM_DATE";
                        break;
                    case "每週":
                        /* sql = @"  SELECT CU_Year+'_'+CU_Week as Date, CaseBaseData.[CBD_SEQ_ID], CaseBaseData.[CBD_Case_Name], CaseBaseData.[CBD_KW], [CBD_Deal] , cast(round(avg([UPLOAD_AMT] / [CBD_KW]),3) as numeric(20,3)) as UPLOAD_RATE, avg(fee) as fee
    FROM CaseBaseData inner join CaseUpload on CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID
    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
    inner join v_Fee fee on fee.[CBD_ID]=CaseUpload.CBD_SEQ_ID and  fee.CM_DATE=CaseUpload.DATA_DATE
    where CaseUpload.CU_Year >= @YearS and CaseUpload.CU_Week >= @WeekS and CaseUpload.CU_Year <= @YearE and CaseUpload.CU_Week <= @WeekE ";*/
                        sql = @"  SELECT  CONVERT(varchar, v_Fee.Year)+'_'+v_Fee.Week as Date,CaseBaseData.CBD_SEQ_ID, CaseBaseData.CBD_Case_Name, CaseBaseData.CBD_KW, CaseBaseData.CBD_Deal
, ISNULL(CAST(ROUND(AVG(UPLOAD_AMT / CBD_KW),3) AS NUMERIC(20,3)),0) AS UPLOAD_RATE
, ISNULL(AVG(fee),0) AS fee
FROM CaseBaseData RIGHT JOIN (SELECT * FROM v_Fee where CM_DATE >= @DateS and CM_DATE <= @DateE 
 ) v_Fee ON v_Fee.CBD_ID=CaseBaseData.CBD_SEQ_ID
LEFT JOIN (SELECT * FROM CaseUpload where  DATA_DATE >= @DateS and DATA_DATE <= @DateE ) CaseUpload ON CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID and v_Fee.CM_DATE=CaseUpload.DATA_DATE
INNER JOIN CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
INNER JOIN City on CaseBaseData.CBD_TownShip = City.C_City_ID 
WHERE 1=1";
                        groupSql = @" group by CaseBaseData.[CBD_SEQ_ID],CaseBaseData.[CBD_Case_Name], [CBD_KW], [CBD_Deal], CONVERT(varchar, v_Fee.Year)+'_'+v_Fee.Week";
                        GridView1.Columns[0].HeaderText = "年週";
                        break;
                    case "每月":
                        /* sql = @"  SELECT CU_Year+'_'+CU_Month as Date, CaseBaseData.[CBD_SEQ_ID], CaseBaseData.[CBD_Case_Name], CaseBaseData.[CBD_KW], [CBD_Deal] , cast(round(avg([UPLOAD_AMT] / [CBD_KW]),3) as numeric(20,3)) as UPLOAD_RATE, avg(fee) as fee
    FROM CaseBaseData inner join CaseUpload on CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID
    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
    inner join v_Fee fee on fee.[CBD_ID]=CaseUpload.CBD_SEQ_ID and  fee.CM_DATE=CaseUpload.DATA_DATE
    where CaseUpload.CU_Year >= @YearS and CaseUpload.CU_Month >= @MonthS and CaseUpload.CU_Year <= @YearE and CaseUpload.CU_Month <= @MonthE ";*/
                        sql = @"  SELECT CONVERT(varchar, v_Fee.Year)+'_'+v_Fee.Month as Date,CaseBaseData.CBD_SEQ_ID, CaseBaseData.CBD_Case_Name, CaseBaseData.CBD_KW, CaseBaseData.CBD_Deal
, ISNULL(CAST(ROUND(AVG(UPLOAD_AMT / CBD_KW),3) AS NUMERIC(20,3)),0) AS UPLOAD_RATE
, ISNULL(AVG(fee),0) AS fee
FROM CaseBaseData RIGHT JOIN (SELECT * FROM v_Fee  
where CM_DATE >= @DateS and CM_DATE <= @DateE 
) v_Fee ON v_Fee.CBD_ID=CaseBaseData.CBD_SEQ_ID
LEFT JOIN (SELECT * FROM CaseUpload where DATA_DATE >= @DateS and DATA_DATE <= @DateE ) CaseUpload ON CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID and v_Fee.CM_DATE=CaseUpload.DATA_DATE
INNER JOIN CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
INNER JOIN City on CaseBaseData.CBD_TownShip = City.C_City_ID
WHERE 1=1";
                        groupSql = @" group by CaseBaseData.[CBD_SEQ_ID],CaseBaseData.[CBD_Case_Name], [CBD_KW], [CBD_Deal], CONVERT(varchar, v_Fee.Year)+'_'+v_Fee.Month";
                        GridView1.Columns[0].HeaderText = "年月";
                        break;
                    case "每年":
                        /* sql = @"  SELECT CU_Year as Date, CaseBaseData.[CBD_SEQ_ID], CaseBaseData.[CBD_Case_Name], CaseBaseData.[CBD_KW], [CBD_Deal] , cast(round(avg([UPLOAD_AMT] / [CBD_KW]),3) as numeric(20,3)) as UPLOAD_RATE, avg(fee) as fee
    FROM CaseBaseData inner join CaseUpload on CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID
    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
    inner join v_Fee fee on fee.[CBD_ID]=CaseUpload.CBD_SEQ_ID and fee.CM_DATE=CaseUpload.DATA_DATE
    where CaseUpload.CU_Year >= @YearS and CaseUpload.CU_Year <= @YearE ";*/
                        sql = @"  SELECT CONVERT(varchar, v_Fee.Year) as Date,CaseBaseData.CBD_SEQ_ID, CaseBaseData.CBD_Case_Name, CaseBaseData.CBD_KW, CaseBaseData.CBD_Deal
, ISNULL(CAST(ROUND(AVG(UPLOAD_AMT / CBD_KW),3) AS NUMERIC(20,3)),0) AS UPLOAD_RATE
, ISNULL(AVG(fee),0) AS fee
FROM CaseBaseData RIGHT JOIN (SELECT * FROM v_Fee where Year >= @YearS and Year <= @YearE) v_Fee ON v_Fee.CBD_ID=CaseBaseData.CBD_SEQ_ID
LEFT JOIN (SELECT * FROM CaseUpload where CU_Year >= @YearS and CU_Year<= @YearE) CaseUpload ON CaseBaseData.CBD_SEQ_ID = CaseUpload.CBD_SEQ_ID and v_Fee.CM_DATE=CaseUpload.DATA_DATE
INNER JOIN CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
INNER JOIN City on CaseBaseData.CBD_TownShip = City.C_City_ID
WHERE 1=1";
                        groupSql = @" group by CaseBaseData.[CBD_SEQ_ID],CaseBaseData.[CBD_Case_Name], [CBD_KW], [CBD_Deal],CONVERT(varchar, v_Fee.Year)";
                        GridView1.Columns[0].HeaderText = "年";
                        break;
                }
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
                            sql1 = sql1 + @"CutomerData.CD_TYPE = '" + CBL_SysType.Items[i].Value.Trim() + "'";
                        }
                        if (i == CBL_SysType.Items.Count - 1)
                        {
                            sql1 = sql1 + @")";
                        }
                    }
                }
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
                            sql3 = sql3 + @"CaseBaseData.CBD_Bearing = '" + CBL_Bearing.Items[i].Value.Trim() + "'";
                        }
                        if (i == CBL_Bearing.Items.Count - 1)
                        {
                            sql3 = sql3 + @")";
                        }
                    }
                }
                #region 案場改複選
                string sql4 = "";
                if (ddlCaseBaseData.Items.Count > 0)
                {
                    sql4 = sql4 + @" and CaseBaseData.CBD_SEQ_ID in (";
                    for (int i = 0; i < ddlCaseBaseData.Items.Count; i++)
                    {
                        sql4 += "'" + ddlCaseBaseData.Items[i].Value + "',";
                    }

                    sql4 = sql4.Substring(0, sql4.Length - 1);

                    sql4 += ")";
                }
                #endregion

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
                            sql5 = sql5 + @"CaseBaseData.CBD_Equipment_Brand = '" + CBL_Equipment.Items[i].Value.Trim() + "'";
                        }
                        if (i == CBL_Equipment.Items.Count - 1)
                        {
                            sql5 = sql5 + @")";
                        }
                    }
                }

                sql = sql + sql1 + sql2 + sql3 + sql4 + sql5;

                sql = sql + groupSql;
                sql = sql + @" order by CaseBaseData.[CBD_SEQ_ID], Date";
                DateTime dDateS = Convert.ToDateTime(dteDateS.Text + @" " + @"00:00:00");
                DateTime dDateE = Convert.ToDateTime(dteDateE.Text + @" " + @"23:59:59");
                bPara.Command = sql;
                bPara.Parameter.Clear();
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekS", Getweek.GetWeekOfYear(dDateS, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekE", Getweek.GetWeekOfYear(dDateE, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthS", dDateS.Month));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthE", dDateE.Month));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearS", dDateS.Year));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearE", dDateE.Year));

                //using (DataTable oDT = bDB.QueryDT(bPara))
                using (DataTable oDT = dDB.QueryDataTable(bPara))
                {

                    DataTable oDT2 = new DataTable();

                    if (redOnly.Checked)
                    {
                        DataView dv = new DataView(oDT);
                        dv.RowFilter = "fee < upload_Rate";
                        oDT2 = dv.ToTable();
                    }
                    else
                    {
                        oDT2 = oDT;
                    }

                    if (oDT2.Rows.Count == 0)
                    {
                        Chart1.Visible = false;
                    }
                    else
                    {
                        Chart1.Visible = true;
                        Chart1.Series.Clear();
                        Chart1.Titles.Clear();
                        Chart1.Series.Add("合約");
                        Chart1.Series.Add("監控");
                        Chart1.Series.Add("電費");
                        Chart1.Series[0].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        Chart1.Series[0].BorderWidth = 2;
                        Chart1.Series[0].Legend = "Legends1";
                        Chart1.Series[0].LegendText = "合約";
                        Chart1.Series[0].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                        Chart1.Series[0].MarkerSize = 12;
                        Chart1.Series[1].XValueMember = "name";
                        Chart1.Series[1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        Chart1.Series[1].BorderWidth = 2;
                        Chart1.Series[1].Legend = "Legends1";
                        Chart1.Series[1].LegendText = "監控";
                        Chart1.Series[1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                        Chart1.Series[1].MarkerSize = 12;
                        Chart1.Series[2].XValueMember = "name";
                        Chart1.Series[2].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        Chart1.Series[2].BorderWidth = 2;
                        Chart1.Series[2].Legend = "Legends1";
                        Chart1.Series[2].LegendText = "電費";
                        Chart1.Series[2].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                        Chart1.Series[2].MarkerSize = 12;
                        float temp;

                        for (int i = 0; i < oDT2.Rows.Count; i++)
                        {
                            if (i > 5000)
                            {
                                limitHint.Text = "繪製筆數超過5000筆上限";
                                break;
                            }
                            else
                            {
                                limitHint.Text = "";
                            }
                            float.TryParse(oDT2.Rows[i][4].ToString(), out temp);
                            Chart1.Series[0].Points.AddXY(oDT2.Rows[i][2].ToString(), temp);
                            Chart1.Series[0].ToolTip = @"案場名稱:#VALX \n" + @"合約數據:#VALY{F5}";
                            float.TryParse(oDT2.Rows[i][5].ToString(), out temp);
                            Chart1.Series[1].Points.AddXY(oDT2.Rows[i][2].ToString(), temp);
                            Chart1.Series[1].ToolTip = @"案場名稱:#VALX \n" + @"監控數據:#VALY{F5}";
                            float.TryParse(oDT2.Rows[i][6].ToString(), out temp);
                            Chart1.Series[2].Points.AddXY(oDT2.Rows[i][2].ToString(), temp);
                            Chart1.Series[2].ToolTip = @"案場名稱:#VALX \n" + @"電費數據:#VALY{F5}";
                        }
                    }
                    return oDT2;
                }
            }
            catch (Exception ex)
            {
                WriteErr(ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
                return null;
            }
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = false, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
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

                    bPara.Command = strSQL;
                    bPara.Result = oResult;
                    ddlCity.Items.Clear();

                    //using (DataTable oDT = bDB.QueryDT(bPara))
                    using (DataTable oDT = dDB.QueryDataTable(bPara))
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

        protected void lkbReCaseBaseDataData_Click(object sender, EventArgs e)
        {
            string strValue = "";
            strValue = hidCaseBaseData.Value;
            string[] arrValue;
            string strSQL = "";
            string strWhereSQL = "";
            try
            {
                if (strValue != "")
                {
                    arrValue = strValue.Split('*');
                    strSQL = "select CBD_SEQ_ID,CBD_Case_Name from CaseBaseData";
                    for (int i = 0; i < arrValue.Length; i++)
                    {
                        if (i == 0) { strWhereSQL += " where CBD_SEQ_ID in ("; }
                        strWhereSQL += "'" + arrValue[i] + "',";
                    }
                    if (strWhereSQL != "")
                    {
                        strWhereSQL = strWhereSQL.Substring(0, strWhereSQL.Length - 1);
                        strWhereSQL += ")";
                    }
                    strSQL += strWhereSQL;

                    bPara.Command = strSQL;
                    bPara.Result = oResult;
                    ddlCaseBaseData.Items.Clear();

                    //using (DataTable oDT = bDB.QueryDT(bPara))
                    using (DataTable oDT = dDB.QueryDataTable(bPara))
                    {
                        if (oResult.Success)
                        {
                            this.ddlCaseBaseData.DataSource = oDT;
                            ddlCaseBaseData.DataTextField = "CBD_Case_Name";
                            ddlCaseBaseData.DataValueField = "CBD_SEQ_ID";
                            this.ddlCaseBaseData.DataBind();
                        }
                    }

                }
                else
                {
                    ddlCaseBaseData.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                WriteErr(ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
            }

        }

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

        #endregion


        #region "匯出"
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = QuerySourceData(0);
            switch (RBL_Type.SelectedItem.ToString())
            {
                case "每日":
                    dt.Columns[0].ColumnName = "日期";
                    break;
                case "每週":
                    dt.Columns[0].ColumnName = "年週";
                    break;
                case "每月":
                    dt.Columns[0].ColumnName = "年月";
                    break;
                case "每年":
                    dt.Columns[0].ColumnName = "年";
                    break;
            }
            //            案場編號 案場名稱    設置容量 合約  監控 電費
            dt.Columns[1].ColumnName = "案場編號";
            dt.Columns[2].ColumnName = "案場名稱";
            dt.Columns[3].ColumnName = "設置容量";
            dt.Columns[4].ColumnName = "合約";
            dt.Columns[5].ColumnName = "監控";
            dt.Columns[6].ColumnName = "電費";
            ToExcel(dt, RBL_Type.SelectedItem.ToString() + "發電小時比較報表_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }


        public void ToExcel(DataTable dt, string fileName)
        {
            string path = Server.MapPath("~/ReportTemplate/發電小時比較報表_範本.xlsx");

            using (Workbook wb = new Workbook())
            {
                wb.LoadFromFile(path);
                Worksheet ws = wb.Worksheets[0];

                string[] columnNames = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                ws.InsertDataTable(dt, true, 3, 14);

                ws.Range[$"R3C14:R{3 + dt.Rows.Count}C{dt.Columns.Count + 13}", true].BorderAround(LineStyleType.Medium, Color.Black);
                ws.Range[$"R3C14:R{3 + dt.Rows.Count}C{dt.Columns.Count + 13}", true].BorderInside(LineStyleType.Medium, Color.Black);


                MemoryStream pic = new MemoryStream();
                Chart1.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                ws.Pictures.Add(4, 1, pic, 68, 69, ImageFormatType.Png);

                Utils.OutputExcelToDownloadSpire(wb, fileName); //匯出
            }
        }
        #endregion

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer))
            {
                if (drv != null)
                {
                    //change text
                    e.Row.Cells[4].Text = double.Parse(drv["CBD_Deal"].ToString()) == 0 ? "" : drv["CBD_Deal"].ToString();

                    double uploadRate = 0;
                    double fee = 0;
                    double.TryParse(drv["UPLOAD_RATE"].ToString(), out uploadRate);
                    double.TryParse(drv["fee"].ToString(), out fee);

                    if (fee < uploadRate) e.Row.Cells[6].BackColor = System.Drawing.Color.LightPink;
                }
            }
        }
    }
}