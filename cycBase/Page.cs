using System;
using System.Web.UI;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.IO;
using cyc.Comm;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;
using cyc.UC;
using cyc.DB;

namespace cyc.Page
{
    /// <summary>
    /// 基礎頁面
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        protected ExeResult oResult = new ExeResult();
        protected UserInfo bUser = null;//使用者資訊

        protected override void OnInit(EventArgs e)
        {
            if (Session["uid"] != null) { bUser = (UserInfo)Session["uid"]; }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        //protected override void OnError(EventArgs e)
        //{
        //    Exception exc = Server.GetLastError();
        //    if (exc is InvalidOperationException)
        //    {
        //        Server.Transfer("~/error/error.aspx", true);
        //    }
        //    base.OnError(e);
        //}

        #region MessageBox

        protected void MsgBox(string sMsg, string sURL = "", bool bReload = false, bool bReturn = false)
        {
            sMsg = sMsg.Replace("'", "\'").Replace(Environment.NewLine, " \n");
            sMsg = string.Format("ClientAlert('{0}');", sMsg);
            if (sURL.Length > 0)
                sMsg += "document.location.href='" + sURL + "';";

            if (bReload && bReturn)
                sMsg += "parent.CloseAndReload(1,1);window.stop ? window.stop() : document.execCommand('Stop');";
            else
            {
                if (bReload)
                    sMsg += "parent.CloseAndReload(0,1);";
                //sMsg += "$('#hidRefresh', window.parent.document).val('1');";
                if (bReturn)
                    sMsg += "parent.jQuery.fancybox.close();";
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alert", sMsg, true);
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", sMsg, true);
        }

        protected virtual void ShowResult(string sMsg, bool bReload, bool bReturn, string sUrl = "")
        {
            if (oResult.Success)
                MsgBox(sMsg, sUrl, bReload, bReturn);
            else
                MsgBox(oResult.Message);
        }

        #endregion
    }

    /// <summary>
    /// 基礎頁面，含DB Connect
    /// </summary>
    public class BasePageDB : BasePage
    {
        private SqlDapperConn _dDB = null;
        //private SqlDBConn _bDB = null;
        //protected SqlDBConn bDB { get { if (_bDB == null) { _bDB = new SqlDBConn(); } return _bDB; } }
        protected SqlDapperConn dDB { get { if (_dDB == null) { _dDB = new SqlDapperConn(oResult); } return _dDB; } }
        protected SqlDBPara bPara = new SqlDBPara();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            bPara.Result = oResult;
        }

        protected override void OnUnload(EventArgs e)
        {
            CloseConnection();//關閉頁面連線
            base.OnUnload(e);
        }

        protected override void OnError(EventArgs e)
        {
            CloseConnection();//關閉頁面連線
            base.OnError(e);
        }
        private void CloseConnection()
        {
            if (_dDB != null) { _dDB.Dispose(); }
            //if (_bDB != null) { _bDB.Dispose(); }
        }

        //protected bool GetUserProgAll(int sApp)
        //{
        //    var mProg = (from lsR in gObj.SysRoleProg
        //                 join lsU in bUser.UserRole on lsR.RoleID equals lsU.ID
        //                 where lsR.ProgID == Convert.ToInt16(Request.QueryString["app"])
        //                 select lsR).FirstOrDefault(p => p.isAllSub == true);
        //    return mProg != null;
        //}
    }

    /// <summary>
    /// 基礎 資料查詢清單 頁面(多清單)
    /// </summary>
    public abstract class BasePageGridMulti : BasePageDB
    {
        public delegate void delBindGridView(int idx, string sSort = null);
        public delegate void delCreateExcel(int idx);
        //public delegate DataTable delQuerySource(int idx);

        protected GridPageSetting oSetting;
        protected abstract DataTable QuerySourceData(int idx);//取得GridView資料來源
        protected abstract GridPageSetting SetPageSetting();

        protected virtual ExportOption GetExportOption(int idx)
        {
            return null;
        }

