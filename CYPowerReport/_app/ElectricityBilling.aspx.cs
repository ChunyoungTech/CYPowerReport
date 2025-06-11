using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.IO;
using System.Configuration;

namespace WebApp._app
{
    public partial class ElectricityBilling : BasePageGridMulti
    {
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
            {
                dteDateS.Text = (DateTime.Today.Year.ToString() + "/01/01");
                dteDateE.Text = DateTime.Today.ToString("yyyy/MM/dd");

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

                #region 業主類別

                bPara.Command = "select '' as TypeID,'' as TypeName union " +
                    "select distinct CD_TYPE as TypeID,CD_TYPE as TypeName" +
                    " from vCaseDataALL" +
                    " where isnull(CD_TYPE,'')<>''";
                bPara.Parameter.Clear();
                using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                {
                    if (oResult.Success)
                    {
                        ddlCD_TYPE.DataSource = oDT;
                        ddlCD_TYPE.DataTextField = "TypeName";
                        ddlCD_TYPE.DataValueField = "TypeID";
                        ddlCD_TYPE.DataBind();
                    }
                }

                #endregion 業主類別
            }
            base.OnInit(e);
        }

        protected override DataTable QuerySourceData(int idx)
        {
            try
            {
                string strSQL = @"
SELECT  EC_SEQ_ID,
CompanyName,
Address,
EC_Meter_Record,
EC_Line_Loss,
EC_Calculation_Record,
EC_Calculation_Rate,
EC_Calculation_Amt,
InvoicePrice + BusinessTax AS InvoicePrice,
MeterRent,
InvoiceNumber,
TotalPrice,
RemittanceFee,
ActualIncome,
LandOwner,
SetAmount,
ExtraPay,
EC_Days,
DayAvg,
BankType,
ECDepartment,
ECMonth,
Comment,
BusinessTax,
BankAccount,
EC_Daily_Amount,
EC_Duarantee_Rate,
EC_Daily_Billing,
EC_Check_Amount,
CBD_Case_Name
,convert(varchar(10),EC_Last_Meter_Reading,111) as Last_Meter_Reading
,convert(varchar(10),EC_Next_Meter_Reading,111) as Next_Extra_Date
,convert(varchar(10),EC_Current_Meter_Reading,111) as Current_Extra_Date
,convert(varchar(10),DATEADD(day,-1,EC_Current_Meter_Reading),111) as Current_Meter_Reading
,convert(varchar(10),MailDate,111) as MailDate
,convert(varchar(10),InvoiceDate,111) as InvoiceDate
,CBD_Case_Name
,CBD_KW
,COD_NAME
,companyId
,electricNumber
,convert(varchar(10),meterUpDate,111) as meterUpDate
from ElectricityBilling as EB
inner join vCaseDataALL as CBD on CBD.CBD_SEQ_ID = EB.CBD_ID where 1=1
                ";
                if (dteDateS.Text != "" && dteDateE.Text != "")
                {
                    strSQL += " and (convert(varchar(10),EC_Current_Meter_Reading,111) between @strDateS and @strDateE)";
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("@strDateS", dteDateS.Text));
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("@strDateE", dteDateE.Text));
                }
                if (invoiceDateS.Text != "" && invoiceDateE.Text != "")
                {
                    strSQL += " and (convert(varchar(10),InvoiceDate,111) between @invoiceDateS and @invoiceDateE)";
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("@invoiceDateS", invoiceDateS.Text));
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("@invoiceDateE", invoiceDateE.Text));
                }

                
                if (ddlCD_TYPE.SelectedValue.Length > 0)
                {
                    strSQL += " and CD_TYPE=@CD_TYPE";
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("@CD_TYPE", ddlCD_TYPE.SelectedValue));
                }
                if (CompanyList.SelectedValue.Length > 0)
                {
                    strSQL += " and CompanyID=@COD_SEQ_ID";
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("COD_SEQ_ID", CompanyList.SelectedValue));
                }

                if (txtCase_Name.Text.Trim().Length > 0)
                {
                    strSQL += " and CBD_Case_Name like '%'+@Name+'%'";
                    bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("@Name", txtCase_Name.Text.Trim()));
                }
                bPara.Command = strSQL;

                DataTable dt = dDB.QueryDataTable(bPara);
                return dt;
            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace); }
            return null;
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh, Excel = btnExport, AutoBind = true } } };
        }

        protected override ExportOption GetExportOption(int idx)
        {
            var cols = new List<string[]>
            {
                new string[] { "序號","EC_SEQ_ID","s"},
                new string[] { "公司名稱","CompanyName","s"},
                new string[] { "設置地點","Address","s"},
                new string[] { "本次抄表日期","Current_Extra_Date","s"},
                new string[] { "下次抄表日期","Next_Extra_Date","s"},
                new string[] { "購電度數","EC_Meter_Record",",i"},
                new string[] { "線路損失率","EC_Line_Loss","i"},
                new string[] { "計算度數","EC_Calculation_Record",",i"},
                new string[] { "費率","EC_Calculation_Rate","i"},
                new string[] { "金額(未稅)","EC_Calculation_Amt",",i"},
                new string[] { "營業稅","BusinessTax",",i"},
                new string[] { "發票(含稅)","InvoicePrice",",i"},
                new string[] { "電表租費","MeterRent",",i"},
                new string[] { "發票日期","InvoiceDate","s"},
                new string[] { "發票號碼","InvoiceNumber","s"},
                new string[] { "應付款金額","TotalPrice",",i"},
                new string[] { "匯費","RemittanceFee",",i"},
                new string[] { "實際入帳金額","ActualIncome",",i"},
                new string[] { "地主","LandOwner","s"},
                new string[] { "設置容量(KW)","SetAmount","i"},
                new string[] { "計費期間-起","Last_Meter_Reading","s"},
                new string[] { "計費期間-迄","Current_Meter_Reading","s"},
                new string[] { "計費天數","EC_Days",",i"},
                new string[] { "日平均","DayAvg",",i000"},
                new string[] { "銀行別","BankType","s"},
                new string[] { "掛號寄出日期","MailDate","s"},
                new string[] { "台電區域","ECDepartment","s"},
                new string[] { "台電計費月份","ECMonth","s"},
                new string[] { "備註","Comment","s"},
                new string[] { "銀行帳戶","BankAccount","s"},
                new string[] { "日金額","EC_Daily_Amount",",i"},
                new string[] { "日保發度數","EC_Duarantee_Rate",",i"},
                new string[] { "日計費度數","EC_Daily_Billing",",i000"},
                new string[] { "驗算","EC_Check_Amount",",i000"},
                new string[] { "案場名稱","CBD_Case_Name","s"},
                new string[] { "公司類別","COD_NAME","s"},
                new string[] { "補付(扣)電費", "Extrapay",",i"},
                new string[] { "電號", "electricNumber","s"},
                new string[] { "掛錶日期", "meterUpDate","s"},
            };

            string[] Mapping = cols.Select(arr => arr[0]).ToArray();

            return new ExportOption()
            {
                Mapping = Mapping,

                Column = cols.Select(arr => arr[1]).ToArray(),

                ColType = cols.Select(arr => arr[2]).ToArray(),

                FileName = "電費報表資料"
            };
        }

        protected void ExampleDownload(object sender, EventArgs e)
        {
            Response.ContentType = "application/download";
            Response.AddHeader("Content-Disposition", $"attachment;filename=\"點位匯入範本.xlsx\"");
            Response.TransmitFile(@"~\example\點位匯入範本.xlsx");
            Response.End();
        }

        protected void lkbReCityData_Click(object sender, EventArgs e)
        {
            string strValue = "";
            strValue = hidCity.Value;
            string[] arrCityValue;
            string strSQL = "";
            string strWhereSQL = "";
            try
            {
                if (strValue != "")
                {
                    arrCityValue = strValue.Split('*');
                    strSQL = "select C_City_ID,C_City_Name from City";
                    for (int i = 0; i < arrCityValue.Length; i++)
                    {
                        if (i == 0) { strWhereSQL += " where C_City_ID in ("; }
                        strWhereSQL += "'" + arrCityValue[i] + "',";
                    }
                    if (strWhereSQL != "")
                    {
                        strWhereSQL = strWhereSQL.Substring(0, strWhereSQL.Length - 1);
                        strWhereSQL += ")";
                    }
                    strSQL += strWhereSQL;

                    bPara.Command = strSQL;
                    bPara.Result = oResult;
                    ddlCity.Items.Clear();
                    using (DataTable oDT = dDB.QueryDataTable(bPara.Command))
                    {
                        if (oResult.Success)
                        {
                            this.ddlCity.DataSource = oDT;
                            ddlCity.DataTextField = "C_City_Name";
                            ddlCity.DataValueField = "C_City_ID";
                            //for (int i=0;i< oDT.Rows.Count -1;i++)
                            //{
                            //    ListItem li = new ListItem();
                            //    li.Value = oDT.Rows[i]["C_City_ID"].ToString();
                            //    li.Text = oDT.Rows[i]["C_City_Name"].ToString();
                            //    ddlCity.Items.Add(li);
                            //}
                            this.ddlCity.DataBind();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErr(ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString());
            }
        }

        /// <summary>
        ///  將錯誤訊息寫到Log檔案，放到專案底下的\ErrLog
        /// </summary>
        /// <param name="strError">錯誤訊息</param>
        protected void WriteErr(string strError)
        {
            string strFilePath = "";
            try
            {
                strFilePath = ConfigurationManager.AppSettings["ErrLogPath"].ToString() + "/ErrLog/";

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                System.Web.HttpRequest request = HttpContext.Current.Request;
                string strRawUrl = request.RawUrl;
                strError = strRawUrl + "\r\n" + strError;

                File.WriteAllText(strFilePath + "/Err_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", strError);
            }
            catch (Exception ex)
            {
                strFilePath = "c:/ErrLog/";

                if (!Directory.Exists(strFilePath))
                {
                    Directory.CreateDirectory(strFilePath);
                }

                System.Web.HttpRequest request = HttpContext.Current.Request;
                string strRawUrl = request.RawUrl;
                strError = strRawUrl + "\r\n" + strError + "\r\n" + ex.Message.ToString() + "\r\n" + ex.StackTrace.ToString();

                File.WriteAllText(strFilePath + "/Err_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".txt", strError);
            }
        }
    }
}