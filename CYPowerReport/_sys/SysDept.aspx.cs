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
    public partial class SysDept : BasePageGridMulti
    {
        protected override DataTable QuerySourceData(int idx)
        {
            //bPara.Command = @"select A.*,B.Name as UpperName from SysDept A left join SysDept B on A.UpperID=B.ID where 1=1";
            //bPara.Command += ddlDeptQ.GetQuerySQL("A.ID", "and");
            //return bDB.QueryDT(bPara);
            return dDB.QueryDataTable(string.Format("select A.*,B.Name as UpperName from SysDept A left join SysDept B on A.UpperID=B.ID where 1=1 {0}", ddlDeptQ.GetQuerySQL("A.ID", "and")));
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = null } } };
        }

        protected void lbRefresh_Click(object sender, EventArgs e)
        {
            int iID = ddlDeptQ.DeptID;
            ddlDeptQ.Reset();
            ddlDeptQ.DeptID = iID;

            BindGridView();
        }
    }
}