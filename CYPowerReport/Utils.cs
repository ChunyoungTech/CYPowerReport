using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class Utils
    {
        static public void OutputExcelToDownloadSpire(Workbook workbook, String FileName)
        {
            try
            {
                MemoryStream MS = new MemoryStream();

                //將WorkBook寫入MemoryStream
                workbook.SaveToStream(MS);

                //輸出
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xlsx");
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(MS.ToArray());
                workbook = null;
                MS.Close();
                MS.Dispose();
                //https://blog.darkthread.net/blog/response-end-alternative/ 
                ////以下兩行一定要有，否則產出的檔案size會比較大，且用Excel開啟會報錯(雖然還是可以修復後開啟)
                //System.Web.HttpContext.Current.Response.Flush();
                //System.Web.HttpContext.Current.Response.End();

                //將Buffer中的內容送出
                HttpContext.Current.Response.Flush();
                //忽視之後透過Response.Write輸出的內容
                HttpContext.Current.Response.SuppressContent = true;
                //忽略之後ASP.NET Pipeline的處理步驟，直接跳關到EndRequest
                HttpContext.Current.ApplicationInstance.CompleteRequest();


            }
            catch (Exception ex) { cyc.Global.WriteSysError(ex.Message + ":" + ex.StackTrace); }
        }

        public static void SetColumnsOrder(DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;
            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }
    }
}