        /// <summary>
        /// 檢查查詢條件，可複寫
        /// </summary>
        protected virtual void QueryCheck(int idx) { }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            oSetting = SetPageSetting();
            if (oSetting != null)
            {
                if (oSetting.CheckSession && !cyc.Comm.Login.CheckSession())
                    Response.Redirect("~/login.aspx?rtn=" + Server.UrlEncode(Request.RawUrl));

                if (!IsPostBack && oSetting.CheckOpen.Length > 0 && System.IO.Path.GetFileName(Request.PhysicalPath) != oSetting.CheckOpen)
                    Response.End();

                if (oSetting.Option != null)
                {
                    foreach (GridOption opt in oSetting.Option)
                    {
                        if (opt.Grid != null) { opt.Grid.DataBound += Grid_DataBound; opt.Grid.Sorting += Grid_Sorting; }
                        if (opt.Pager != null) { opt.Pager.PageChanged += Pager_PageChanged; }
                        if (opt.Query != null) { opt.Query.Click += Query_Click; }
                        if (opt.Refresh != null) { opt.Refresh.Click += Refresh_Click; }
                        if (opt.Excel != null) { opt.Excel.Click += Excel_Click; }
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Page.IsPostBack)
            {
                BasePageFunc.AutoBindGrid(oSetting, BindGridView);
            }
        }

        /// <summary>
        /// GridView生成後觸發
        /// </summary>
        private void Grid_DataBound(object sender, EventArgs e)
        {
            BasePageFunc.Grid_DataBound(oSetting, (GridView)sender);
        }

        /// <summary>
        /// GridView排序觸發
        /// </summary>
        private void Grid_Sorting(object sender, GridViewSortEventArgs e)
        {
            BasePageFunc.Grid_Sorting(oSetting, (GridView)sender, e, BindGridView);
        }

        /// <summary>
        /// Pager換頁事件觸發
        /// </summary>
        private void Pager_PageChanged(object sender, PagerChangeArgs e)
        {
            BasePageFunc.Pager_PageChanged(oSetting, (ucPager)sender, e, BindGridView);
        }

        /// <summary>
        /// 查詢按鍵用
        /// </summary>
        private void Query_Click(object sender, EventArgs e)
        {
            BasePageFunc.Query_Click(oSetting, (Button)sender, BindGridView);
        }

        /// <summary>
        /// 匯出EXCEL用
        /// </summary>
        private void Excel_Click(object sender, EventArgs e)
        {
            BasePageFunc.Excel_Click(oSetting, (Button)sender, CreateExcel);
        }

        /// <summary>
        /// 前端重整GridView用
        /// </summary>
        protected void Refresh_Click(object sender, EventArgs e)
        {
            BasePageFunc.Refresh_Click(oSetting, (LinkButton)sender, BindGridView);
        }

        /// <summary>
        /// 取得及設定排序欄位
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="column">排序欄位</param>
        /// <returns></returns>
        protected string GetSort(int idx = 0, string column = null)
        {
            string sDirection = "SortDirection" + idx.ToString();
            string sExpression = "SortExpression" + idx.ToString();

            if (ViewState[sExpression] == null && column == null)
                return "";
            else if (column != null)
            {
                if (ViewState[sExpression] != null && ViewState[sExpression].ToString() == column)
                    ViewState[sDirection] = (ViewState[sDirection] == null || ViewState[sDirection].ToString() == "DESC") ? "ASC" : "DESC";
                else
                    ViewState[sExpression] = column;
            }
            if (ViewState[sDirection] == null) { ViewState[sDirection] = "ASC"; }
            return ViewState[sExpression].ToString() + " " + ViewState[sDirection].ToString();
        }

        /// <summary>
        /// 綁定 GridView 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="sSort"></param>
        protected void BindGridView(int idx = 0, string sSort = null)
        {
            QueryCheck(idx);
            if (oResult.Success)
            {
                try
                {
                    using (DataTable oDT = this.QuerySourceData(idx))
                    {
                        if (oResult.Success && oSetting.Option[idx].Grid != null)
                        {
                            oDT.DefaultView.Sort = GetSort(idx, sSort);
                            oSetting.Option[idx].Grid.DataSource = oDT;
                            oSetting.Option[idx].Grid.DataBind();
                            if (oSetting.Option[idx].Pager != null) { oSetting.Option[idx].Pager.showTotalCnt(oDT.Rows.Count); }
                        }
                    }
                }
                catch (Exception ex) { Global.WriteSysError(ex.Message, oResult); }
            }
            if (!oResult.Success) { MsgBox(oResult.Message); }
        }

        /// <summary>
        /// 產生 Excel 檔案
        /// </summary>
        /// <param name="idx"></param>
        protected void CreateExcel(int idx)
        {

            using (DataTable oDT = this.QuerySourceData(idx))
            {
                using (ExportExcel excel = new ExportExcel(GetExportOption(idx), oDT.DefaultView))
                {
                    if (excel.Workbook != null)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            excel.Workbook.Write(ms);
                            Response.Clear();
                            Response.AddHeader("Content-Disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode((excel.Option.FileName == "" ? "匯出清冊" : excel.Option.FileName) + ".xls"));
                            Response.ContentType = "application/octet-stream";
                            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                            Response.OutputStream.Flush();
                            Response.OutputStream.Close();
                        }
                        Response.Flush();
                        Response.End();
                    }
                }
            }

        }
    }

