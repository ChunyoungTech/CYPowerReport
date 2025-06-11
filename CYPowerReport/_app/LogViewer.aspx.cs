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
    public partial class LogViewer : BasePageGridMulti
    {

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = DateTime.Today.AddDays(-1).ToString("yyyy/MM/dd");
                dteDateE.Text = DateTime.Today.ToString("yyyy/MM/dd");
                //bPara.Command = "SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''";
                //bPara.Parameter.Clear();

            }
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {
            DateTime dDateS = Convert.ToDateTime(dteDateS.Text);
            DateTime dDateE = (Convert.ToDateTime(dteDateE.Text)).AddDays(1).AddMilliseconds(-1);
            bPara.Command = @"
SELECT * 
from sysgloballog
where SGL_Date between @sDate and @eDate
and SGL_MsgLog like '%'+@searchString +'%'
and SGL_MsgLog <>'背景執行工作中' 
and SGL_MsgLog <>'平均發電量計算成功'
order by SGL_Date desc
";

            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("searchString", searchString.Text));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("sDate", dDateS));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("eDate", dDateE));

            //return bDB.QueryDT(bPara);
            return dDB.QueryDataTable(bPara);
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh, AutoBind = true } } };
        }

        protected override ExportOption GetExportOption(int idx)
        {
            return new ExportOption()
            {
                Mapping = new string[] { "序號", "姓名", "地址", "統一編號", "電話", "EMAIL", "聯絡", "類別" },
                Column = new string[] { "CD_SEQ_ID", "CD_NAME", "CD_ADDRESS", "CD_Uniform_Numbers", "CD_Tel", "CD_EMail", "CD_Contact", "CD_TYPE" },
                ColType = new string[] { "s", "s", "s", "s", "s", "s", "s", "s" },
                FileName = "業主基本資料"
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