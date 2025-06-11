using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;

namespace WebApp._sys
{
    public partial class SysOperationLog : BasePageGridMulti
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
                dteDateE.Text = DateTime.Today.ToString("yyyy/MM/dd");
                ddlProgQ.DataSource = cyc.Global.SysProg.List;
                ddlProgQ.DataBind();
                ddlProgQ.Items.Insert(0, "");

                bPara.Command = "select distinct(OPERATION_TYPE)as TYPE from SysOperationLog";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("select distinct(OPERATION_TYPE)as TYPE from SysOperationLog"))
                {
                    if (oResult.Success)
                    {
                        ddlTypeQ.DataSource = oDT;
                        ddlTypeQ.DataBind();
                        ddlTypeQ.Items.Insert(0, "");
                    }
                }
            }
            base.OnInit(e);
        }

        protected override void QueryCheck(int idx)
        {
            if (dteDateS.Text.Trim().Length == 0 || dteDateE.Text.Trim().Length == 0)
            { oResult.Error("[日期區間]不可空白"); }
            else if (!cyc.Comm.Check.IsDateTime(dteDateS.Text) || !cyc.Comm.Check.IsDateTime(dteDateE.Text))
            { oResult.Error("[日期區間]輸入格式錯誤"); }
        }

        protected override DataTable QuerySourceData(int idx)
        {
            DateTime DateS = Convert.ToDateTime(dteDateS.Text);
            DateTime DateE = Convert.ToDateTime(dteDateE.Text).AddDays(1).AddSeconds(-1);

//            bPara.Command = @"
//select A.SEQ_ID,A.OPERATION_TYPE,B.Name as SYS_PROG_ID,C.Name as OPERATION_USER,A.OPERATION_TIME,A.OPERATION_DESC 
//from SysOperationLog A left join SysProg B on A.SYS_PROG_ID=B.ID left join SysUser C on A.OPERATION_USER=C.ID
//where A.OPERATION_TIME between @DateS and @DateE
//";
//            if (ddlProgQ.SelectedValue.Length > 0) { bPara.Command += " and A.SYS_PROG_ID=@Prog"; }
//            if (ddlTypeQ.SelectedValue.Length > 0) { bPara.Command += " and A.OPERATION_TYPE=@Type"; }

//            bPara.Parameter.Clear();
//            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", DateS));
//            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", DateE));
//            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Prog", ddlProgQ.SelectedValue));
//            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Type", ddlTypeQ.SelectedValue));
//            return bDB.QueryDT(bPara);

            return dDB.QueryDataTable(string.Format(@"
select A.SEQ_ID,A.OPERATION_TYPE,B.Name as SYS_PROG_ID,C.Name as OPERATION_USER,A.OPERATION_TIME,A.OPERATION_DESC 
from SysOperationLog A left join SysProg B on A.SYS_PROG_ID=B.ID left join SysUser C on A.OPERATION_USER=C.ID
where A.OPERATION_TIME between @DateS and @DateE {0} {1}"
, string.IsNullOrEmpty(ddlProgQ.SelectedValue) ? string.Empty : "and A.SYS_PROG_ID=@Prog"
, string.IsNullOrEmpty(ddlTypeQ.SelectedValue) ? string.Empty : "and A.OPERATION_TYPE=@Type")
                , new { DateS, DateE, Prog = ddlProgQ.SelectedValue, Type = ddlTypeQ.SelectedValue });
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
        }
    }
}