    /// <summary>
    /// BasePage 共用Function
    /// </summary>
    public class BasePageFunc
    {
        public static void Query_Click(GridPageSetting oSetting, Button sender, BasePageGridMulti.delBindGridView bindGrid)
        {
            var opt = oSetting.Option.FirstOrDefault(p => p.Query != null && p.Query.ID == sender.ID);
            bindGrid(Array.IndexOf(oSetting.Option, opt));
        }

        public static void Refresh_Click(GridPageSetting oSetting, LinkButton sender, BasePageGridMulti.delBindGridView bindGrid)
        {
            var opt = oSetting.Option.FirstOrDefault(p => p.Refresh != null && p.Refresh.ID == sender.ID);
            bindGrid(Array.IndexOf(oSetting.Option, opt));
        }

        public static void Grid_Sorting(GridPageSetting oSetting, GridView sender, GridViewSortEventArgs e, BasePageGridMulti.delBindGridView bindGrid)
        {
            var opt = oSetting.Option.FirstOrDefault(p => p.Grid != null && p.Grid.ID == sender.ID);
            bindGrid(Array.IndexOf(oSetting.Option, opt), e.SortExpression);
        }

        public static void Grid_DataBound(GridPageSetting oSetting, GridView sender)
        {
            var opt = oSetting.Option.FirstOrDefault(p => p.Pager != null && p.Pager.TargetID == sender.ID);
            if (opt != null) { opt.Pager.Refresh(); }
        }

        public static void Pager_PageChanged(GridPageSetting oSetting, ucPager sender, PagerChangeArgs e, BasePageGridMulti.delBindGridView bindGrid)
        {
            var opt = oSetting.Option.FirstOrDefault(p => p.Pager != null && p.Pager.ID == sender.ID && p.Grid != null);
            if (opt != null)
            {
                opt.Grid.PageIndex = e.CurrentPage - 1;
                opt.Grid.PageSize = e.PageSize;
                bindGrid(Array.IndexOf(oSetting.Option, opt));
            }
        }

        public static void Excel_Click(GridPageSetting oSetting, Button sender, BasePageGridMulti.delCreateExcel createExcel)
        {
            var opt = oSetting.Option.FirstOrDefault(p => p.Excel != null && p.Excel.ID == sender.ID);
            if (opt != null) { createExcel(Array.IndexOf(oSetting.Option, opt)); }
        }

        public static void AutoBindGrid(GridPageSetting oSetting, BasePageGridMulti.delBindGridView bindGrid)
        {
            if (oSetting.Option != null)
            {
                foreach (GridOption opt in oSetting.Option)
                    if (opt.AutoBind && opt.Grid != null)
                        bindGrid(Array.IndexOf(oSetting.Option, opt));
            }
        }

        public static void CheckEditValid(EditPageOption oOption, System.Web.UI.Page oPage, ExeResult oResult)
        {
            if (oResult.Success && (oOption.Session || oOption.Guid || oOption.Parameter.Length > 0 || oOption.Parent || oOption.Opener))
            {
                if (oResult.Success && oOption.Session)
                {
                    if (!cyc.Comm.Login.CheckSession())
                        oResult.Error("未登入或逾時登出");
                    else if (oOption.Guid) { }
                }
                if (oResult.Success && oOption.Parent)
                {
                    if (oPage.Request.UrlReferrer != null)
                    {
                        //if (Request.Url.Authority != Request.UrlReferrer.Authority) { oResult.Error("來源頁有誤"); }
                        if (oPage.Request.Url.Host != oPage.Request.UrlReferrer.Host) { oResult.Error("來源頁有誤"); }
                    }
                    else { oResult.Error("來源頁有誤"); }
                }
                if (oResult.Success && oOption.Opener)
                {
                    if (oPage.PreviousPage != null && oPage.PreviousPage is System.Web.UI.Page)
                    {
                        if (oPage.Request.Url.Host != oPage.PreviousPage.Request.Url.Host) { oResult.Error("來源頁有誤"); }
                        //if (Request.Url.Authority != PreviousPage.Request.Url.Authority) { oResult.Error("來源頁有誤"); }
                        //if (oResult.Success) { }
                    }
                    else { oResult.Error("來源頁有誤"); }
                }
                if (oResult.Success && oOption.Parameter.Length > 0)
                {
                    string[] sPara = oOption.Parameter.Split(',');
                    foreach (string pa in sPara)
                        if (string.IsNullOrEmpty(oPage.Request.QueryString[pa]))
                        { oResult.Error("參數錯誤"); break; }

                    if (oResult.Success && oOption.IsIntPa && !string.IsNullOrEmpty(oPage.Request.QueryString["pa"]))
                        if (!Check.IsInteger(oPage.Request.QueryString["pa"]))
                            oResult.Error("參數錯誤");
                }
            }
        }

