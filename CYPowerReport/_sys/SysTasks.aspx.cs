using cyc.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._sys
{
    public partial class SysTasks : cyc.Page.BasePageGridMulti
    {
        protected override System.Data.DataTable QuerySourceData(int idx)
        {
            //return cyc.Global.ObjToDataTable<cyc.AutoTask>(cyc.Global.Tasks);
            return null;
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { CheckOpen = "", Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = null, Refresh = lbRefresh } } };
        }
    }
}