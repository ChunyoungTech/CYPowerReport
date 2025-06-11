using cyc.Page;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.IO;
using System.Configuration;

using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;


using NPOI.XSSF.UserModel;

using NPOI.SS.UserModel.Charts;
using NPOI.SS.Util;
using Spire.Xls;
using Spire.Xls.Charts;
using System.Drawing;

namespace WebApp._app
{
    public partial class DailyPowerReport : BasePageGridMulti
    {
        TableCell headerCell_2 = new TableCell();
        DataTable oDT = new DataTable();
        GridViewRow myRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
        GregorianCalendar Getweek = new GregorianCalendar();

        static List<List<string>> dataCache = new List<List<string>>();
        static List<string> rowCache = new List<string>();

        static string dateType = "";
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
                //bPara.Command = "SELECT distinct CBD_Equipment_Brand FROM CaseBaseData ";
                //bPara.Parameter.Clear();
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

        public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        protected override DataTable QuerySourceData(int idx)
        {
            try
            {
                dataCache.Clear();

                DataTable oDT2 = new DataTable();

                oDT2.Columns.Add("DATA_DATE");
                oDT2.Rows.Add("");

                GridView2.DataSource = oDT2;
                GridView2.DataBind();
                //extra process
                //avgs

//                //compare period
//                string condition = "";
//                string whereCondition = @"
//FROM CaseUpload CU
//JOIN CaseBaseData CBD ON CBD.CBD_SEQ_ID=CU.CBD_SEQ_ID
//JOIN CutomerData d ON d.CD_SEQ_ID = CBD.CBD_Case_Owner 
//where CU.DATA_DATE >= @DateS and CU.DATA_DATE <= @DateE
//";
                List<string> ConditionList = new List<string>();
                //案場
                if (ddlCaseBaseData.Items.Count > 0)
                    ConditionList.Add(string.Format(" and cu.CBD_SEQ_ID in ({0})", string.Join(",", ddlCaseBaseData.Items.Cast<ListItem>().Select(p => p.Value))));
                //condition = string.Join(",", ddlCaseBaseData.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
                //if (condition != "")
                //    whereCondition += $" and cu.CBD_SEQ_ID IN ({condition})";

                //縣市
                if (ddlCity.Items.Count > 0)
                    ConditionList.Add(string.Format(" and CBD.CBD_TownShip in ({0})", string.Join(",", ddlCity.Items.Cast<ListItem>().Select(p => string.Format("'{0}'", p.Value)))));
                //condition = string.Join(",", ddlCity.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
                //if (condition != "")
                //    whereCondition += $" and CBD.CBD_TownShip in ({condition})";

                //fix 業主過濾條件 
                if (CBL_SysType.Items.Cast<ListItem>().Any(x => x.Selected))
                    ConditionList.Add(string.Format(" and d.CD_TYPE in ({0})", string.Join(",", CBL_SysType.Items.Cast<ListItem>().Where(x => x.Selected).Select(p => string.Format("'{0}'", p.Value)))));
                //condition = string.Join(",", CBL_SysType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
                //if (condition != "")
                //    whereCondition += $" and d.CD_TYPE in ({condition})";

                //fix 監控商過濾條件 
                if (CBL_Equipment.Items.Cast<ListItem>().Any(x => x.Selected))
                    ConditionList.Add(string.Format(" and CBD_Equipment_Brand in ({0})", string.Join(",", CBL_Equipment.Items.Cast<ListItem>().Where(x => x.Selected).Select(p => string.Format("'{0}'", p.Value)))));
                //condition = string.Join(",", CBL_Equipment.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
                //if (condition != "")
                //    whereCondition += $" and CBD_Equipment_Brand in ({condition})";

//                //find case id
//                bPara.Command = $@"
//SELECT   CU.CBD_SEQ_ID
//         ,CBD.CBD_Case_Name
//         ,avg(upload_pr) AS avg_pr
//{whereCondition}
//GROUP BY CBD_Case_Name,CU.CBD_SEQ_ID";

                bPara.Command = string.Format(@"
SELECT CU.CBD_SEQ_ID,CBD.CBD_Case_Name,avg(upload_pr) AS avg_pr
FROM CaseUpload CU
JOIN CaseBaseData CBD ON CBD.CBD_SEQ_ID=CU.CBD_SEQ_ID
JOIN CutomerData d ON d.CD_SEQ_ID = CBD.CBD_Case_Owner 
where CU.DATA_DATE between @DateS and @DateE {0}
GROUP BY CBD_Case_Name,CU.CBD_SEQ_ID", string.Join(" ", ConditionList));

                bPara.Parameter.Clear();
                DateTime dDateS = Convert.ToDateTime(dteDateS.Text + @" " + @"00:00:00");
                DateTime dDateE = Convert.ToDateTime(dteDateE.Text + @" " + @"23:59:59");
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));

                //DataTable exidDB = bDB.QueryDT(bPara);
                DataTable exidDB = dDB.QueryDataTable(bPara);

                if (exidDB == null || exidDB.Rows.Count == 0)
                {
                    Chart1.Series.Clear();
                    Chart1.Visible = false;
                    oResult.Error("查無符合條件資料");
                    return oDT2;
                }

                //var ids = exidDB.Select("CBD_SEQ_ID").ToList();
                var ids = exidDB.Rows.OfType<DataRow>().Select(dr => dr.Field<int>("CBD_SEQ_ID")).ToList();

                var names = exidDB.Rows.OfType<DataRow>().Select(dr => dr.Field<string>("CBD_Case_Name")).ToList();

                DateTime dDateExtra = Convert.ToDateTime(dteDateE.Text + @" " + @"00:00:00");
                DateTime exDateStart;
                DateTime exDateEnd;

                DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                System.Globalization.Calendar cal = dfi.Calendar;
                //string gridText = "";
                DataTable gd2 = GridView2.DataSource as DataTable;

                List<List<double>> totalRows = new List<List<double>>();

                switch (RBL_Type.SelectedItem.ToString())
                {
                    default:
                    case "每日":
                        List<DateTime> days = new List<DateTime>();
                        while (dDateE > dDateS)
                        {
                            days.Add(dDateS);
                            dDateS = dDateS.AddDays(1);
                        }

                        setupGrid(ids, gd2, days);

                        int d = 0;
                        foreach (DateTime day in days)
                        {
                            List<double> row = getRow(ids, day, day.AddDays(1));
                            d = addData(d, totalRows, row, day.ToString("yyyy/MM/dd"));
                        }
                        setAvg(d, totalRows);
                        exDateStart = new DateTime(dDateExtra.Year, dDateExtra.Month, 1).AddMonths(-1);
                        exDateEnd = new DateTime(dDateExtra.Year, dDateExtra.Month, 1);
                        setLastMonth(ids, exDateStart, exDateEnd, d, exDateStart.Month + "月");
                        setHeaders(exidDB, names, "每日");
                        break;

                    case "每週":
                        List<DateTime> weekstarts = new List<DateTime>();
                        while (dDateE > dDateS)
                        {
                            weekstarts.Add(StartOfWeek(dDateS, DayOfWeek.Sunday));
                            dDateS = dDateS.AddDays(7);
                        }
                        weekstarts.Add(StartOfWeek(dDateS, DayOfWeek.Sunday));

                        setupGrid(ids, gd2, weekstarts);

                        int w = 0;
                        List<List<double>> totalwRows = new List<List<double>>();
                        foreach (DateTime day in weekstarts)
                        {
                            List<double> row = getRow(ids, day, day.AddDays(7));
                            w = addData(w, totalRows, row, day.Year + "-" + (Getweek.GetWeekOfYear(day, CalendarWeekRule.FirstDay, DayOfWeek.Sunday) - 1));
                        }
                        setAvg(w, totalRows);
                        exDateStart = new DateTime(dDateExtra.Year, dDateExtra.Month, 1).AddMonths(-1);
                        exDateEnd = new DateTime(dDateExtra.Year, dDateExtra.Month, 1);
                        setLastMonth(ids, exDateStart, exDateEnd, w, exDateStart.Month + "月");
                        setHeaders(exidDB, names, "每週");
                        break;




                    case "每月":
                        List<DateTime> monthStarts = new List<DateTime>();
                        while (dDateE > dDateS)
                        {
                            monthStarts.Add(new DateTime(dDateS.Year, dDateS.Month, 1));
                            dDateS = dDateS.AddMonths(1);
                        }
                        setupGrid(ids, gd2, monthStarts);

                        int month = 0;
                        foreach (DateTime day in monthStarts)
                        {
                            List<double> row = getRow(ids, day, day.AddMonths(1));
                            month = addData(month, totalRows, row, day.Year + "/" + day.Month);
                        }
                        setAvg(month, totalRows);
                        exDateStart = new DateTime(dDateExtra.Year, dDateExtra.Month, 1).AddMonths(-1);
                        exDateEnd = new DateTime(dDateExtra.Year, dDateExtra.Month, 1);
                        setLastMonth(ids, exDateStart, exDateEnd, month, exDateStart.Month + "月");

                        setHeaders(exidDB, names, "每月");

                        break;



                    case "每年":
                        List<DateTime> yearStarts = new List<DateTime>();
                        while (dDateE > dDateS)
                        {
                            yearStarts.Add(new DateTime(dDateS.Year, 1, 1));
                            dDateS = dDateS.AddYears(1);
                        }
                        setupGrid(ids, gd2, yearStarts);

                        int year = 0;
                        foreach (DateTime day in yearStarts)
                        {
                            List<double> row = getRow(ids, day, day.AddYears(1));
                            year = addData(year, totalRows, row, day.Year.ToString());
                        }
                        setAvg(year, totalRows);
                        exDateStart = new DateTime(dDateExtra.Year, 1, 1).AddYears(-1);
                        exDateEnd = new DateTime(dDateExtra.Year, 1, 1);
                        setLastMonth(ids, exDateStart, exDateEnd, year, exDateStart.Year + "年");
                        setHeaders(exidDB, names, "每年");
                        break;

                }

                //chart
                if (rowCache.Count > 0)
                {
                    Chart1.Series.Clear();
                    Chart1.Visible = true;
                    int i = 0;
                    foreach (string name in names)
                    {
                        Chart1.Series.Add(name + " -PR值");
                        Chart1.Series.Add(name + " -發電小時");
                        Chart1.Series[i * 2].LegendText = name + " -PR值";
                        Chart1.Series[i * 2 + 1].LegendText = name + " -發電小時";
                        Chart1.Series[i * 2].BorderWidth = 2;
                        Chart1.Series[i * 2 + 1].BorderWidth = 2;
                        Chart1.Series[i * 2].Legend = "Legends1";
                        Chart1.Series[i * 2 + 1].Legend = "Legends1";
                        Chart1.Series[i * 2].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Column;
                        Chart1.Series[i * 2 + 1].ChartType = System.Web.UI.DataVisualization.Charting.SeriesChartType.Line;
                        Chart1.Series[i * 2].YAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Primary;
                        Chart1.Series[i * 2 + 1].YAxisType = System.Web.UI.DataVisualization.Charting.AxisType.Secondary;
                        Chart1.Series[i * 2 + 1].MarkerStyle = System.Web.UI.DataVisualization.Charting.MarkerStyle.Circle;
                        Chart1.Series[i * 2 + 1].MarkerSize = 12;

                        Chart1.Series[i * 2].ToolTip = @"案場名稱:" + name + @"\n" + Chart1.Series[i * 2].LegendText + @": 日期 #VALX 數值:#VALY{F5}";
                        Chart1.Series[i * 2 + 1].ToolTip = @"案場名稱:" + name + @"\n" + Chart1.Series[i * 2 + 1].LegendText + @": 日期 #VALX 數值:#VALY{F5}";


                        i++;
                    }

                    List<string> rowData;
                    string x = "";

                    //skip  avg and lastmonth
                    for (i = 0; i < dataCache.Count - 2; i++)
                    {
                        //remove date and total
                        rowData = dataCache[i].ToList();
                        x = rowData[0];
                        rowData.RemoveAt(0);
                        rowData.RemoveAt(rowData.Count - 1);

                        for (int j = 0; j < (rowData.Count / 3); j++)
                        {
                            float temp;
                            float.TryParse(rowData[j * 3].ToString(), out temp);
                            Chart1.Series[j * 2].Points.AddXY(x, temp);
                            float.TryParse(rowData[j * 3 + 1].ToString(), out temp);
                            Chart1.Series[j * 2 + 1].Points.AddXY(x, temp);
                        }
                    }
                }

                return oDT2;
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult); }

