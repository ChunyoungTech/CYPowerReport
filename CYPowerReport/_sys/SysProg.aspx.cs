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
    public partial class SysProg : BasePageGridMulti
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                this.ddlDirQ.DataSource = cyc.Global.SysDir.List;
                this.ddlDirQ.DataBind();
                this.ddlDirQ.Items.Insert(0, "");
            }
            base.OnInit(e);
        }
        protected override DataTable QuerySourceData(int idx)
        {
            return dDB.QueryDataTable(string.Format(@"
select A.ID,A.Name,A.Enabled,A.Seq,B.Name as DirName 
from SysProg A left join SysDir B on A.DirID=B.ID where 1=1 {0} {1} order by B.Seq,A.Seq"
, string.IsNullOrWhiteSpace(ddlDirQ.SelectedValue) ? string.Empty : "and A.DirID=@DirID"
, string.IsNullOrWhiteSpace(ddlEnabledQ.SelectedValue) ? string.Empty : "and A.Enabled=@Enabled")
                , new { DirID = ddlDirQ.SelectedValue, Enabled = ddlEnabledQ.SelectedValue == "1" });

            //return cyc.Global.ObjToDataTable<cyc.SysProg>(cyc.Global.SysProg.List.Where
            //    (p => (ddlDirQ.SelectedValue.Length > 0 ? p.DirID == Convert.ToInt32(ddlDirQ.SelectedValue) : true)
            //    && (ddlEnabledQ.SelectedValue.Length > 0 ? p.Enabled == Convert.ToBoolean(ddlEnabledQ.SelectedValue) : true)).OrderBy(p => p.DirID).ThenBy(p => p.Seq).ToList());
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Excel = btnExport, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh } } };
        }

        protected void btnReInit_Click(object sender, EventArgs e)
        {
            cyc.Global.SysProg.Init(dDB, true);
            cyc.Global.SysProgSub.Init(dDB, true);
        }
    }
}