using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;

namespace WebApp._app
{
    public partial class CaseBaseData : BasePageGridMulti
    {

        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {

                bPara.Command = "SELECT COD_SEQ_ID,COD_NAME FROM CompanyData";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        CompanyList.DataSource = oDT;
                        CompanyList.DataBind();
                        CompanyList.Items.Insert(0, "");
                    }
                }

                bPara.Command = "SELECT sysType FROM vSysType WHERE sysType IS NOT NULL AND sysType<>''";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        VCDType.DataSource = oDT;
                        VCDType.DataBind();
                        VCDType.Items.Insert(0, "");
                    }
                }

                bPara.Command = "SELECT CD_SEQ_ID,CD_NAME FROM CutomerData order by CD_SEQ_ID";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        VOwner.DataSource = oDT;
                        VOwner.DataBind();
                        VOwner.Items.Insert(0, "");
                    }
                }

                bPara.Command = "SELECT C_City_ID,C_City_Name FROM City where LEN(C_City_ID)=2 order by C_City_ID";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        VCity.DataSource = oDT;
                        VCity.DataBind();
                        VCity.Items.Insert(0, "");
                    }
                }

                VType.Items.Insert(0, "");
                VType.Items.Insert(1, "低壓");
                VType.Items.Insert(2, "高壓");


                bPara.Command = "SELECT CD_SEQ_ID,CD_NAME,CD_TYPE FROM CutomerData order by CD_SEQ_ID";
                bPara.Parameter.Clear();
                DataTable dt_questionVideo = dDB.QueryDataTable(bPara.Command);
                string createArrayScript = "var cd_data = [";
                for (var i = 0; i <= dt_questionVideo.Rows.Count - 1; i++)
                {
                    if (i == dt_questionVideo.Rows.Count - 1)
                    {
                        createArrayScript += "[" + dt_questionVideo.Rows[i]["CD_SEQ_ID"] + ",\"" + dt_questionVideo.Rows[i]["CD_NAME"] + "\",\"" + dt_questionVideo.Rows[i]["CD_TYPE"] + "\"]];";
                    }
                    else
                    {
                        createArrayScript += "[" + dt_questionVideo.Rows[i]["CD_SEQ_ID"] + ",\"" + dt_questionVideo.Rows[i]["CD_NAME"] + "\",\"" + dt_questionVideo.Rows[i]["CD_TYPE"] + "\"],";
                    }
                }
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "registerVideoQArray", createArrayScript, true);

            }
            base.OnInit(e);
        }


        protected override DataTable QuerySourceData(int idx)
        {

            bPara.Command = @"select CBD.*,C.C_City_Name,C2.C_City_Name as CBD_TownName,CD.CD_NAME,CD.CD_TYPE,MM.MM_Module_Model ,COD_SEQ_ID,COD.COD_NAME
from CaseBaseData CBD 
left join City C on CBD_County_ID=C_City_ID 
left join City C2 on CBD_TownShip=C2.C_City_ID 
left join CutomerData CD on CBD_Case_Owner=CD_SEQ_ID 
left join Module_Model MM on CBD.CBD_Module_Model=MM.MM_SEQ_ID 
left join CompanyData COD on CBD.CompanyID=COD.COD_SEQ_ID
where 1=1
";
            if (txtCaseName.Text.Trim().Length > 0) { bPara.Command += " and CBD_Case_Name like '%'+@Name+'%'"; }
            if (CompanyList.SelectedValue.Length > 0) { bPara.Command += " and CBD.CompanyID=@COD_SEQ_ID"; }
            if (VOwner.SelectedValue.Length > 0) { bPara.Command += " and CBD_Case_Owner=@Owner"; }
            if (VCity.SelectedValue.Length > 0) { bPara.Command += " and CBD_County_ID=@County"; }
            if (VType.SelectedValue.Length > 0) { bPara.Command += " and CBD_Voltage_Type=@Type"; }
            if (VCDType.SelectedValue.Length > 0) { bPara.Command += " and CD_Type=@CDType"; }

            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtCaseName.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("COD_SEQ_ID", CompanyList.SelectedValue));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Owner", VOwner.SelectedValue));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("County", VCity.SelectedValue));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Type", VType.SelectedValue));
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
            {//"s":文字、"i":數字、"t":日期、"p":百分比、"y":是否
                Mapping = new string[] { "案場名稱", "公司類別", "案場業主", "監控廠商", "縣市", "鄉鎮", "地址", "定位點", "KW數", "片數", "模組型號", "方位", "案場類型", "電壓型態", "備註" },
                Column = new string[] { "CBD_Case_Name", "COD_NAME", "CD_NAME", "CBD_Equipment_Brand", "C_City_Name", "CBD_TownName", "CBD_Address", "CBD_GPS", "CBD_KW", "CBD_Slices", "MM_Module_Model", "CBD_Bearing", "CBD_Case_Type", "CBD_Voltage_Type", "CBD_Remarks" },
                ColType = new string[] { "s", "s", "s", "s", "s", "s", "s", "s", "i", "i", "s", "s", "s", "s", "s" },
                FileName = "案場基本資料"
            };
        }

    }
}