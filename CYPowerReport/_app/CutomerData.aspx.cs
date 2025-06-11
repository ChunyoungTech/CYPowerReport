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
    public partial class CutomerData : BasePageGridMulti
    {

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                //bPara.Command = "SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''";
                //bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable("SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''"))
                {
                    if (oResult.Success)
                    {
                        VCDType.DataSource = oDT;
                        VCDType.DataBind();
                        VCDType.Items.Insert(0, "");
                    }
                }
            }
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {

            bPara.Command = @"select * from CutomerData where 1=1";
            if (txtName.Text.Trim().Length > 0) { bPara.Command += " and CD_NAME like '%'+@Name+'%'"; }
            if (txtUniformNumbers.Text.Trim().Length > 0) { bPara.Command += " and CD_Uniform_Numbers = @UniformNumbers"; }
            if (txtTel.Text.Trim().Length > 0) { bPara.Command += " and CD_Tel like '%'+@Tel+'%'"; }
            if (VCDType.SelectedValue.Length > 0) { bPara.Command += " and CD_Type=@CDType"; }
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtName.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("UniformNumbers", txtUniformNumbers.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Tel", txtTel.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("CDType", VCDType.SelectedValue));
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
                Mapping = new string[] { "序號", "姓名", "地址", "統一編號", "電話", "EMAIL", "聯絡", "類別" },
                Column = new string[] { "CD_SEQ_ID", "CD_NAME", "CD_ADDRESS", "CD_Uniform_Numbers", "CD_Tel", "CD_EMail", "CD_Contact", "CD_TYPE" },
                ColType = new string[] { "s", "s", "s", "s", "s", "s", "s", "s"},
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