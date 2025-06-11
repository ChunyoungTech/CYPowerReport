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
    public partial class TagDataSet : BasePageGridMulti
    {
        protected override DataTable QuerySourceData(int idx)
        {
            bPara.Command = @"select * from TagData where 1=1";
            if (ddlTypeQ.SelectedValue.Length > 0) { bPara.Command += " and Tag_Type=@Type"; }
            if (txtNameQ.Text.Trim().Length > 0) { bPara.Command += " and Tag_Name like '%'+@Name+'%'"; }
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Type", ddlTypeQ.SelectedValue));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtNameQ.Text.Trim()));
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
                Mapping = new string[] { "資料點名稱", "資料點描述", "資料點單位", "資料點類型", "AI點的HIHI警報值", "AI點的HI警報值", "AI點的LO警報值", "AI點的LOLO警報值", "opc_name" },
                Column = new string[] { "Tag_Name", "Tag_Desc", "Unit", "Tag_Type", "HiHi_Limit", "Hi_Limit", "Lo_Limit", "LoLo_Limit", "opc_name" },
                ColType = new string[] { "s", "s", "s", "s", "s", "s", "s", "s", "s" },
                FileName = "資料點基本資料"
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