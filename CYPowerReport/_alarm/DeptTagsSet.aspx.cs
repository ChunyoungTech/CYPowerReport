using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;

namespace WebApp._alarm
{
    public partial class DeptTagsSet : BasePageGridMulti
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                ddlDeptQ.DeptID = bUser.Dept.ID;
        }

        protected override DataTable QuerySourceData(int idx)
        {
            bPara.Command = @"select * from DeptTagAlarmStatus A 
                              inner join TagData B on A.tag_data_id=B.ID
                              left join MappSetting M on M.MS_SEQ_ID=A.MAppGroupId";
            //bPara.Command += ddlDeptQ.GetQuerySQL("A.dept_id", "where");
            bPara.Command += " where A.dept_id=@Dept";
            if (ddlTypeQ.SelectedValue.Length > 0) { bPara.Command += " and B.Tag_Type=@Type"; }
            if (txtNameQ.Text.Trim().Length > 0) { bPara.Command += " and B.Tag_Name like '%'+@Name+'%'"; }
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Type", ddlTypeQ.SelectedValue));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Name", txtNameQ.Text.Trim()));
            bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("Dept", ddlDeptQ.DeptID));
            //return bDB.QueryDT(bPara);
            return dDB.QueryDataTable(bPara);
        }

        protected override GridPageSetting SetPageSetting()
        {
            return new GridPageSetting() { Option = new GridOption[] { new GridOption { AutoBind = true, Grid = GridView1, Pager = ucPager, Query = btnQuery, Refresh = lbRefresh, Excel = btnExport } } };
        }

        protected override ExportOption GetExportOption(int idx)
        {
            return new ExportOption()
            {
                Mapping = new string[] { "資料點名稱" },
                Column = new string[] { "Tag_Name" },
                ColType = new string[] { "s" },
                FileName = "部門與資料點對應"
            };
        }

        [WebMethod(EnableSession = true)]
        public static bool DeleteItem(string id, int App)
        {
            cyc.ExeResult oResult = new cyc.ExeResult();
            if (HttpContext.Current.Session["uid"] == null)
                oResult.Error("未登入");
            else
            {
                var oUser = (cyc.UserInfo)HttpContext.Current.Session["uid"];
                using (var dDB = new cyc.DB.SqlDapperConn(oResult))
                {
                    dDB.Execute("delete DeptTagAlarmStatus where id=@id", new { id });
                    if (oResult.Success)
                        cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = App, ExecType = "update", ExecDesc = string.Format("刪除部門警報參數點", id), UserID = oUser.User.ID }, dDB);
                }
            }
            return oResult.Success;

            //try
            //{
            //    using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn(cyc.DB.ConnString.Main))
            //    {
            //        cyc.DB.SqlDBPara bPara = new cyc.DB.SqlDBPara
            //        {
            //            Command = @"delete DeptTagAlarmStatus where id=@id"
            //        };
            //        bPara.Parameter.Clear();
            //        bPara.Parameter.Add(new System.Data.SqlClient.SqlParameter("id", id));
            //        oDB.QueryDT(bPara);
            //        var oUser = (cyc.UserInfo)HttpContext.Current.Session["uid"];
            //        cyc.ExecLog.WriteKeyLog(new cyc.ExecLog.LogKeyItem() { ExecID = App, ExecType = "update", ExecDesc = string.Format("刪除部門警報參數點", id), UserID = oUser.User.ID }, oDB);
            //    }
            //}
            //catch (Exception e)
            //{
            //    cyc.Global.WriteError($"刪除部門警報參數點失敗： {e.StackTrace}");
            //    return false;
            //}
            //return true;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //	1全部禁能	2HIHI禁能	3HI禁能	4LO禁能	5LOLO禁能	6HIHI設定值	7HI設定值	8LO設定值	9LOLO設定值
            DataRowView drv = (DataRowView)e.Row.DataItem;

            if ((e.Row.RowType == DataControlRowType.DataRow) || (e.Row.RowType == DataControlRowType.Footer))
            {
                if (drv != null)
                {
                    //change text
                    e.Row.Cells[6].Text = drv["ALL_Enable"].ToString()  == "True" ? "是" : "否";
                    e.Row.Cells[7].Text = drv["HIHI_Enable"].ToString() == "True" ? "是" : "否";
                    e.Row.Cells[8].Text = drv["HI_Enable"].ToString()   == "True" ? "是" : "否";
                    e.Row.Cells[9].Text = drv["LO_Enable"].ToString()   == "True" ? "是" : "否";
                    e.Row.Cells[10].Text = drv["LOLO_Enable"].ToString() == "True" ? "是" : "否";
                }
            }
        }
    }
}