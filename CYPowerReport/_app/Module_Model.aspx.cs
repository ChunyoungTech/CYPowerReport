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
    public partial class Module_Model : BasePageGridMulti
    {

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //bPara.Command = "SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''";
                //bPara.Parameter.Clear();
            }
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {

            bPara.Command = @"select * from Module_Model where 1=1";
            if (txtModule_Model.Text.Trim().Length > 0) { bPara.Command += " and MM_Module_Model like '%'+@Name+'%'"; }
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtModule_Model.Text.Trim()));

            //return bDB.QueryDT(bPara);
            return dDB.QueryDataTable(bPara);
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh, Excel = btnExport, AutoBind = true } } };
        }

        protected override ExportOption GetExportOption(int idx)
        {
            return new ExportOption()
            {
                Mapping = new string[] { "序號", "名稱", "片數", "停用日期" },
                Column = new string[] { "MM_SEQ_ID", "MM_Module_Model", "MM_Kw_Pis", "MM_StopDate" },
                ColType = new string[] { "s", "s", "s", "s"},
                FileName = "模組型號資料"
            };
        }

        protected void ExampleDownload(object sender, EventArgs e)
        {
            Response.ContentType = "application/download";
            Response.AddHeader("Content-Disposition", $"attachment;filename=\"點位匯入範本.xlsx\"");
            Response.TransmitFile(@"~\example\點位匯入範本.xlsx");
            Response.End();
        }

    }
}