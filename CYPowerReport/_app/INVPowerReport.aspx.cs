using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Globalization;
using System.IO;
using System.Configuration;
using ClosedXML.Excel;
using Spire.Xls;
using System.Drawing;

namespace WebApp._app
{
    public partial class INVPowerReport : BasePageGridMulti
    {
        GregorianCalendar Getweek = new GregorianCalendar();
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
                dteDateE.Text = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");

                //bPara.Command = "SELECT sysType FROM vSysType where sysType is not null";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT sysType FROM vSysType where sysType is not null"))
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
                using (DataTable oDT = dDB.QueryDataTable("SELECT C_City_Name FROM City WHERE len(C_City_ID) = 2 "))
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
                using (DataTable oDT = dDB.QueryDataTable("SELECT distinct CBD_Bearing FROM CaseBaseData "))
                {
                    if (oResult.Success)
                    {
                        for (int i = 0; i < oDT.Rows.Count; i++)
                        {
                            CBL_Bearing.Items.Add(oDT.Rows[i][0].ToString());
                        }
                    }
                }
                //bPara.Command = @"SELECT distinct CBD_Case_Name FROM [InvUpload]
                //inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                //inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner inner join City on CaseBaseData.CBD_County_ID = City.C_City_ID";
                ////where CutomerData.CD_TYPE = '" + CBL_SysType.SelectedItem.ToString().Trim() + @"' and City.C_City_Name = '" + CBL_CountyID.SelectedItem.ToString().Trim() + @"' and CaseBaseData.CBD_Bearing = '" + CBL_Bearing.SelectedItem.ToString().Trim() + @"'";
                //bPara.Parameter.Clear();
                //using (DataTable oDT = bDB.QueryDT(bPara))
                //{
                //    if (oResult.Success)
                //    {
                //        ddlCaseName.Items.Clear();

                //        for (int i = 0; i < oDT.Rows.Count; i++)
                //        {
                //            ddlCaseName.Items.Add(oDT.Rows[i][0].ToString());
                //        }
                //    }
                //}
                bPara.Command = "SELECT distinct CBD_Equipment_Brand FROM CaseBaseData ";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT distinct CBD_Equipment_Brand FROM CaseBaseData "))
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
            Chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = true;
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart1.Legends.Add("Legends1");
            Chart2.ChartAreas.Clear();
            Chart2.Legends.Clear();
            Chart2.ChartAreas.Add("ChartArea1");
            Chart2.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = true;
            Chart2.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart2.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart2.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart2.Legends.Add("Legends1");
            Chart3.ChartAreas.Clear();
            Chart3.Legends.Clear();
            Chart3.ChartAreas.Add("ChartArea1");
            Chart3.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart3.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = true;
            Chart3.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart3.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart3.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart3.Legends.Add("Legends1");
            Chart4.ChartAreas.Clear();
            Chart4.Legends.Clear();
            Chart4.ChartAreas.Add("ChartArea1");
            Chart4.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart4.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = true;
            Chart4.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart4.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart4.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart4.Legends.Add("Legends1");
            Chart5.ChartAreas.Clear();
            Chart5.Legends.Clear();
            Chart5.ChartAreas.Add("ChartArea1");
            Chart5.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            Chart5.ChartAreas["ChartArea1"].AxisX.LabelStyle.IsStaggered = true;
            Chart5.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            Chart5.ChartAreas["ChartArea1"].AxisX.IsMarginVisible = false;
            Chart5.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;
            Chart5.Legends.Add("Legends1");
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {
            #region whereOption
            string whereCondition = @"
    FROM InvUpload u
    JOIN InvBaseData b ON u.INV_NAME=b.IBD_INV_NAME 
                           AND u.CBD_SEQ_ID=b.CBD_SEQ_ID
    JOIN CaseBaseData c ON u.CBD_SEQ_ID=c.CBD_SEQ_ID
    JOIN CutomerData d ON d.CD_SEQ_ID = c.CBD_Case_Owner 
    WHERE u.DATA_DATE >= @DateS and u.DATA_DATE <= @DateE
";
            string condition = "";
            //案場
            condition = string.Join(",", ddlCaseBaseData.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and u.CBD_SEQ_ID in ({condition})";

            //縣市
            condition = string.Join(",", ddlCity.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and c.CBD_TownShip in ({condition})";

            //fix 業主過濾條件 
            condition = string.Join(",", CBL_SysType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and d.CD_TYPE in ({condition})";

            //fix 監控商過濾條件 
            condition = string.Join(",", CBL_Equipment.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and CBD_Equipment_Brand in ({condition})";
            #endregion
            DateTime dDateS = Convert.ToDateTime(dteDateS.Text + @" " + @"00:00:00");
            DateTime dDateE = Convert.ToDateTime(dteDateE.Text + @" " + @"23:59:59");

            //get max index
            //string sql = $@"SELECT max(ibd_inv_id) {whereCondition} ";
            string sql = $@"SELECT ISNULL(max(ibd_inv_id),0) {whereCondition} ";
            bPara.Command = sql;
            bPara.Parameter.Clear();
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));

            DataTable oDT = dDB.QueryDataTable(bPara);

            int maxIndex = int.Parse(oDT.Rows[0][0].ToString());

            if (maxIndex <= 0) 
            {
                oResult.Error("查無相關資料");
                return null;
            }

            //get data
            string dateCol = "";
            string pivotCol = "";
            List<string> numList = new List<string>();
            for (int i = 1; i <= maxIndex; i++)
            {
                numList.Add($"[{i}]");
            }
            string nums = string.Join(",", numList);

            switch (RBL_Type.SelectedItem.ToString())
            {
                case "每日":
                    dateCol = "data_date";
                    pivotCol = "u.data_date";
                    break;
                case "每週":
                    dateCol = "iu_year+'_'+iu_week as data_date";
                    pivotCol = "iu_year, iu_week";
                    break;
                case "每月":
                    dateCol = "iu_year+'_'+iu_month as data_date";
                    pivotCol = "iu_year, iu_month";
                    break;
                case "每年":
                    dateCol = "iu_year as data_date";
                    pivotCol = "iu_year";
                    break;
            }

            sql = $@"
SELECT CBD_Case_Name, {dateCol}, {nums}
FROM  
(
    SELECT c.CBD_Case_Name,ibd_inv_id ,sum(u.INV_AMT) AS total, {pivotCol}

    {whereCondition}

    GROUP BY ibd_inv_id,c.CBD_Case_Name, {pivotCol}
) AS SourceTable  
PIVOT  
(  
AVG(total)  
FOR ibd_inv_id IN ({nums})
) AS PivotTable;  
";

            bPara.Command = sql;
            bPara.Parameter.Clear();
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));

            oDT = dDB.QueryDataTable(bPara);
            //oDT = dDB.QueryDT(bPara);
            oDT.Columns.Add("sum", typeof(double));
            double sum = 0;
            double temp = 0;
            foreach (System.Data.DataColumn col in oDT.Columns) col.ReadOnly = false;
            foreach (DataRow r in oDT.Rows)
            {
                sum = 0;
                for (int i = 2; i <= maxIndex + 1; i++)
                {
                    temp = 0;
                    double.TryParse(r[i].ToString(), out temp);
                    sum += temp;
                    r[i] = Math.Round(temp, 2);
                }
                r["sum"] = sum;

            }

            //reset grid col

            GridView1.Columns.Clear();

            addCol("CBD_Case_Name", "案場名稱");
            addCol("data_date", "時間");

            ((BoundField)GridView1.Columns[1]).DataFormatString = "{0:yyyy/MM/dd}";
            for (int i = 1; i <= maxIndex; i++)
            {
                addCol(i.ToString(), "逆變器_" + i);
            }
            addCol("sum", "總計");

            //draw pic
            List<string> caseNames = oDT.AsEnumerable().Select(x => x["CBD_Case_Name"].ToString()).Distinct().ToList();
            Chart1.Visible = false;
            Chart2.Visible = false;
            Chart3.Visible = false;
            Chart4.Visible = false;
            Chart5.Visible = false;

            if (caseNames.Count > 0) drawChart(dDateS, dDateE, caseNames[0], Chart1);
            if (caseNames.Count > 1) drawChart(dDateS, dDateE, caseNames[1], Chart2);
            if (caseNames.Count > 2) drawChart(dDateS, dDateE, caseNames[2], Chart3);
            if (caseNames.Count > 3) drawChart(dDateS, dDateE, caseNames[3], Chart4);
            if (caseNames.Count > 4) drawChart(dDateS, dDateE, caseNames[4], Chart5);

            return oDT;
        }

