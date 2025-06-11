using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using cyc.Page;

namespace WebApp._query
{
    /// <summary>
    /// GetReportFile 的摘要描述
    /// </summary>
    public class GetReportFile : cyc.Page.BaseHandler
    {
        protected override void DoHandler(HttpContext context)
        {
            //cyc.DB.SqlDBPara oPara = new cyc.DB.SqlDBPara() { Result = oResult, Object = new { ID = context.Request.QueryString["pa"] } };
            //oPara.Command = "select A.*,B.dept_id,B.report_name from ReportDataExecLog A inner join ReportData B on A.report_data_id=B.ID where A.seq_id=@ID";
            cyc.Data.ReportExecLog xData = null;
            using (var oDB = new cyc.DB.SqlDapperConn(oResult))
            {
                xData = oDB.QueryOne<cyc.Data.ReportExecLog>("select A.*,B.dept_id,B.report_name from ReportDataExecLog A inner join ReportData B on A.report_data_id=B.ID where A.seq_id=@ID", new { ID = context.Request.QueryString["pa"] });
            }
            if (oResult.Success && xData != null)
            {
                if (cyc.UC.DeptControl.CheckDeptLimite(oUser, xData.dept_id))
                {
                    //string sPath = "../ReportDownload/" + xData.report_data_id.ToString() + "/";
                    //string sPath = (cyc.Comm.SysQuery.GetSysSettingValue("ReportPath") + @"\" + xData.report_data_id.ToString() + @"\").Replace(@"\\", @"\");
                    string sPath = (cyc.Comm.SysQuery.GetAppSettingValue("ReportStorePath") + @"\" + xData.report_data_id.ToString() + @"\").Replace(@"\\", @"\");
                    string sFileName = xData.report_name + "_" + xData.exec_time.ToString("yyyyMMddHHmm");
                    string sFile = sPath + xData.file_name;
                    //if (System.IO.File.Exists(context.Server.MapPath(sPath + xData.file_name)))
                    if (System.IO.File.Exists(sFile))
                    {
                        //string sExt = new System.IO.FileInfo(context.Server.MapPath(sPath + xData.file_name)).Extension;
                        string sExt = new System.IO.FileInfo(sFile).Extension;
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + context.Server.UrlPathEncode(sFileName + sExt));
                        context.Response.TransmitFile(sPath + xData.file_name);
                    }
                    else
                        context.Response.Write("檔案不存在");
                }
                else
                    context.Response.Write("非部門權限資料");
            }
        }

        protected override BaseHandlerOption SetBaseOption()
        {
            return new BaseHandlerOption() { Session = true, Parameter = "pa" };
        }
    }
}