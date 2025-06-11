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
    public partial class CompanyData : BasePageGridMulti
    {

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {

            bPara.Command = @"select * from CompanyData where 1=1";
            if (txtName.Text.Trim().Length > 0) { bPara.Command += " and COD_NAME like '%'+@Name+'%'"; }
            if (txtUniformNumbers.Text.Trim().Length > 0) { bPara.Command += " and COD_Uniform_Numbers = @UniformNumbers"; }
            if (txtTel.Text.Trim().Length > 0) { bPara.Command += " and COD_Tel like '%'+@Tel+'%'"; }
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtName.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("UniformNumbers", txtUniformNumbers.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Tel", txtTel.Text.Trim()));
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
                FileName = "公司基本資料"
            };
        }
    }
}