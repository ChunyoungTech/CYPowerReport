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
    public partial class SysRole : BasePageGridMulti
    {
        protected override DataTable QuerySourceData(int idx)
        {
            //return cyc.Global.ObjToDataTable<cyc.SysRole>(cyc.Global.SysRole.List);
            return dDB.QueryDataTable("select ID,Name,Enabled,isDefault from SysRole");
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = ucPager, Refresh = lbRefresh } } };
        }
        protected void btnReInit_Click(object sender, EventArgs e)
        {
            cyc.Global.SysRole.Init(dDB, true);
            cyc.Global.SysRoleProg.Init(dDB, true);
            cyc.Global.SysRoleProgSub.Init(dDB, true);
            cyc.Global.SysRoleUser.Init(dDB, true);
        }
    }
}