        public static string ReCheckAuth(string sKey, string sGuid, ExeResult oResult)
        {
            if (sKey.Length == 0 || sKey != sGuid) { oResult.Error("認證失敗"); }
            return "";
        }
    }

    /// <summary>
    /// DataView 匯出 Excel
    /// </summary>
    public class ExportExcel : IDisposable
    {
        public ExportOption Option { get; set; }
        public HSSFWorkbook Workbook { get; set; }

        public ExportExcel(ExportOption opt, DataView oDV)
        {
            Option = opt;
            if (Option != null && Option.ColType.Length > 0 && Option.Column.Length > 0 && Option.Mapping.Length > 0 && Option.ColType.Length == Option.Column.Length && Option.ColType.Length == Option.Mapping.Length)
            {
                Workbook = new HSSFWorkbook();
                ISheet sheet1 = Workbook.CreateSheet(Option.FileName == "" ? "sheet1" : Option.FileName);

                // 创建一个单元格样式，并设置千分位格式
                ICellStyle commaStyle = Workbook.CreateCellStyle();
                commaStyle.DataFormat = Workbook.CreateDataFormat().GetFormat("#,##0");

                // 创建一个单元格样式，并设置千分位格式
                ICellStyle commaDecimalStyle = Workbook.CreateCellStyle();
                commaDecimalStyle.DataFormat = Workbook.CreateDataFormat().GetFormat("#,##0.###");

                int idx = 0;
                int idx2 = 0;

                int idxRow = 0;
                IRow header = sheet1.CreateRow(idxRow);
                for (idx = 0; idx < Option.Mapping.Length; idx++)
                    header.CreateCell(idx).SetCellValue(Option.Mapping[idx]);
                idxRow++;


                try
                {
                    for (idx = 0; idx < oDV.Count; idx++)
                    {
                        IRow datarow = sheet1.CreateRow(idxRow);
                        for (idx2 = 0; idx2 < Option.Mapping.Length; idx2++)
                        {
                            var cell = datarow.CreateCell(idx2);
                            if (oDV[idx][Option.Column[idx2]] is DBNull)
                            {
                                cell.SetCellValue("");
                            }
                            else
                            {
                                switch (Option.ColType[idx2])
                                {
                                    case "s"://文字
                                        cell.SetCellValue(oDV[idx][Option.Column[idx2]].ToString());
                                        break;
                                    case ",i"://

                                        cell.SetCellValue(Convert.ToDouble(oDV[idx][Option.Column[idx2]]));
                                        cell.CellStyle = commaStyle;
                                        break;
                                    case ",i000"://

                                        cell.SetCellValue(Convert.ToDouble(oDV[idx][Option.Column[idx2]]));
                                        cell.CellStyle = commaDecimalStyle;
                                        break;
                                    case "i"://數字
                                        cell.SetCellValue(Convert.ToDouble(oDV[idx][Option.Column[idx2]]));
                                        break;
                                    case "t"://日期
                                        if (oDV[idx][Option.Column[idx2]].ToString() != "")
                                            cell.SetCellValue(Convert.ToDateTime(oDV[idx][Option.Column[idx2]]).ToString("yyyy/MM/dd"));
                                        break;
                                    case "p"://百分比
                                        cell.SetCellValue(string.Format("{0:P2}", Convert.ToDouble(oDV[idx][Option.Column[idx2]])));
                                        break;
                                    case "y"://是否
                                        if (Convert.ToBoolean(oDV[idx][Option.Column[idx2]]))
                                            cell.SetCellValue("是");
                                        break;
                                    default://文字
                                        cell.SetCellValue(oDV[idx][Option.Column[idx2]].ToString());
                                        break;
                                }
                            }
                        }
                        idxRow++;
                    }
                }
                catch (Exception ex)
                {
                    cyc.Global.WriteSysError(oDV[idx][Option.Column[idx2]].ToString() + "  " + ex.Message);

                }
                //for (int idx = 0; idx < Option.Mapping.Length; idx++)
                //    sheet1.AutoSizeColumn(idx, true);
            }
        }

