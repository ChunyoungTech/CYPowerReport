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
using Calendar = System.Globalization.Calendar;
using System.Drawing;

namespace WebApp._app
{
    public partial class INVComparisonReport : BasePageGridMulti
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

                bPara.Command = @"SELECT distinct CBD_Case_Name FROM [InvUpload]
                inner join CaseBaseData on CaseBaseData.CBD_SEQ_ID = InvUpload.CBD_SEQ_ID
                inner join CutomerData on CutomerData.CD_SEQ_ID = CaseBaseData.CBD_Case_Owner inner join City on CaseBaseData.CBD_County_ID = City.C_City_ID";
                //where CutomerData.CD_TYPE = '" + CBL_SysType.SelectedItem.ToString().Trim() + @"' and City.C_City_Name = '" + CBL_CountyID.SelectedItem.ToString().Trim() + @"' and CaseBaseData.CBD_Bearing = '" + CBL_Bearing.SelectedItem.ToString().Trim() + @"'";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
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

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;

            if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer))
            {
                if (drv != null)
                {
                    if (double.Parse(drv["INV_KWH_Percent"].ToString().Trim()) < -10)  //條件式
                    {
                        //e.Row.Attributes.Add("style", "background-color:#FF6666"); //新增背景色屬性
                        e.Row.Cells[8].BackColor = System.Drawing.Color.LightPink;
                    }
                    if (double.Parse(drv["INV_KWP_Percent"].ToString().Trim()) < -10)  //條件式
                    {
                        //e.Row.Attributes.Add("style", "background-color:#FF6666"); //新增背景色屬性
                        e.Row.Cells[10].BackColor = System.Drawing.Color.LightPink;
                    }
                }
            }
        }

        protected DataTable QuerySourceDataold(int idx)
        {
            #region whereOption
            string whereCondition = @"
FROM InvUpload IU
JOIN CaseBaseData CBD ON CBD.CBD_SEQ_ID=IU.CBD_SEQ_ID
join InvBaseData as IBD on IBD.ibd_inv_name=IU.inv_name AND ibd.CBD_SEQ_ID=IU.CBD_SEQ_ID
JOIN CutomerData d ON d.CD_SEQ_ID = CBD.CBD_Case_Owner 
WHERE IBD_INV_KW IS not null and IBD_INV_KWP is not null and INV_AMT>0
and IU.DATA_DATE >= @DateS and IU.DATA_DATE <= @DateE
";
            string condition = "";
            //案場
            condition = string.Join(",", ddlCaseBaseData.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and IU.CBD_SEQ_ID in ({condition})";

            //縣市
            condition = string.Join(",", ddlCity.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and CBD.CBD_TownShip in ({condition})";

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


            string sql = $@"
SELECT 
    IU.CBD_SEQ_ID as CBD_Number, 
    CBD.CBD_Case_Name,
    IBD_INV_ID as INV_Number,
    IBD_INV_KW as INV_KW,
    IBD_INV_KWP as INV_KWP,
    DATA_DATE as INV_DATE,
    INV_AMT as INV_Power,
    INV_AMT/IBD_INV_KW AS INV_KWH,
    6.33 AS INV_KWH_Percent,
    INV_AMT/IBD_INV_KWP AS INV_KWHP,
    6.33 AS INV_KWP_Percent
{whereCondition}
";
            bPara.Command = sql;
            bPara.Parameter.Clear();
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));
            //DataTable result = bDB.QueryDT(bPara);
            DataTable result = dDB.QueryDataTable(bPara);

            if (result.Rows.Count < 1)
                return result;

            var query = from item in result.AsEnumerable()
                        group item by new { CBD_Number = item["CBD_Number"], INV_DATE = item["INV_DATE"] } into grouped
                        select new
                        {
                            key = grouped.Key.CBD_Number.ToString() + grouped.Key.INV_DATE.ToString(),
                            INV_KWH_avg = grouped.Average(x => double.Parse(x[7].ToString())),
                            INV_KWP_avg = grouped.Average(x => double.Parse(x[9].ToString()))
                        };
            var qd = query.ToDictionary(q => q.key,
                 q => new { q.INV_KWH_avg, q.INV_KWP_avg });

            foreach (DataRow r in result.Rows)
            {
                string rc = r["CBD_Number"].ToString();
                string rd = r["INV_DATE"].ToString();
                //dynamic q = query.Where(x => x.CBD_Number.ToString() == r["CBD_Number"].ToString() && x.INV_DATE.ToString() == r["INV_DATE"].ToString()).FirstOrDefault();
                var q = qd[rc + rd];

                r["INV_KWH_Percent"] = (double.Parse(r["INV_KWH"].ToString()) - q.INV_KWH_avg) * 100 / q.INV_KWH_avg;
                r["INV_KWP_Percent"] = (double.Parse(r["INV_KWHP"].ToString()) - q.INV_KWP_avg) * 100 / q.INV_KWP_avg;
            }

            //deal with time span
            //result.Columns.Remove("CBD_Number2");
            //result.Columns.Remove("INV_DATE2");
            //result.Columns.Remove("AVG_KW");
            //result.Columns.Remove("AVG_KWP");
            result.Columns.Add("timeSpanId");

            DateTime d;
            switch (RBL_Type.SelectedItem.ToString())
            {
                case "每日":
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year + "/" + d.Month + "/" + d.Day;
                    }

                    //DataTable rCloned = result.Clone();
                    //rCloned.Columns["INV_KWH_Percent"].DataType = typeof(double);
                    //rCloned.Columns["INV_KWP_Percent"].DataType = typeof(double);
                    //foreach (DataRow row in result.Rows)
                    //{
                    //    rCloned.ImportRow(row);
                    //}
                    if (CheckBox1.Checked)
                    {
                        DataView v = new DataView(result);
                        v.RowFilter = "INV_KWH_Percent < -10 OR INV_KWP_Percent < -10";
                        return v.ToTable();
                    }
                    return result;

                case "每週":

                    Calendar c = new GregorianCalendar();
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year + "-" + c.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    }

                    break;
                case "每月":
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year + "-" + d.Month;
                    }
                    break;
                case "每年":
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year;
                    }
                    break;
            }
            //
            DataTable result2 = result.Clone();
            result2.Columns["INV_KWH_Percent"].DataType = typeof(double);
            result2.Columns["INV_KWP_Percent"].DataType = typeof(double);
            DataTable timespans = result.DefaultView.ToTable(true, "timeSpanId", "CBD_Number", "INV_Number");

            DataView dv = new DataView(result);

            foreach (DataRow r in timespans.Rows)
            {
                dv.RowFilter = $"timeSpanId = '{r["timeSpanId"]}' and CBD_Number = {r["CBD_Number"]} and INV_Number = {r["INV_Number"]}";
                // "CBD_Number", "CBD_Case_Name", "INV_Number", "INV_KW", "INV_KWP", "timeSpanId", "INV_Power", "INV_KWH", "INV_KWH_Percent", "INV_KWHP", "INV_KWP_Percent"
                DataTable dt = dv.ToTable();

                DataTable dtCloned = dt.Clone();
                dtCloned.Columns["INV_KWH_Percent"].DataType = typeof(double);
                dtCloned.Columns["INV_KWP_Percent"].DataType = typeof(double);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                DataRow dtr = dt.Rows[0];
                result2.Rows.Add(dtr["CBD_Number"],
                    dtr["CBD_Case_Name"],
                    dtr["INV_Number"],
                    getRound(dt.Compute("avg(INV_KW)", "")),
                                    getRound(dt.Compute("avg(INV_KWP)", "")),
                                    null,
                                    getRound(dt.Compute("avg(INV_Power)", "")),
                                    getRound(dt.Compute("avg(INV_KWH)", "")),
                                    getRound(dtCloned.Compute("avg(INV_KWH_Percent)", "")),
                                    getRound(dt.Compute("avg(INV_KWHP)", "")),
                                    getRound(dtCloned.Compute("avg(INV_KWP_Percent)", "")),
                                    dtr["timeSpanId"]
                                    );
            }

            if (CheckBox1.Checked)
            {
                dv = new DataView(result2);
                dv.RowFilter = "INV_KWH_Percent < -10 OR INV_KWP_Percent < -10";
                return dv.ToTable();
            }

            return result2;
        }

        protected override DataTable QuerySourceData(int idx)
        {
            #region whereOption
            string whereCondition = @"
FROM InvUploadResult IU
JOIN CaseBaseData CBD ON CBD.CBD_SEQ_ID=IU.CBD_Number
join InvBaseData as IBD on IBD.ibd_inv_name=IU.inv_name AND ibd.CBD_SEQ_ID=IU.CBD_Number
JOIN CutomerData d ON d.CD_SEQ_ID = CBD.CBD_Case_Owner 
where IU.INV_DATE >= @DateS and IU.INV_DATE <= @DateE
";
            string condition = "";
            //案場
            condition = string.Join(",", ddlCaseBaseData.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and CBD_Number in ({condition})";

            //縣市
            condition = string.Join(",", ddlCity.Items.Cast<ListItem>().Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and CBD.CBD_TownShip in ({condition})";

            //fix 業主過濾條件 
            condition = string.Join(",", CBL_SysType.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and d.CD_TYPE in ({condition})";

            //fix 監控商過濾條件 
            condition = string.Join(",", CBL_Equipment.Items.Cast<ListItem>().Where(x => x.Selected).Select(x => "'" + x.Value + "'").ToArray<string>());
            if (condition != "")
                whereCondition += $" and CBD_Equipment_Brand in ({condition})";

            //
            if (CheckBox2.Checked)
            {
                whereCondition += $" and ThreeDayErr = 3";
            }
            whereCondition += "order by INV_DATE,CBD_Number,INV_Number";

            #endregion
            DateTime dDateS = Convert.ToDateTime(dteDateS.Text + @" " + @"00:00:00");
            DateTime dDateE = Convert.ToDateTime(dteDateE.Text + @" " + @"23:59:59");


            string sql = $@"
SELECT 
    CBD_Number, 
    IU.CBD_Case_Name,
    INV_Number,
    INV_Name,
    INV_KW,
    INV_KWP,
    INV_DATE,
    INV_Power,
    INV_KWH,
    INV_KWH_Percent * 100 as INV_KWH_Percent,
    INV_KWHP,
    INV_KWP_Percent * 100 as INV_KWP_Percent
{whereCondition}
";
            bPara.Command = sql;
            bPara.Parameter.Clear();
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", dDateS));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", dDateE));
            //DataTable result = bDB.QueryDT(bPara);
            DataTable result = dDB.QueryDataTable(bPara);

            if (result.Rows.Count < 1)
                return result;

            //var query = from item in result.AsEnumerable()
            //            group item by new { CBD_Number = item["CBD_Number"], INV_DATE = item["INV_DATE"] } into grouped
            //            select new
            //            {
            //                key = grouped.Key.CBD_Number.ToString() + grouped.Key.INV_DATE.ToString(),
            //                INV_KWH_avg = grouped.Average(x => double.Parse(x[7].ToString())),
            //                INV_KWP_avg = grouped.Average(x => double.Parse(x[9].ToString()))
            //            };
            //var qd = query.ToDictionary(q => q.key,
            //     q => new { q.INV_KWH_avg, q.INV_KWP_avg });

            //foreach (DataRow r in result.Rows)
            //{
            //    string rc = r["CBD_Number"].ToString();
            //    string rd = r["INV_DATE"].ToString();
            //    //dynamic q = query.Where(x => x.CBD_Number.ToString() == r["CBD_Number"].ToString() && x.INV_DATE.ToString() == r["INV_DATE"].ToString()).FirstOrDefault();
            //    var q = qd[rc + rd];

            //    r["INV_KWH_Percent"] = (double.Parse(r["INV_KWH"].ToString()) - q.INV_KWH_avg) * 100 / q.INV_KWH_avg;
            //    r["INV_KWP_Percent"] = (double.Parse(r["INV_KWHP"].ToString()) - q.INV_KWP_avg) * 100 / q.INV_KWP_avg;
            //}

            //deal with time span
            //result.Columns.Remove("CBD_Number2");
            //result.Columns.Remove("INV_DATE2");
            //result.Columns.Remove("AVG_KW");
            //result.Columns.Remove("AVG_KWP");
            result.Columns.Add("timeSpanId");

            DateTime d;
            switch (RBL_Type.SelectedItem.ToString())
            {
                case "每日":
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year + "/" + d.Month + "/" + d.Day;
                    }

                    //DataTable rCloned = result.Clone();
                    //rCloned.Columns["INV_KWH_Percent"].DataType = typeof(double);
                    //rCloned.Columns["INV_KWP_Percent"].DataType = typeof(double);
                    //foreach (DataRow row in result.Rows)
                    //{
                    //    rCloned.ImportRow(row);
                    //}
                    if (CheckBox1.Checked)
                    {
                        DataView v = new DataView(result);
                        v.RowFilter = "INV_KWH_Percent < -10 OR INV_KWP_Percent < -10";
                        return v.ToTable();
                    }
                    return result;

                case "每週":

                    Calendar c = new GregorianCalendar();
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year + "-" + c.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
                    }

                    break;
                case "每月":
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year + "-" + d.Month;
                    }
                    break;
                case "每年":
                    foreach (DataRow r in result.Rows)
                    {
                        d = DateTime.Parse(r["INV_DATE"].ToString());
                        r["timeSpanId"] = d.Year;
                    }
                    break;
            }
            //
            DataTable result2 = result.Clone();
            result2.Columns["INV_KWH_Percent"].DataType = typeof(double);
            result2.Columns["INV_KWP_Percent"].DataType = typeof(double);
            DataTable timespans = result.DefaultView.ToTable(true, "timeSpanId", "CBD_Number", "INV_Number");

            DataView dv = new DataView(result);

            foreach (DataRow r in timespans.Rows)
            {
                dv.RowFilter = $"timeSpanId = '{r["timeSpanId"]}' and CBD_Number = {r["CBD_Number"]} and INV_Number = {r["INV_Number"]}";
                // "CBD_Number", "CBD_Case_Name", "INV_Number", "INV_KW", "INV_KWP", "timeSpanId", "INV_Power", "INV_KWH", "INV_KWH_Percent", "INV_KWHP", "INV_KWP_Percent"
                DataTable dt = dv.ToTable();

                DataTable dtCloned = dt.Clone();
                dtCloned.Columns["INV_KWH_Percent"].DataType = typeof(double);
                dtCloned.Columns["INV_KWP_Percent"].DataType = typeof(double);
                foreach (DataRow row in dt.Rows)
                {
                    dtCloned.ImportRow(row);
                }

                DataRow dtr = dt.Rows[0];
                result2.Rows.Add(dtr["CBD_Number"],
                    dtr["CBD_Case_Name"],
                    dtr["INV_Number"],
                    dtr["INV_Name"],
                    getRound(dt.Compute("avg(INV_KW)", "")),
                                    getRound(dt.Compute("avg(INV_KWP)", "")),
                                    null,
                                    getRound(dt.Compute("avg(INV_Power)", "")),
                                    getRound(dt.Compute("avg(INV_KWH)", "")),
                                    getRound(dtCloned.Compute("avg(INV_KWH_Percent)", "")),
                                    getRound(dt.Compute("avg(INV_KWHP)", "")),
                                    getRound(dtCloned.Compute("avg(INV_KWP_Percent)", "")),
                                    dtr["timeSpanId"]
                                    );
            }

            if (CheckBox1.Checked)
            {
                dv = new DataView(result2);
                dv.RowFilter = "INV_KWH_Percent < -10 OR INV_KWP_Percent < -10";

                return dv.ToTable();
            }

            return result2;
        }


        private double getRound(object tar)
        {
            return Math.Round(double.Parse(tar.ToString()), 3);
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = false, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
        }

        protected void CBL_CountyID_SelectedIndexChanged(object sender, EventArgs e)
        {
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

                    bPara.Command = strSQL;
                    bPara.Result = oResult;
                    ddlCity.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
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

                    bPara.Command = strSQL;
                    bPara.Result = oResult;
                    ddlCaseBaseData.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
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

            ToExcel(dt, RBL_Type.SelectedItem.ToString() + "各案場INV發電量比較_" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
        }


        public void ToExcel(DataTable dt, string fileName)
        {

            Utils.SetColumnsOrder(dt, new string[] { "CBD_Number", "CBD_Case_Name", "INV_Number", "INV_KW", "INV_KWP", "timeSpanId", "INV_Power", "INV_KWH", "INV_KWH_Percent", "INV_KWHP", "INV_KWP_Percent" });
            dt.Columns.Remove("INV_DATE");
            string path = Server.MapPath("~/ReportTemplate/各案場INV發電量比較_範本.xlsx");

            using (Workbook wb = new Workbook())
            {
                wb.LoadFromFile(path);
                Worksheet ws = wb.Worksheets[0];

                ws.InsertDataTable(dt, false, 2, 1, true);

                ws.Range[$"R1C1:R{1 + dt.Rows.Count}C11", true].BorderAround(LineStyleType.Medium, Color.Black);
                ws.Range[$"R1C1:R{1 + dt.Rows.Count}C11", true].BorderInside(LineStyleType.Medium, Color.Black);

                ConditionalFormatWrapper format1 = ws.Range[$"R2C9:R{1 + dt.Rows.Count}C9", true].ConditionalFormats.AddCondition();
                format1.FirstFormula = "=$I2<-10";
                format1.FormatType = ConditionalFormatType.Formula;
                format1.BackColor = Color.LightPink;

                ConditionalFormatWrapper format2 = ws.Range[$"R2C11:R{1 + dt.Rows.Count}C11", true].ConditionalFormats.AddCondition();
                format2.FirstFormula = "=$K2<-10";
                format2.FormatType = ConditionalFormatType.Formula;
                format2.BackColor = Color.LightPink;

                Utils.OutputExcelToDownloadSpire(wb, fileName); //匯出
            }
            #endregion

        }
    }
}