            //got ids 
            return new DataTable();
        }

        private int addData(int timestep, List<List<double>> totalRows, List<double> row, string firstCell)
        {
            rowCache = new List<string>();
            totalRows.Add(row);
            int i = 1;
            GridView2.Rows[timestep].Cells[0].Text = firstCell;
            rowCache.Add(firstCell);
            foreach (double v in row)
            {
                GridView2.Rows[timestep].Cells[i].Text = v.ToString();
                rowCache.Add(v.ToString());
                i++;
            }
            dataCache.Add(rowCache);
            timestep++;
            return timestep;
        }

        private void setAvg(int timestep, List<List<double>> totalRows)
        {
            //getAvg
            rowCache = new List<string>();
            GridView2.Rows[timestep].Cells[0].Text = "平均";
            rowCache.Add("平均");
            for (int i = 0; i < totalRows[0].Count; i++)
            {
                GridView2.Rows[timestep].Cells[i + 1].Text = totalRows.Average(innerList => innerList[i]).ToString("0.#####");
                rowCache.Add(GridView2.Rows[timestep].Cells[i + 1].Text);
            }
            dataCache.Add(rowCache);
        }

        private List<double> setLastMonth(List<int> ids, DateTime exDateStart, DateTime exDateEnd, int timestep, string firstCell)
        {
            rowCache = new List<string>();
            timestep++;
            GridView2.Rows[timestep].Cells[0].Text = firstCell;
            rowCache.Add(firstCell);
            List<double> lastMonthAvg = getRow(ids, exDateStart, exDateEnd);
            int i = 1;
            foreach (double v in lastMonthAvg)
            {
                GridView2.Rows[timestep].Cells[i].Text = v.ToString();
                rowCache.Add(GridView2.Rows[timestep].Cells[i].Text);
                i++;
            }
            dataCache.Add(rowCache);

            return lastMonthAvg;
        }

        private void setupGrid(List<int> ids, DataTable gd2, List<DateTime> days)
        {
            for (int i = 0; i < (ids.Count * 3) + 1; i++)
            {
                BoundField test = new BoundField();
                GridView2.Columns.Add(test);
                gd2.Columns.Add(i.ToString());
            }

            foreach (DateTime day in days)
            {
                gd2.Rows.Add(new string[(ids.Count * 3) + 1]);
            }
            gd2.Rows.Add(new string[(ids.Count * 3) + 1]);
            GridView2.DataSource = gd2;
            GridView2.DataBind();
        }

        private void setHeaders(DataTable exidDB, List<string> names, string gridText)
        {
            rowCache = new List<string>();
            dateType = gridText;
            rowCache.AddRange(names);
            GridViewRow myRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            GridViewRow myRow2 = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            TableCell head2Cell = new TableCell();

            head2Cell = new TableCell();
            head2Cell.Text = "日期";
            head2Cell.ForeColor = System.Drawing.Color.White;
            head2Cell.Font.Bold = true;
            head2Cell.BackColor = System.Drawing.Color.FromArgb(0x1c5e55);
            head2Cell.HorizontalAlign = HorizontalAlign.Center;
            head2Cell.RowSpan = 2;
            myRow.Cells.Add(head2Cell);
            for (int i = 0; i < exidDB.Rows.Count; i++)
            {
                head2Cell = new TableCell();
                head2Cell.Text = names[i];
                head2Cell.ForeColor = System.Drawing.Color.White;
                head2Cell.Font.Bold = true;
                head2Cell.BackColor = System.Drawing.Color.FromArgb(0x1c5e55);
                head2Cell.ColumnSpan = 3;
                head2Cell.HorizontalAlign = HorizontalAlign.Center;
                myRow.Cells.Add(head2Cell);
                head2Cell = new TableCell();
                head2Cell.Text = gridText + "平均<br />PR值";
                head2Cell.ForeColor = System.Drawing.Color.White;
                head2Cell.Font.Bold = true;
                head2Cell.BackColor = System.Drawing.Color.FromArgb(0x1c5e55);
                head2Cell.HorizontalAlign = HorizontalAlign.Center;
                myRow2.Cells.Add(head2Cell);
                head2Cell = new TableCell();
                head2Cell.Text = gridText + "平均<br />發電小時";

                head2Cell.ForeColor = System.Drawing.Color.White;
                head2Cell.Font.Bold = true;
                head2Cell.BackColor = System.Drawing.Color.FromArgb(0x1c5e55);
                head2Cell.HorizontalAlign = HorizontalAlign.Center;
                myRow2.Cells.Add(head2Cell);
                head2Cell = new TableCell();
                head2Cell.Text = gridText + "加總<br />發電量";

                head2Cell.ForeColor = System.Drawing.Color.White;
                head2Cell.Font.Bold = true;
                head2Cell.BackColor = System.Drawing.Color.FromArgb(0x1c5e55);
                head2Cell.HorizontalAlign = HorizontalAlign.Center;
                myRow2.Cells.Add(head2Cell);
            }
            head2Cell = new TableCell();
            head2Cell.Text = RBL_Type.SelectedItem.ToString() + "加總<br />發電量";
            head2Cell.ForeColor = System.Drawing.Color.White;
            head2Cell.Font.Bold = true;
            head2Cell.RowSpan = 2;
            head2Cell.BackColor = System.Drawing.Color.FromArgb(0x1c5e55);
            head2Cell.HorizontalAlign = HorizontalAlign.Center;
            myRow.Cells.Add(head2Cell);


            GridView2.HeaderRow.Parent.Controls.AddAt(0, myRow);
            GridView2.HeaderRow.Parent.Controls.AddAt(1, myRow2);
        }

        private List<double> getRow(List<int> ids, DateTime exDateStart, DateTime exDateEnd)
        {
            bPara.Command = $@"
SELECT t1.CBD_SEQ_ID, CBD_Case_Name ,isnull(avg_pr,0) AS avg_pr,isnull(avg_rate,0) AS avg_rate,isnull(sum_amt,0)AS sum_amt
from
(
SELECT    ISNULL(avg(upload_pr),0) AS avg_pr
         ,ISNULL(avg(upload_rate),0) AS avg_rate
         ,ISNULL(sum(upload_amt),0) AS sum_amt
         ,CU.CBD_SEQ_ID

FROM CaseUpload CU
JOIN CaseBaseData CBD ON CBD.CBD_SEQ_ID=CU.CBD_SEQ_ID
JOIN CutomerData d ON d.CD_SEQ_ID = CBD.CBD_Case_Owner 
where CU.DATA_DATE >= @DateS and CU.DATA_DATE < @DateE
GROUP BY CBD_Case_Name,cu.CBD_SEQ_ID

) t1
right join 
(SELECT   CBD.CBD_SEQ_ID, CBD.CBD_Case_Name 
    
FROM CaseBaseData CBD
where CBD.CBD_SEQ_ID IN ({string.Join(",", ids)})
) AS t2
ON t1.CBD_SEQ_ID= t2.CBD_SEQ_ID
ORDER BY cbd_case_name
";

            bPara.Parameter.Clear();
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", exDateStart));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", exDateEnd));

            //DataTable exDB = bDB.QueryDT(bPara);
            DataTable exDB = dDB.QueryDataTable(bPara);
            List<double> lastAvgs = new List<double>();
            double totalSum = 0;
            foreach (DataRow r in exDB.Rows)
            {

                lastAvgs.Add(double.Parse(r["avg_pr"].ToString()));
                lastAvgs.Add(double.Parse(r["avg_rate"].ToString()));
                //sum and avg by original row count
                double singleSum = double.Parse(r["sum_amt"].ToString());
                lastAvgs.Add(singleSum);
                totalSum += singleSum;
            }
            lastAvgs.Add(totalSum);

            return lastAvgs;
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = false, Grid = null, Pager = null, Query = btnQuery, Refresh = lbRefresh } } };
        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {
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

                    bPara.Command = strSQL;
                    bPara.Result = oResult;
                    ddlCity.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(bPara))
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
                    using (DataTable oDT = dDB.QueryDataTable(bPara))
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



        #region "匯出"
        protected void btnExport_Click(object sender, EventArgs e)
        {
            int i, j;
            //int intRowIndex;
            int intColumnIndex;

            //string strValue;

            string sTemplateFile = Server.MapPath("~/ReportTemplate/每日發電量報表_範本.xlsx");
            if (!File.Exists(sTemplateFile))
            {
                oResult.Error("範本檔案不存在");
            }

            else
            {
                try
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(sTemplateFile);
                    Worksheet sheet = workbook.Worksheets[0];
                    MemoryStream pic = new MemoryStream();
                    Chart1.SaveImage(pic, System.Web.UI.DataVisualization.Charting.ChartImageFormat.Png);
                    sheet.Pictures.Add(3, 1, pic, 65, 69, ImageFormatType.Png);

                    intColumnIndex = 15;

                    string label1 = sheet.Range["R4C15", true].Value;
                    string label2 = sheet.Range["R4C16", true].Value;
                    string label3 = sheet.Range["R4C17", true].Value;

                    sheet.Range["R4C15", true].Value = dateType + sheet.Range["R4C15", true].Value;
                    sheet.Range["R4C16", true].Value = dateType + sheet.Range["R4C16", true].Value;
                    sheet.Range["R4C17", true].Value = dateType + sheet.Range["R4C17", true].Value;
                    i = 0;
                    j = 0;
                    foreach (string name in rowCache)
                    {
                        j = i * 3;
                        sheet.Range[3, j + intColumnIndex].Value = rowCache[i];


                        sheet.Range[3, j + intColumnIndex, 3, j + intColumnIndex + 2].Merge();
                        sheet.Copy(sheet.Range["R4C15:R4C17", true], sheet.Range[$"R4C{intColumnIndex + j }:R4C{intColumnIndex + j + 2}", true]);

                        i++;
                    }

                    sheet.Range[3, j + intColumnIndex + 3].Value = dateType + "加總";
                    sheet.Range[3, j + intColumnIndex + 3, 4, j + intColumnIndex + 3].Merge();

                    i = 5;
                    foreach (List<string> data in dataCache)
                    {
                        sheet.Range[i, 14].Value = data[0];
                        data.RemoveAt(0);
                        sheet.InsertArray(data.ToArray(), i++, 15, false);
                    }

                    string indexR = (dataCache.Count + 4).ToString();
                    string indexC = (dataCache[0].Count + 14).ToString();
                    sheet.Range[$"R3C14:R{indexR}C{indexC}", true].BorderAround(LineStyleType.Medium, Color.Black);
                    sheet.Range[$"R3C14:R{indexR}C{indexC}", true].BorderInside(LineStyleType.Medium, Color.Black);



                    //save and launch the file
                    string strFileName = RBL_Type.SelectedItem.ToString() + "發電量報表_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
                    Utils.OutputExcelToDownloadSpire(workbook, strFileName); //匯出

                }
                catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace, oResult); }
            }

        }

        #endregion
    }
}