        public void Dispose()
        {
            if (Workbook != null) { Workbook = null; }
        }
    }

    #region 類別定義

    public class ExportOption
    {
        public string[] Mapping { get; set; }
        public string[] Column { get; set; }
        public string[] ColType { get; set; }
        public string FileName { get; set; }
    }

    /// <summary>
    /// GridView 頁面 設定
    /// </summary>
    public class GridPageSetting
    {
        public bool CheckSession { get; set; }
        public bool CheckGuid { get; set; }
        public string CheckOpen { get; set; }
        public GridOption[] Option { get; set; }
        public GridPageSetting() { CheckSession = true; CheckGuid = false; CheckOpen = "index.aspx"; }
    }

    /// <summary>
    /// GridView相對應控制項
    /// </summary>
    public class GridOption
    {
        public GridView Grid { get; set; }
        public Button Excel { get; set; }
        public ucPager Pager { get; set; }
        public Button Query { get; set; }
        public LinkButton Refresh { get; set; }
        public bool AutoBind { get; set; }
        public GridOption() { AutoBind = false; }
    }

    /// <summary>
    /// 資料編輯頁面 設定選項
    /// </summary>
    public class EditPageOption
    {
        public Button Confirm { get; set; }//[確認]按鍵
        public Button Delete { get; set; }//[刪除]按鍵
        public string SuccessMsg { get; set; }//執行成功訊息
        public bool Session { get; set; }//檢查Session
        public bool Opener { get; set; }//檢查Opener
        public bool Parent { get; set; }//檢查上層視窗
        public string CheckOpen { get; set; }//是否檢查直接輸入網址進入
        public bool Guid { get; set; }//
        public string Parameter { get; set; }//檢查必要參數
        public bool IsIntPa { get; set; }//'pa'參數是否為int，預設=是
        public bool CloseWhenSuccess { get; set; }//作業完成後自動關閉
        public bool ReCheckAuth { get; set; }//是否檢查重新登入
        public bool IsWriteExecLog { get; set; }//是否寫入操作紀錄
        public EditPageOption() { SuccessMsg = "更新完成"; Session = false; Opener = false; Parent = false; Guid = false; Parameter = ""; IsIntPa = true; CloseWhenSuccess = true; CheckOpen = "open.aspx"; }
    }

    #endregion

    /// <summary>
    /// 基礎 編輯資料 頁面
    /// </summary>
    public abstract class BasePageEdit : BasePageDB
    {
        protected EditPageOption PageOption;
        protected abstract EditPageOption SetEditOption();

        /// <summary>
        /// 設定更新完成訊息
        /// </summary>
        protected string SuccessMessage { get; set; }

        /// <summary>
        /// 設定紀錄檔資訊
        /// </summary>
        protected string LogMessage { get; set; }

        /// <summary>
        /// 將資料載入頁面
        /// </summary>
        protected abstract void LoadData();
        /// <summary>
        /// 儲存輸入資料
        /// </summary>
        protected abstract void SaveData();
        /// <summary>
        /// 資料載入前檢核
        /// </summary>
        protected virtual void LoadCheck() { }
        /// <summary>
        /// 資料儲存前檢核
        /// </summary>
        protected virtual void SaveCheck() { }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PageOption = SetEditOption();
            if (PageOption == null) PageOption = new EditPageOption();
            if (PageOption.Confirm != null) { PageOption.Confirm.Click += Confirm_Click; }

            if (!IsPostBack && PageOption.CheckOpen.Length > 0 && System.IO.Path.GetFileName(Request.PhysicalPath) != PageOption.CheckOpen)
                oResult.Error("參數錯誤");

            BasePageFunc.CheckEditValid(PageOption, this, oResult);