        private void addCol(string dataField, string header)
        {
            var Bfile = new BoundField();
            Bfile.DataField = dataField;
            Bfile.HeaderText = header;
            GridView1.Columns.Add(Bfile);
        }

        //protected override DataTable QuerySourceData(int idx)
        //{

        //    DataTable oDT2 = new DataTable();
        //    string sql = "";
        //    string strWhereSQL2 = "";
        //    string strWhereSQL3 = "";
        //    switch (RBL_Type.SelectedItem.ToString())
        //    {
        //        case "每日":
        //            sql = @"Select distinct top 5 [CBD_Case_Name], CaseBaseData.CBD_SEQ_ID From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE ";
        //            break;
        //        case "每週":
        //            sql = @"Select distinct top 5 [CBD_Case_Name], CaseBaseData.CBD_SEQ_ID From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.IU_Year >= @YearS and InvUpload.IU_Week >= @WeekS and InvUpload.IU_Year <= @YearE and InvUpload.IU_Week <= @WeekE ";
        //            break;
        //        case "每月":
        //            sql = @"Select distinct top 5 [CBD_Case_Name], CaseBaseData.CBD_SEQ_ID From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.IU_Year >= @YearS and InvUpload.IU_Month >= @MonthS and InvUpload.IU_Year <= @YearE and InvUpload.IU_Month <= @MonthE ";
        //            break;
        //        case "每年":
        //            sql = @"Select distinct top 5 [CBD_Case_Name], CaseBaseData.CBD_SEQ_ID From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.IU_Year >= @YearS and InvUpload.IU_Year <= @YearE";
        //            break;
        //    }

        //    //fix 業主過濾條件 
        //    string result = string.Join(",", CBL_SysType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
        //    if (result != "")
        //        sql += $" and CutomerData.CD_TYPE in ({result})";

        //    //fix 監控商過濾條件 
        //    string Equipment = string.Join(",", CBL_Equipment.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
        //    if (Equipment != "")
        //        sql += $" and CBD_Equipment_Brand in ({Equipment})";

        //    #region 縣市改複選
        //    strWhereSQL2 = "";
        //    if (ddlCity.Items.Count > 0)
        //    {
        //        strWhereSQL2 = strWhereSQL2 + @" and CBD_TownShip in (";
        //        for (int i = 0; i < ddlCity.Items.Count; i++)
        //        {
        //            strWhereSQL2 += "'" + ddlCity.Items[i].Value + "',";
        //        }

        //        strWhereSQL2 = strWhereSQL2.Substring(0, strWhereSQL2.Length - 1);

        //        strWhereSQL2 += ")";
        //    }
        //    #endregion

