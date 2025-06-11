using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;

namespace WebApp._app
{
    public partial class OperationHistory : BasePageGridMulti
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = DateTime.Today.AddDays(-7).ToString("yyyy/MM/dd");
                dteDateE.Text = DateTime.Today.ToString("yyyy/MM/dd");

                GetProgQ();
                GetTypeQ();
            }
            base.OnInit(e);
        }

        protected void GetProgQ()
        {
            //ddlProgQ.DataSource = cyc.Global.SysProg;
            //ddlProgQ.DataBind();
            //ddlProgQ.Items.Insert(0, "");
            bPara.Command = @"select ID,Name
                from SysProg
                where isnull(maintern_table,'')<>''
                order by ID";
            bPara.Parameter.Clear();
            using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
            {
                if (oResult.Success)
                {
                    ddlProgQ.DataSource = oDT;
                    ddlProgQ.DataTextField = "Name";
                    ddlProgQ.DataValueField = "ID";
                    ddlProgQ.DataBind();
                    ddlProgQ.Items.Insert(0, "");
                }
            }
        }

        protected void GetTypeQ()
        {
            bPara.Command = @"select TYPE_ID
	            ,(case TYPE_ID when 'insert' then '新增' when 'update' then '修改' else '未定義' end) as TYPE_NAME
	            from (
		            select distinct(OPERATION_TYPE) as TYPE_ID 
			            from SysOperationLog
	            ) as Q";
            //bPara.Parameter.Clear();
            using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
            {
                if (oResult.Success)
                {
                    ddlTypeQ.DataSource = oDT;
                    ddlTypeQ.DataTextField = "TYPE_NAME";
                    ddlTypeQ.DataValueField = "TYPE_ID";
                    ddlTypeQ.DataBind();
                    ddlTypeQ.Items.Insert(0, "");
                }
            }
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
            DateTime DateE = Convert.ToDateTime(dteDateE.Text);
            string strSQL = @"
                select A.SEQ_ID
	                ,(case A.OPERATION_TYPE when 'insert' then '新增' when 'update' then '修改' 
		                else '未定義' end) as OPERATION_TYPE
                    ,B.Name as SYS_PROG_ID,C.Name as OPERATION_USER
                    ,A.OPERATION_TIME,A.OPERATION_DESC 
	                ,(isnull(B.maintern_table,'') + '-' + OPERATION_KEY) as '編號'
                    from SysOperationLog A 
                    left join SysProg B on A.SYS_PROG_ID=B.ID 
	                left join SysUser C on A.OPERATION_USER=C.ID
                    where A.OPERATION_TIME between @DateS and @DateE
                ";
            bPara.Parameter.Clear();

            if (ddlProgQ.SelectedValue.Length > 0) {
                strSQL += " and A.SYS_PROG_ID=@Prog";
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Prog", ddlProgQ.SelectedValue));

            }
            if (ddlTypeQ.SelectedValue.Length > 0) {
                strSQL += " and A.OPERATION_TYPE=@Type";
                bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Type", ddlTypeQ.SelectedValue));
            }

            strSQL+= " order by A.SEQ_ID desc";
            bPara.Command = strSQL;

            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateS", DateS));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("DateE", DateE.AddDays(1).AddSeconds(-1)));


            //return bDB.QueryDT(bPara);
            return dDB.QueryDataTable(bPara);
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
        }
    }
}