            if (!oResult.Success) { Session["invalid"] = oResult.Message; Response.Redirect("~/invalid.aspx"); }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (oResult.Success) { this.LoadCheck(); }
                if (oResult.Success) { this.LoadData(); }
                if (!oResult.Success) { Session["invalid"] = oResult.Message; Response.Redirect("~/invalid.aspx"); }
            }
            base.OnLoad(e);
        }

        /// <summary>
        /// [確定]按鍵用
        /// </summary>
        private void Confirm_Click(object sender, EventArgs e)
        {
            //檢查資料正確性
            if (!Page.IsValid) { oResult.Success = false; return; }
            this.SaveCheck();
            //儲存資料
            if (oResult.Success) { this.SaveData(); }

            this.ShowResult((SuccessMessage == null ? PageOption.SuccessMsg : SuccessMessage), true, true);
        }
    }

    /// <summary>
    /// 基礎導向頁面
    /// </summary>
    public class BaseNavi : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["App"]) && Check.IsInteger(Request.QueryString["App"]))
            {
                if (Session["uid"] != null)
                {
                    UserInfo oUser = (UserInfo)Session["uid"];
                    var m = (from lsU in oUser.Roles
                             join lsR in Global.SysRoleProg.List on lsU equals lsR.RoleID
                             join lsP in Global.SysProg.List on lsR.ProgID equals lsP.ID
                             join lsD in Global.SysDir.List on lsP.DirID equals lsD.ID
                             where lsP.ID == Convert.ToInt16(Request["App"]) && lsP.Enabled == true
                             select lsP).FirstOrDefault();
                    if (m != null) { Server.Transfer(string.Format("~/{0}/{1}", m.Folder, m.Path)); }
                }
                else
                {
                    Response.Redirect("~/login.aspx?rtn=" + Server.UrlEncode(Request.RawUrl)); return;
                }
            }
            base.OnInit(e);
        }
    }

    /// <summary>
    /// 基礎 處理常式
    /// </summary>
    public abstract class BaseHandler : System.Web.IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        protected BaseHandlerOption oOption;
        protected abstract void DoHandler(System.Web.HttpContext context);
        protected abstract BaseHandlerOption SetBaseOption();
        protected ExeResult oResult = new ExeResult();
        protected UserInfo oUser;

        /// <summary>
        /// 處理常式 主流程
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(System.Web.HttpContext context)
        {
            //取得 設定項目
            oOption = SetBaseOption();
            if (oOption == null) { oOption = new BaseHandlerOption(); }

            //檢核
            this.CheckValid(context);
            if (oResult.Success)
            {
                //檢核成功才繼續
                context.Response.ContentType = "text/plain";
                if (oOption.NoCache)
                {
                    context.Response.CacheControl = "no-cache";
                    context.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                }
                DoHandler(context);
            }
            context.Response.End();
        }
        public bool IsReusable { get { return false; } }

        /// <summary>
        /// 根據 設定項目 檢核
        /// </summary>
        /// <param name="context"></param>
        private void CheckValid(System.Web.HttpContext context)
        {
            if (oOption.Session)
            {
                if (context.Session["uid"] == null)
                    oResult.Error("未登入");
                else
                    oUser = (UserInfo)context.Session["uid"];

                if (oResult.Success && oOption.Guid)
                {
                    if (string.IsNullOrEmpty(context.Request.QueryString["Guid"]))
                        oResult.Error("參數錯誤");
                    else if (context.Request.QueryString["Guid"] != oUser.Guid)
                        oResult.Error("參數錯誤");
                }
            }
            if (oResult.Success && oOption.Parameter.Length > 0)
            {
                if (string.IsNullOrEmpty(context.Request.QueryString[oOption.Parameter]))
                    oResult.Error("參數錯誤");
            }
        }

        /// <summary>
        /// 將傳入JSON字串 轉換成 指定物件類別
        /// </summary>
        /// <typeparam name="T">物件類別</typeparam>
        /// <param name="obj">物件</param>
        /// <param name="str">JSON字串</param>
        protected T DeserializeObject<T>(string str)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
            }
            catch { oResult.Error("格式錯誤"); }
            return default(T);
        }

        /// <summary>
        /// 將傳入物件 轉換成 JSON字串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns>回傳JSON字串</returns>
        protected string SerializeObject<T>(T obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 處理常式 設定選項
        /// </summary>
        public class BaseHandlerOption
        {
            public bool Session { get; set; }//是否檢查Session
            public bool Guid { get; set; }//是否檢查
            public string Parameter { get; set; }//檢查必要參數
            public bool NoCache { get; set; }//設定Client NoCache
            public BaseHandlerOption() { Session = false; Guid = false; Parameter = ""; NoCache = true; }
        }
    }
}
