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
    public partial class SysUser : BasePageGridMulti
    {
        protected override DataTable QuerySourceData(int idx)
        {
            //bPara.Command = @"select A.*,B.Name as DeptName from SysUser A left join SysDept B on A.DeptID=B.ID where 1=1";
            //bPara.Command += ddlDeptQ.GetQuerySQL("A.DeptID", "and");
            //if (this.txtNameQ.Text.Trim().Length > 0) { bPara.Command += " and A.Name like '%'+@Name+'%'"; }
            //bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", this.txtNameQ.Text.Trim()));
            //return bDB.QueryDT(bPara);
            return dDB.QueryDataTable(string.Format("select A.*,B.Name as DeptName from SysUser A left join SysDept B on A.DeptID=B.ID where 1=1 {0} {1}"
                , ddlDeptQ.GetQuerySQL("A.DeptID", "and")
                , string.IsNullOrWhiteSpace(txtNameQ.Text) ? string.Empty : "and A.Name like '%'+@Name+'%'")
                , new { Name = txtNameQ.Text });
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
        }
    }
}