        //    #region 案場改複選
        //    strWhereSQL3 = "";
        //    if (ddlCaseBaseData.Items.Count > 0)
        //    {
        //        strWhereSQL3 = strWhereSQL3 + @" and CaseBaseData.CBD_SEQ_ID in (";
        //        for (int i = 0; i < ddlCaseBaseData.Items.Count; i++)
        //        {
        //            strWhereSQL3 += "'" + ddlCaseBaseData.Items[i].Value + "',";
        //        }
        //        strWhereSQL3 = strWhereSQL3.Substring(0, strWhereSQL3.Length - 1);
        //        strWhereSQL3 += ")";
        //    }
        //    #endregion
        //    sql += strWhereSQL2 + strWhereSQL3;
        //    sql = sql + @" Order by CaseBaseData.CBD_SEQ_ID";
        //    DateTime dDateS = Convert.ToDateTime(dteDateS.Text + @" " + @"00:00:00");
        //    DateTime dDateE = Convert.ToDateTime(dteDateE.Text + @" " + @"23:59:59");
        //    bPara.Command = sql;
        //    bPara.Parameter.Clear();
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekS", Getweek.GetWeekOfYear(dDateS, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekE", Getweek.GetWeekOfYear(dDateE, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthS", dDateS.Month));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthE", dDateE.Month));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearS", dDateS.Year));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearE", dDateE.Year));
        //    DataTable oDT4 = bDB.QueryDT(bPara);
        //    switch (RBL_Type.SelectedItem.ToString())
        //    {
        //        case "每日":
        //            sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE ";
        //            break;
        //        case "每週":
        //            sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.IU_Year >= @YearS and InvUpload.IU_Week >= @WeekS and InvUpload.IU_Year <= @YearE and InvUpload.IU_Week <= @WeekE ";
        //            break;
        //        case "每月":
        //            sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.IU_Year >= @YearS and InvUpload.IU_Month >= @MonthS and InvUpload.IU_Year <= @YearE and InvUpload.IU_Month <= @MonthE ";
        //            break;
        //        case "每年":
        //            sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
        //            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //            inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
        //            where InvUpload.IU_Year >= @YearS and InvUpload.IU_Year <= @YearE";
        //            break;
        //    }

        //    //sql = sql + @" and CBD_Case_Name = '" + (ddlCaseName.SelectedIndex < 0 ? "" : ddlCaseName.SelectedItem.ToString().Trim()) + @"'";
        //    #region 縣市改複選
        //    strWhereSQL2 = "";
        //    if (ddlCity.Items.Count > 0)
        //    {
        //        strWhereSQL2 = strWhereSQL2 + @" and CBD_TownShip in (";
        //        for (int i = 0; i < ddlCity.Items.Count; i++)
        //        {
        //            strWhereSQL2 += "'" + ddlCity.Items[i].Value + "',";
        //        }

        //        strWhereSQL2 = strWhereSQL2.Substring(0, strWhereSQL2.Length - 1);

        //        strWhereSQL2 += ")";
        //    }
        //    #endregion

        //    #region 案場改複選
        //    strWhereSQL3 = "";
        //    if (ddlCaseBaseData.Items.Count > 0)
        //    {
        //        strWhereSQL3 = strWhereSQL3 + @" and CaseBaseData.CBD_SEQ_ID in (";
        //        for (int i = 0; i < ddlCaseBaseData.Items.Count; i++)
        //        {
        //            strWhereSQL3 += "'" + ddlCaseBaseData.Items[i].Value + "',";
        //        }

        //        strWhereSQL3 = strWhereSQL3.Substring(0, strWhereSQL3.Length - 1);

        //        strWhereSQL3 += ")";
        //    }

        //    #endregion
        //    sql += strWhereSQL2 + strWhereSQL3;

        //    sql = sql + @") A Order by LEN([INV_NAME]), [INV_NAME]";
        //    dDateS = Convert.ToDateTime(dteDateS.Text + @" " + @"00:00:00");
        //    dDateE = Convert.ToDateTime(dteDateE.Text + @" " + @"23:59:59");
        //    bPara.Command = sql;
        //    bPara.Parameter.Clear();
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekS", Getweek.GetWeekOfYear(dDateS, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekE", Getweek.GetWeekOfYear(dDateE, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthS", dDateS.Month));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthE", dDateE.Month));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearS", dDateS.Year));
        //    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearE", dDateE.Year));
        //    using (DataTable oDT = bDB.QueryDT(bPara))
        //    {
        //        string ssql = "";
        //        string ssql2 = "";
        //        string ssql3 = "";
        //        string[] INV_NAME = new string[1];
        //        if (oDT.Rows.Count > 0)
        //        {
        //            if (GridView1.Columns.Count > 0)
        //            {
        //                GridView1.Columns.Clear();
        //            }
        //            BoundField Bfile = new BoundField();
        //            Bfile.DataField = "CBD_Case_Name";
        //            Bfile.HeaderText = "案場名稱";
        //            GridView1.Columns.Add(Bfile);

        //            //fix 業主過濾條件 
        //            result = string.Join(",", CBL_SysType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
        //            string sqlWTF4 = "";
        //            if (result != "")
        //                sqlWTF4 = $" and CutomerData.CD_TYPE in ({result})";

        //            switch (RBL_Type.SelectedItem.ToString())
        //            {
        //                case "每日":
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "DATA_DATE";
        //                    Bfile.HeaderText = "日期";
        //                    GridView1.Columns.Add(Bfile);
        //                    INV_NAME = new string[oDT.Rows.Count];
        //                    for (int i = 0; i < oDT.Rows.Count; i++)
        //                    {
        //                        INV_NAME[i] = oDT.Rows[i][0].ToString();
        //                        Bfile = new BoundField();
        //                        Bfile.DataField = oDT.Rows[i][0].ToString();
        //                        Bfile.HeaderText = oDT.Rows[i][0].ToString();
        //                        GridView1.Columns.Add(Bfile);
        //                        ssql = ssql + @"[" + oDT.Rows[i][0].ToString() + @"]";
        //                        ssql2 = ssql2 + @"p.[" + oDT.Rows[i][0].ToString() + @"]";
        //                        ssql3 = ssql3 + @"isnull(p.[" + oDT.Rows[i][0].ToString() + @"],0)";
        //                        if (i != oDT.Rows.Count - 1)
        //                        {
        //                            ssql = ssql + @", ";
        //                            ssql2 = ssql2 + @", ";
        //                            ssql3 = ssql3 + @"+ ";
        //                        }
        //                    }
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "sumINV";
        //                    Bfile.HeaderText = "總計";
        //                    GridView1.Columns.Add(Bfile);
        //                    sql = @"Select p.[CBD_Case_Name], CONVERT(varchar(100), p.[DATA_DATE], 111) as DATA_DATE ," + ssql2 + @", " + ssql3 + @" as sumINV, p.[CBD_SEQ_ID]
        //                        FROM (
        //                            SELECT CaseBaseData.CBD_Case_Name ,l.[INV_NAME],InvBaseData.IBD_INV_ID, l.[DATA_DATE], l.[CBD_SEQ_ID], l.[INV_AMT] 
        //                                FROM [CYPowerReportDB].[dbo].[InvUpload] l 
        //                                inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = l.CBD_SEQ_ID
        //                                inner join InvBaseData on InvBaseData.IBD_INV_NAME= l.INV_NAME
        //                                inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //                                inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID 
        //                                where l.DATA_DATE >= @DateS and l.DATA_DATE <= @DateE ";
        //                    //sql = sql + @" and CBD_Case_Name = '" + (ddlCaseName.SelectedIndex < 0 ? "" : ddlCaseName.SelectedItem.ToString().Trim()) + @"'";
        //                    sql += strWhereSQL2 + strWhereSQL3 + sqlWTF4;
        //                    sql = sql + @") t PIVOT (MAX([INV_AMT]) FOR [IBD_INV_ID] IN (" + ssql + @")) p";
        //                    sql = sql + @" Order by p.[CBD_SEQ_ID], p.[DATA_DATE]";
        //                    break;
        //                case "每週":
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "IU_Week";
        //                    Bfile.HeaderText = "週數";
        //                    GridView1.Columns.Add(Bfile);
        //                    INV_NAME = new string[oDT.Rows.Count];
        //                    ssql3 = ssql3 + @"sum(";
        //                    for (int i = 0; i < oDT.Rows.Count; i++)
        //                    {
        //                        INV_NAME[i] = oDT.Rows[i][0].ToString();
        //                        Bfile = new BoundField();
        //                        Bfile.DataField = @"INV_" + i;
        //                        Bfile.HeaderText = oDT.Rows[i][0].ToString();
        //                        GridView1.Columns.Add(Bfile);
        //                        ssql = ssql + @"[" + oDT.Rows[i][0].ToString() + @"]";
        //                        ssql2 = ssql2 + @"sum(p.[" + oDT.Rows[i][0].ToString() + @"]) as INV_" + i;
        //                        ssql3 = ssql3 + @"isnull(p.[" + oDT.Rows[i][0].ToString() + @"],0)";
        //                        if (i != oDT.Rows.Count - 1)
        //                        {
        //                            ssql = ssql + @", ";
        //                            ssql2 = ssql2 + @", ";
        //                            ssql3 = ssql3 + @"+ ";
        //                        }
        //                    }
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "sumINV";
        //                    Bfile.HeaderText = "總計";
        //                    GridView1.Columns.Add(Bfile);
        //                    ssql3 = ssql3 + @")";
        //                    sql = @"Select p.[CBD_Case_Name], (p.[IU_Year] +'-W'+ p.[IU_Week]) as IU_Week ," + ssql2 + @", " + ssql3 + @" as sumINV,  p.[CBD_SEQ_ID] FROM (SELECT CaseBaseData.CBD_Case_Name ,l.[INV_NAME], l.[DATA_DATE], l.[CBD_SEQ_ID], l.[INV_AMT], l.[IU_Year], l.[IU_Week] FROM [CYPowerReportDB].[dbo].[InvUpload] l inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = l.CBD_SEQ_ID
        //                    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //                    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID";
        //                    //sql = sql + @" and CBD_Case_Name = '" + (ddlCaseName.SelectedIndex < 0 ? "" : ddlCaseName.SelectedItem.ToString().Trim()) + @"'";
        //                    sql += strWhereSQL2 + strWhereSQL3 + sqlWTF4;
        //                    sql = sql + @") t PIVOT (MAX([INV_AMT]) FOR [INV_NAME] IN (" + ssql + @")) p  where p.IU_Year >= @YearS and p.IU_Week >= @WeekS and p.IU_Year <= @YearE and p.IU_Week <= @WeekE ";
        //                    sql = sql + @" Group by p.[CBD_Case_Name], p.[CBD_SEQ_ID], p.[IU_Year], p.[IU_Week]  Order by p.[CBD_SEQ_ID], p.[IU_Year], p.[IU_Week]";
        //                    break;
        //                case "每月":
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "IU_Month";
        //                    Bfile.HeaderText = "月份";
        //                    GridView1.Columns.Add(Bfile);
        //                    INV_NAME = new string[oDT.Rows.Count];
        //                    ssql3 = ssql3 + @"sum(";
        //                    for (int i = 0; i < oDT.Rows.Count; i++)
        //                    {
        //                        INV_NAME[i] = oDT.Rows[i][0].ToString();
        //                        Bfile = new BoundField();
        //                        Bfile.DataField = @"INV_" + i;
        //                        Bfile.HeaderText = oDT.Rows[i][0].ToString();
        //                        GridView1.Columns.Add(Bfile);
        //                        ssql = ssql + @"[" + oDT.Rows[i][0].ToString() + @"]";
        //                        ssql2 = ssql2 + @"sum(p.[" + oDT.Rows[i][0].ToString() + @"]) as INV_" + i;
        //                        ssql3 = ssql3 + @"isnull(p.[" + oDT.Rows[i][0].ToString() + @"],0)";
        //                        if (i != oDT.Rows.Count - 1)
        //                        {
        //                            ssql = ssql + @", ";
        //                            ssql2 = ssql2 + @", ";
        //                            ssql3 = ssql3 + @"+ ";
        //                        }
        //                    }
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "sumINV";
        //                    Bfile.HeaderText = "總計";
        //                    GridView1.Columns.Add(Bfile);
        //                    ssql3 = ssql3 + @")";
        //                    sql = @"Select p.[CBD_Case_Name], (p.[IU_Year] + p.[IU_Month]) as IU_Month ," + ssql2 + @", " + ssql3 + @" as sumINV,  p.[CBD_SEQ_ID] FROM (SELECT CaseBaseData.CBD_Case_Name ,l.[INV_NAME], l.[DATA_DATE], l.[CBD_SEQ_ID], l.[INV_AMT], l.[IU_Year], l.[IU_Month] FROM [CYPowerReportDB].[dbo].[InvUpload] l inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = l.CBD_SEQ_ID
        //                    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //                    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID";
        //                    //sql = sql + @" and CBD_Case_Name = '" + (ddlCaseName.SelectedIndex < 0 ? "" : ddlCaseName.SelectedItem.ToString().Trim()) + @"'";
        //                    sql += strWhereSQL2 + strWhereSQL3 + sqlWTF4;
        //                    sql = sql + @") t PIVOT (MAX([INV_AMT]) FOR [INV_NAME] IN (" + ssql + @")) p  where p.IU_Year >= @YearS and p.IU_Month >= @MonthS and p.IU_Year <= @YearE and p.IU_Month <= @MonthE ";
        //                    sql = sql + @" Group by p.[CBD_Case_Name], p.[CBD_SEQ_ID], p.[IU_Year], p.[IU_Month]  Order by p.[CBD_SEQ_ID], p.[IU_Year], p.[IU_Month]";
        //                    break;
        //                case "每年":
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "IU_Year";
        //                    Bfile.HeaderText = "年份";
        //                    GridView1.Columns.Add(Bfile);
        //                    INV_NAME = new string[oDT.Rows.Count];
        //                    ssql3 = ssql3 + @"sum(";
        //                    for (int i = 0; i < oDT.Rows.Count; i++)
        //                    {
        //                        INV_NAME[i] = oDT.Rows[i][0].ToString();
        //                        Bfile = new BoundField();
        //                        Bfile.DataField = @"INV_" + i;
        //                        Bfile.HeaderText = oDT.Rows[i][0].ToString();
        //                        GridView1.Columns.Add(Bfile);
        //                        ssql = ssql + @"[" + oDT.Rows[i][0].ToString() + @"]";
        //                        ssql2 = ssql2 + @"sum(p.[" + oDT.Rows[i][0].ToString() + @"]) as INV_" + i;
        //                        ssql3 = ssql3 + @"isnull(p.[" + oDT.Rows[i][0].ToString() + @"],0)";
        //                        if (i != oDT.Rows.Count - 1)
        //                        {
        //                            ssql = ssql + @", ";
        //                            ssql2 = ssql2 + @", ";
        //                            ssql3 = ssql3 + @"+ ";
        //                        }
        //                    }
        //                    Bfile = new BoundField();
        //                    Bfile.DataField = "sumINV";
        //                    Bfile.HeaderText = "總計";
        //                    GridView1.Columns.Add(Bfile);
        //                    ssql3 = ssql3 + @")";
        //                    sql = @"Select p.[CBD_Case_Name], p.[IU_Year]," + ssql2 + @", " + ssql3 + @" as sumINV, p.[CBD_SEQ_ID] FROM (SELECT CaseBaseData.CBD_Case_Name,InvBaseData.IBD_INV_ID ,l.[INV_NAME], l.[DATA_DATE], l.[CBD_SEQ_ID], l.[INV_AMT], l.[IU_Year] FROM [CYPowerReportDB].[dbo].[InvUpload] l inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = l.CBD_SEQ_ID
        //                    inner join InvBaseData on InvBaseData.IBD_INV_NAME= l.INV_NAME
        //                    inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
        //                    inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID";
        //                    //sql = sql + @" and CBD_Case_Name = '" + (ddlCaseName.SelectedIndex < 0 ? "" : ddlCaseName.SelectedItem.ToString().Trim()) + @"'";
        //                    sql += strWhereSQL2 + strWhereSQL3 + sqlWTF4;
        //                    sql = sql + @") t PIVOT (MAX([INV_AMT]) FOR [IBD_INV_ID] IN (" + ssql + @")) p  where p.IU_Year >= @YearS and p.IU_Year <= @YearE ";
        //                    sql = sql + @" Group by p.[CBD_Case_Name], p.[CBD_SEQ_ID], p.[IU_Year] Order by p.[CBD_SEQ_ID], p.[IU_Year]";

        //                    break;
        //            }



        //            bPara.Command = sql;
        //            bPara.Parameter.Clear();
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekS", Getweek.GetWeekOfYear(dDateS, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("WeekE", Getweek.GetWeekOfYear(dDateE, CalendarWeekRule.FirstDay, DayOfWeek.Monday)));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthS", dDateS.Month));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("MonthE", dDateE.Month));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearS", dDateS.Year));
        //            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("YearE", dDateE.Year));
        //            using (oDT2 = bDB.QueryDT(bPara))
        //            {
        //                if (oDT4.Rows.Count == 0)
        //                {
        //                    Chart1.Visible = false;
        //                    Chart2.Visible = false;
        //                    Chart3.Visible = false;
        //                    Chart4.Visible = false;
        //                    Chart5.Visible = false;
        //                }
        //                else
        //                {
        //                    for (int ii = 0; ii < oDT4.Rows.Count; ii++)
        //                    {
        //                        switch (ii)
        //                        {
        //                            case 0:
        //                                Chart1.Visible = true;
        //                                Chart2.Visible = false;
        //                                Chart3.Visible = false;
        //                                Chart4.Visible = false;
        //                                Chart5.Visible = false;
        //                                Chart1.Series.Clear();
        //                                Chart1.Titles.Clear();
        //                                Chart1.Titles.Add(oDT4.Rows[ii][0].ToString().Trim());
        //                                INV_NAME = drawChart(ref sql, dDateS, dDateE, oDT4, ii, Chart1);
        //                                break;
        //                            case 1:
        //                                Chart2.Visible = true;
        //                                Chart3.Visible = false;
        //                                Chart4.Visible = false;
        //                                Chart5.Visible = false;
        //                                Chart2.Series.Clear();
        //                                Chart2.Titles.Clear();
        //                                Chart2.Titles.Add(oDT4.Rows[ii][0].ToString().Trim());
        //                                INV_NAME = drawChart(ref sql, dDateS, dDateE, oDT4, ii, Chart2);
        //                                break;
        //                            case 2:
        //                                Chart3.Visible = true;
        //                                Chart4.Visible = false;
        //                                Chart5.Visible = false;
        //                                Chart3.Series.Clear();
        //                                Chart3.Titles.Clear();
        //                                Chart3.Titles.Add(oDT4.Rows[ii][0].ToString().Trim());

        //                                INV_NAME = drawChart(ref sql, dDateS, dDateE, oDT4, ii, Chart3);
        //                                break;
        //                            case 3:
        //                                Chart4.Visible = true;
        //                                Chart5.Visible = false;
        //                                Chart4.Series.Clear();
        //                                Chart4.Titles.Clear();
        //                                Chart4.Titles.Add(oDT4.Rows[ii][0].ToString().Trim());

        //                                INV_NAME = drawChart(ref sql, dDateS, dDateE, oDT4, ii, Chart4);
        //                                break;
        //                            case 4:
        //                                Chart5.Visible = true;
        //                                Chart5.Series.Clear();
        //                                Chart5.Titles.Clear();
        //                                Chart5.Titles.Add(oDT4.Rows[ii][0].ToString().Trim());
        //                                INV_NAME = drawChart(ref sql, dDateS, dDateE, oDT4, ii, Chart5);
        //                                break;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        return oDT2;
        //    }
        //}

        private void drawChart(DateTime dDateS, DateTime dDateE, string caseName, System.Web.UI.DataVisualization.Charting.Chart tarChart)
        {
            tarChart.Visible = true;
            string[] INV_NAME;
            string sql = "";
            tarChart.Visible = true;
            tarChart.Series.Clear();
            tarChart.Titles.Clear();
            tarChart.Titles.Add(caseName);

            switch (RBL_Type.SelectedItem.ToString())
            {
                case "每日":
                    sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE ";
                    sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                    break;
                case "每週":
                    sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE ";
                    sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                    break;
                case "每月":
                    sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE ";
                    sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                    break;
                case "每年":
                    sql = @"Select * from (Select distinct [INV_NAME] From InvUpload inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                where InvUpload.IU_Year >= @YearS and InvUpload.IU_Year <= @YearE";
                    sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                    break;
            }
            sql = sql + @") A Order by LEN([INV_NAME]), [INV_NAME]";
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
            using (DataTable oDT5 = dDB.QueryDataTable(bPara))
            {
                INV_NAME = new string[oDT5.Rows.Count];
                for (int i = 0; i < oDT5.Rows.Count; i++)
                {
                    INV_NAME[i] = oDT5.Rows[i][0].ToString();
                }
                for (int i = 0; i < INV_NAME.Length; i++)
                {
                    switch (RBL_Type.SelectedItem.ToString())
                    {
                        case "每日":
                            sql = @"SELECT CONVERT(varchar(100), InvUpload.[DATA_DATE], 111) as DATA_DATE, ROUND(isnull([INV_AMT],0), 2) as INV_AMT FROM [InvUpload]
                                                        inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                        inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                        inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                        where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE and InvUpload.[INV_NAME] = @INV_NAME";
                            sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                            sql = sql + @" Order by InvUpload.DATA_DATE";
                            break;
                        case "每週":
                            sql = @"SELECT (InvUpload.[IU_Year] +'-W'+ InvUpload.[IU_Week]) as IU_Week ,ROUND(sum(isnull([INV_AMT],0)), 2) as INV_AMT FROM [InvUpload]
                                                                        inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                                        inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                                        inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                                        where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE and InvUpload.[INV_NAME] = @INV_NAME";
                            sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                            sql = sql + @" Group by InvUpload.[IU_Year], InvUpload.[IU_Week] Order by InvUpload.[IU_Year], InvUpload.[IU_Week]";
                            break;
                        case "每月":
                            sql = @"SELECT (InvUpload.[IU_Year] + InvUpload.[IU_Month]) as IU_Month ,ROUND(sum(isnull([INV_AMT],0)), 2) as INV_AMT FROM [InvUpload]
                                                                        inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                                        inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                                        inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                                        where InvUpload.DATA_DATE >= @DateS and InvUpload.DATA_DATE <= @DateE and InvUpload.[INV_NAME] = @INV_NAME";
                            sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                            sql = sql + @" Group by InvUpload.[IU_Year], InvUpload.[IU_Month] Order by InvUpload.[IU_Year], InvUpload.[IU_Month]";
                            break;
                        case "每年":
                            sql = @"SELECT InvUpload.[IU_Year], ROUND(sum(isnull([INV_AMT],0)), 2) as INV_AMT FROM [InvUpload]
                                                                        inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                                                                        inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner 
                                                                        inner join City on CaseBaseData.CBD_TownShip = City.C_City_ID
                                                                        where InvUpload.IU_Year >= @YearS and InvUpload.IU_Year <= @YearE and InvUpload.[INV_NAME] = @INV_NAME";
                            sql = sql + @" and CBD_Case_Name = '" + caseName + @"'";
                            sql = sql + @" Group by InvUpload.[IU_Year] Order by InvUpload.[IU_Year]";
                            break;
                    }
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
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("INV_NAME", INV_NAME[i].ToString()));
                    //using (DataTable oDT6 = bDB.QueryDT(bPara))
                    using (DataTable oDT6 = dDB.QueryDataTable(bPara))
                    {
                        tarChart.Series.Add(INV_NAME[i].ToString());
                        tarChart.Series[i].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        tarChart.Series[i].BorderWidth = 2;
                        tarChart.Series[i].Legend = "Legends1";
                        tarChart.Series[i].LegendText = INV_NAME[i].ToString();
                        tarChart.Series[i].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                        tarChart.Series[i].MarkerSize = 12;
                        for (int j = 0; j < oDT6.Rows.Count; j++)
                        {
                            tarChart.Series[i].Points.AddXY(oDT6.Rows[j][0].ToString(), float.Parse(oDT6.Rows[j][1].ToString()));
                            tarChart.Series[i].ToolTip = @"逆變器名稱:" + INV_NAME[i].ToString() + @"\n" + @"日期 #VALX 數值:#VALY{F5}";
                        }
                    }
                }
            }
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = false, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
        }

        protected void CBL_CountyID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int CBL_SysTypeCont = 0;
            //int CBL_CountyIDCont = 0;
            //int CBL_BearingCont = 0;
            //for (int i = 0; i < CBL_SysType.Items.Count; i++)
            //{
            //    if (CBL_SysType.Items[i].Selected)
            //    {
            //        CBL_SysTypeCont += 1;
            //    }
            //}
            //for (int i = 0; i < CBL_CountyID.Items.Count; i++)
            //{
            //    if (CBL_CountyID.Items[i].Selected)
            //    {
            //        CBL_CountyIDCont += 1;
            //    }
            //}
            //for (int i = 0; i < CBL_Bearing.Items.Count; i++)
            //{
            //    if (CBL_Bearing.Items[i].Selected)
            //    {
            //        CBL_BearingCont += 1;
            //    }
            //}
            //string sql1 = "";
            //if (CBL_SysTypeCont != 0)
            //{
            //    for (int i = 0; i < CBL_SysType.Items.Count; i++)
            //    {
            //        if (CBL_SysType.Items[i].Selected)
            //        {
            //            if (sql1 == "")
            //            {
            //                sql1 = sql1 + @" and (";
            //            }
            //            else
            //            {
            //                sql1 = sql1 + @" or ";
            //            }
            //            sql1 = sql1 + @"CutomerData.CD_TYPE = '" + CBL_SysType.Items[i].Value.Trim() + "'";
            //        }
            //        if (i == CBL_SysType.Items.Count - 1)
            //        {
            //            sql1 = sql1 + @")";
            //        }
            //    }
            //}
            //string sql2 = "";
            //if (CBL_CountyIDCont != 0)
            //{
            //    for (int i = 0; i < CBL_CountyID.Items.Count; i++)
            //    {
            //        if (CBL_CountyID.Items[i].Selected)
            //        {
            //            if (sql2 == "")
            //            {
            //                sql2 = sql2 + @" and (";
            //            }
            //            else
            //            {
            //                sql2 = sql2 + @" or ";
            //            }
            //            sql2 = sql2 + @"City.C_City_Name = '" + CBL_CountyID.Items[i].Value.Trim() + "'";
            //        }
            //        if (i == CBL_CountyID.Items.Count - 1)
            //        {
            //            sql2 = sql2 + @")";
            //        }
            //    }
            //}
            //string sql3 = "";
            //if (CBL_BearingCont != 0)
            //{
            //    for (int i = 0; i < CBL_Bearing.Items.Count; i++)
            //    {
            //        if (CBL_Bearing.Items[i].Selected)
            //        {
            //            if (sql3 == "")
            //            {
            //                sql3 = sql3 + @" and (";
            //            }
            //            else
            //            {
            //                sql3 = sql3 + @" or ";
            //            }
            //            sql3 = sql3 + @"CaseBaseData.CBD_Bearing = '" + CBL_Bearing.Items[i].Value.Trim() + "'";
            //        }
            //        if (i == CBL_Bearing.Items.Count - 1)
            //        {
            //            sql3 = sql3 + @")";
            //        }
            //    }
            //}
            //string sql = @"SELECT distinct CBD_Case_Name FROM[vInvUpload]
            //inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = vInvUpload.CBD_SEQ_ID
            //inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner inner join City on CaseBaseData.CBD_County_ID = City.C_City_ID WHERE 1 = 1";
            //if (CBL_SysTypeCont != 0 || CBL_CountyIDCont != 0 || CBL_BearingCont != 0)
            //{
            //    sql = sql + sql1 + sql2 + sql3;
            //}
            //bPara.Command = sql;
            //bPara.Parameter.Clear();
            //using (DataTable oDT = bDB.QueryDT(bPara))
            //{
            //    if (oResult.Success)
            //    {
            //        ddlCaseName.Items.Clear();

            //        for (int i = 0; i < oDT.Rows.Count; i++)
            //        {
            //            ddlCaseName.Items.Add(oDT.Rows[i][0].ToString());
            //        }
            //    }
            //}
            getCaseName();
        }

        protected void getCaseName()
        {
            int CBL_SysTypeCont = 0;
            int CBL_CountyIDCont = 0;
            int CBL_BearingCont = 0;
            for (int i = 0; i < CBL_SysType.Items.Count; i++)
            {
                if (CBL_SysType.Items[i].Selected)
                {
                    CBL_SysTypeCont += 1;
                }
            }
            for (int i = 0; i < CBL_CountyID.Items.Count; i++)
            {
                if (CBL_CountyID.Items[i].Selected)
                {
                    CBL_CountyIDCont += 1;
                }
            }
            for (int i = 0; i < CBL_Bearing.Items.Count; i++)
            {
                if (CBL_Bearing.Items[i].Selected)
                {
                    CBL_BearingCont += 1;
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
            string sql2 = "";
            if (CBL_CountyIDCont != 0)
            {
                for (int i = 0; i < CBL_CountyID.Items.Count; i++)
                {
                    if (CBL_CountyID.Items[i].Selected)
                    {
                        if (sql2 == "")
                        {
                            sql2 = sql2 + @" and (";
                        }
                        else
                        {
                            sql2 = sql2 + @" or ";
                        }
                        sql2 = sql2 + @"City.C_City_Name = '" + CBL_CountyID.Items[i].Value.Trim() + "'";
                    }
                    if (i == CBL_CountyID.Items.Count - 1)
                    {
                        sql2 = sql2 + @")";
                    }
                }
            }
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
            string sql = @"SELECT distinct CBD_Case_Name FROM[vInvUpload]
            inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = vInvUpload.CBD_SEQ_ID
            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner inner join City on CaseBaseData.CBD_County_ID = City.C_City_ID WHERE 1 = 1";
            if (CBL_SysTypeCont != 0 || CBL_CountyIDCont != 0 || CBL_BearingCont != 0)
            {
                sql = sql + sql1 + sql2 + sql3;
            }
            //bPara.Command = sql;
            //bPara.Parameter.Clear();
            using (DataTable oDT = dDB.QueryDataTable(sql))
            {
                if (oResult.Success)
                {
                    ddlCaseName.Items.Clear();

                    for (int i = 0; i < oDT.Rows.Count; i++)
                    {
                        ddlCaseName.Items.Add(oDT.Rows[i][0].ToString());
                    }
                }
            }
        }

        protected void CBL_SysType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CBL_SysTypeCont = 0;
            int CBL_CountyIDCont = 0;
            int CBL_BearingCont = 0;
            for (int i = 0; i < CBL_SysType.Items.Count; i++)
            {
                if (CBL_SysType.Items[i].Selected)
                {
                    CBL_SysTypeCont += 1;
                }
            }
            for (int i = 0; i < CBL_CountyID.Items.Count; i++)
            {
                if (CBL_CountyID.Items[i].Selected)
                {
                    CBL_CountyIDCont += 1;
                }
            }
            for (int i = 0; i < CBL_Bearing.Items.Count; i++)
            {
                if (CBL_Bearing.Items[i].Selected)
                {
                    CBL_BearingCont += 1;
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
            string sql2 = "";
            if (CBL_CountyIDCont != 0)
            {
                for (int i = 0; i < CBL_CountyID.Items.Count; i++)
                {
                    if (CBL_CountyID.Items[i].Selected)
                    {
                        if (sql2 == "")
                        {
                            sql2 = sql2 + @" and (";
                        }
                        else
                        {
                            sql2 = sql2 + @" or ";
                        }
                        sql2 = sql2 + @"City.C_City_Name = '" + CBL_CountyID.Items[i].Value.Trim() + "'";
                    }
                    if (i == CBL_CountyID.Items.Count - 1)
                    {
                        sql2 = sql2 + @")";
                    }
                }
            }
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
            string sql = @"SELECT distinct CBD_Case_Name FROM[vInvUpload]
            inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = vInvUpload.CBD_SEQ_ID
            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner inner join City on CaseBaseData.CBD_County_ID = City.C_City_ID WHERE 1 = 1";
            if (CBL_SysTypeCont != 0 || CBL_CountyIDCont != 0 || CBL_BearingCont != 0)
            {
                sql = sql + sql1 + sql2 + sql3;
            }
            //bPara.Command = sql;
            //bPara.Parameter.Clear();
            using (DataTable oDT = dDB.QueryDataTable(sql))
            {
                if (oResult.Success)
                {
                    ddlCaseName.Items.Clear();

                    for (int i = 0; i < oDT.Rows.Count; i++)
                    {
                        ddlCaseName.Items.Add(oDT.Rows[i][0].ToString());
                    }
                }
            }
        }

        protected void CBL_Bearing_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CBL_SysTypeCont = 0;
            int CBL_CountyIDCont = 0;
            int CBL_BearingCont = 0;
            for (int i = 0; i < CBL_SysType.Items.Count; i++)
            {
                if (CBL_SysType.Items[i].Selected)
                {
                    CBL_SysTypeCont += 1;
                }
            }
            for (int i = 0; i < CBL_CountyID.Items.Count; i++)
            {
                if (CBL_CountyID.Items[i].Selected)
                {
                    CBL_CountyIDCont += 1;
                }
            }
            for (int i = 0; i < CBL_Bearing.Items.Count; i++)
            {
                if (CBL_Bearing.Items[i].Selected)
                {
                    CBL_BearingCont += 1;
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
            string sql2 = "";
            if (CBL_CountyIDCont != 0)
            {
                for (int i = 0; i < CBL_CountyID.Items.Count; i++)
                {
                    if (CBL_CountyID.Items[i].Selected)
                    {
                        if (sql2 == "")
                        {
                            sql2 = sql2 + @" and (";
                        }
                        else
                        {
                            sql2 = sql2 + @" or ";
                        }
                        sql2 = sql2 + @"City.C_City_Name = '" + CBL_CountyID.Items[i].Value.Trim() + "'";
                    }
                    if (i == CBL_CountyID.Items.Count - 1)
                    {
                        sql2 = sql2 + @")";
                    }
                }
            }
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
            string sql = @"SELECT distinct CBD_Case_Name FROM[vInvUpload]
            inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = vInvUpload.CBD_SEQ_ID
            inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner inner join City on CaseBaseData.CBD_County_ID = City.C_City_ID WHERE 1 = 1";
            if (CBL_SysTypeCont != 0 || CBL_CountyIDCont != 0 || CBL_BearingCont != 0)
            {
                sql = sql + sql1 + sql2 + sql3;
            }
            //bPara.Command = sql;
            //bPara.Parameter.Clear();
            using (DataTable oDT = dDB.QueryDataTable(sql))
            {
                if (oResult.Success)
                {
                    ddlCaseName.Items.Clear();

                    for (int i = 0; i < oDT.Rows.Count; i++)
                    {
                        ddlCaseName.Items.Add(oDT.Rows[i][0].ToString());
                    }
                }
            }
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
                            //for (int i=0;i< oDT.Rows.Count -1;i++)
                            //{
                            //    ListItem li = new ListItem();
                            //    li.Value = oDT.Rows[i]["C_City_ID"].ToString();
                            //    li.Text = oDT.Rows[i]["C_City_Name"].ToString();
                            //    ddlCity.Items.Add(li);
                            //}
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

                    //bPara.Command = strSQL;
                    //bPara.Result = oResult;
                    ddlCaseBaseData.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(strSQL))
                    {
                        if (oResult.Success)
                        {
                            this.ddlCaseBaseData.DataSource = oDT;
                            ddlCaseBaseData.DataTextField = "CBD_Case_Name";
                            ddlCaseBaseData.DataValueField = "CBD_SEQ_ID";
                            //for (int i=0;i< oDT.Rows.Count -1;i++)
                            //{
                            //    ListItem li = new ListItem();
                            //    li.Value = oDT.Rows[i]["C_City_ID"].ToString();
                            //    li.Text = oDT.Rows[i]["C_City_Name"].ToString();
                            //    ddlCity.Items.Add(li);
                            //}
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
                            //for (int i=0;i< oDT.Rows.Count -1;i++)
                            //{
                            //    ListItem li = new ListItem();
                            //    li.Value = oDT.Rows[i]["C_City_ID"].ToString();
                            //    li.Text = oDT.Rows[i]["C_City_Name"].ToString();
                            //    ddlCity.Items.Add(li);
                            //}
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


        #region "匯出"
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = QuerySourceData(0);

            ToExcel(dt, RBL_Type.SelectedItem.ToString() + "各INV發電量報表_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }


        public void ToExcel(DataTable dt, string fileName)
        {

            string path = Server.MapPath("~/ReportTemplate/各INV發電量報表_範本.xlsx");
            using (Workbook wb = new Workbook())
            {
                wb.LoadFromFile(path);
                Worksheet ws = wb.Worksheets[0];

                DataTable dataTableDistinct = dt.DefaultView.ToTable(true, "CBD_Case_Name");

                int count = dataTableDistinct.Rows.Count;

                DataView dv = new DataView(dt);


                int rowCur = 0;

                for (int i = 0; i < count; i++)
                {

                    string name = dataTableDistinct.Rows[i]["CBD_Case_Name"].ToString();
                    dv.RowFilter = "CBD_Case_Name = '" + name + "'";
                    DataTable dt2 = dv.ToTable();
                    dt2.Columns["CBD_Case_Name"].ColumnName = "案場名稱";
                    dt2.Columns["DATA_DATE"].ColumnName = "日期";
                    dt2.Columns["sum"].ColumnName = "總計";

                    HashSet<string> colNames = new HashSet<string>();
                    foreach (DataColumn dc in dt2.Columns)
                    {
                        if (dt2.AsEnumerable().All(dr => dr.IsNull(dc.ColumnName)))
                            colNames.Add(dc.ColumnName);
                    }
                    foreach (string colName in colNames)
                    {
                        dt2.Columns.Remove(colName);
                    }

                    //ws.Range["3 + rowCur "N"].InsertTable(dt2, false);
                    ws.InsertDataTable(dt2, true, 3 + rowCur, 14);

                    ws.Range[$"R{3 + rowCur}C14:R{3 + rowCur + dt2.Rows.Count}C{dt2.Columns.Count + 13}", true].BorderAround(LineStyleType.Medium, Color.Black);
                    ws.Range[$"R{3 + rowCur}C14:R{3 + rowCur + dt2.Rows.Count}C{dt2.Columns.Count + 13}", true].BorderInside(LineStyleType.Medium, Color.Black);

                    addPic(ws, rowCur, i);
                    rowCur += dt2.Rows.Count + 2;
                }

                Utils.OutputExcelToDownloadSpire(wb, fileName); //匯出
            }
        }

        private void addPic(Worksheet ws, int rowCur, int i)
        {
            if (i < 5)
            {
                MemoryStream pic = new MemoryStream();
                switch (i)
                {
                    case 0:
                        Chart1.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                        break;
                    case 1:
                        Chart2.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                        break;
                    case 2:
                        Chart3.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                        break;
                    case 3:
                        Chart4.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                        break;
                    case 4:
                        Chart5.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                        break;
                }

                ws.Pictures.Add(3 + rowCur, 1, pic, 67, 69, ImageFormatType.Png);
            }
        }
        #endregion

    }
}