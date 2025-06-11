using cyc.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;

namespace WebApp._edit
{
    public partial class OperationHistoryQ : cyc.Page.BasePageEdit
    {
        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa" };
        }

        protected override void LoadData()
        {
            bindGrid();
        }

        protected override void SaveData()
        {
            //int iID = Convert.ToInt32(Request.QueryString["pa"]);
            //List<cyc.SysRoleProg> insDataP = new List<cyc.SysRoleProg>(), delDataP = new List<cyc.SysRoleProg>();
            //List<cyc.SysRoleProgSub> insDataS = new List<cyc.SysRoleProgSub>(), delDataS = new List<cyc.SysRoleProgSub>();

            //var oldDataP = from ls in cyc.Global.SysRoleProg where ls.RoleID == iID select ls;
            //var oldDataS = from ls in cyc.Global.SysRoleProgSub where ls.RoleID == iID select ls;
            //List<cyc.SysRoleProg> newDataP = new List<cyc.SysRoleProg>();
            //List<cyc.SysRoleProgSub> newDataS = new List<cyc.SysRoleProgSub>();

            //foreach (GridViewRow gRow in GridView1.Rows)
            //{
            //    if (gRow.RowType == DataControlRowType.DataRow)
            //    {
            //        CheckBox chkID = (CheckBox)(gRow.FindControl("chkID"));
            //        HiddenField hidID = (HiddenField)(gRow.FindControl("hidMainID"));
            //        CheckBoxList chkList = (CheckBoxList)(gRow.FindControl("chkSubList"));
            //        int pID = Convert.ToInt32(hidID.Value);

            //        if (chkID.Checked)
            //            newDataP.Add(new cyc.SysRoleProg() { RoleID = iID, ProgID = pID, isAllSub = true });
            //        else if (chkList.SelectedValue.Length > 0)
            //        {
            //            newDataP.Add(new cyc.SysRoleProg() { RoleID = iID, ProgID = pID, isAllSub = false });
            //            foreach (ListItem item in chkList.Items.Cast<ListItem>().Where(li => li.Selected))
            //                newDataS.Add(new cyc.SysRoleProgSub() { RoleID = iID, ProgID = pID, SubID = Convert.ToInt32(item.Value) });
            //        }
            //    }
            //}

            //foreach (var n in newDataP)
            //{
            //    if (oldDataP.FirstOrDefault(p => p.ProgID == n.ProgID && p.isAllSub == n.isAllSub) == null) { insDataP.Add(n); }
            //}
            //foreach (var o in oldDataP)
            //{
            //    if (newDataP.FirstOrDefault(p => p.ProgID == o.ProgID && p.isAllSub == o.isAllSub) == null) { delDataP.Add(o); }
            //}
            //foreach (var n in newDataS)
            //{
            //    if (oldDataS.FirstOrDefault(p => p.ProgID == n.ProgID && p.SubID == n.SubID) == null) { insDataS.Add(n); }
            //}
            //foreach (var o in oldDataS)
            //{
            //    if (newDataS.FirstOrDefault(p => p.ProgID == o.ProgID && p.SubID == o.SubID) == null) { delDataS.Add(o); }
            //}

            //if (insDataP.Count > 0 || insDataS.Count > 0 || delDataP.Count > 0 || delDataS.Count > 0)
            //{
            //    cyc.DB.SqlDBPara oPara = new cyc.DB.SqlDBPara() { Result = oResult };
            //    using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn("", true))
            //    {
            //        try
            //        {
            //            if (delDataP.Count > 0)
            //            {
            //                oPara.Command = "delete from SysRoleProg where RoleID=@RoleID and ProgID=@ProgID and isAllSub=@isAllSub";
            //                oPara.Object = delDataP;
            //                oDB.Execute(oPara);
            //            }
            //            if (oResult.Success && insDataP.Count > 0)
            //            {
            //                oPara.Command = "insert into SysRoleProg (RoleID,ProgID,isAllSub) values (@RoleID,@ProgID,@isAllSub)";
            //                oPara.Object = insDataP;
            //                oDB.Execute(oPara);
            //            }
            //            if (oResult.Success && delDataS.Count > 0)
            //            {
            //                oPara.Command = "delete from SysRoleProgSub where RoleID=@RoleID and ProgID=@ProgID and SubID=@SubID";
            //                oPara.Object = delDataS;
            //                oDB.Execute(oPara);
            //            }
            //            if (oResult.Success && insDataS.Count > 0)
            //            {
            //                oPara.Command = "insert into SysRoleProgSub (RoleID,ProgID,SubID) values (@RoleID,@ProgID,@SubID)";
            //                oPara.Object = insDataS;
            //                oDB.Execute(oPara);
            //            }
            //        }
            //        catch (Exception ex) { cyc.Global.WriteError(ex.Message + ":" + ex.StackTrace, oResult); }
            //        finally { oDB.Result(oResult); }
            //    }
            //    if (oResult.Success) { CYCloud.SysInit.InitSysRoleProg(); }
            //}
        }

        #endregion

        private void bindGrid()
        {
            string strSQL = "";
            DataTable dtTable1 = new DataTable();
            DataTable dtTable2 = new DataTable();
            DataTable dtTable3 = new DataTable();
            DataSet dsData = new DataSet();
            List<SqlParameter> Sqllist = new List<SqlParameter>();

            //strSQL = @"declare @SerialNo varchar(20)='" + Request.QueryString["pa"] + @"';";
            //strSQL += @"select EC_SEQ_ID,CBD_ID
            //                ,EC_Last_Meter_Reading,EC_Current_Meter_Reading
            //                ,EC_Next_Meter_Reading
            //                ,EC_Days,EC_Meter_Record
            //                ,EC_Calculation_Rate,EC_Calculation_Record
            //                ,EC_Calculation_Amt,EC_Daily_Amount
            //                ,EC_Duarantee_Rate,EC_Daily_Billing
            //                ,EC_Check_Amount,EC_Line_Loss
            //                ,isnull(EB.Update_User ,'') as Update_User,isnull(EB.Update_Time,1900/1/1) as Update_Time
            //                from ElectricityBilling as EB
            //                where EC_SEQ_ID=@SerialNo;
            //                ";

            //strSQL += @"select EC_SEQ_ID,CBD_ID
            //                ,EC_Last_Meter_Reading,EC_Current_Meter_Reading
            //                ,EC_Next_Meter_Reading
            //                ,EC_Days,EC_Meter_Record
            //                ,EC_Calculation_Rate,EC_Calculation_Record
            //                ,EC_Calculation_Amt,EC_Daily_Amount
            //                ,EC_Duarantee_Rate,EC_Daily_Billing
            //                ,EC_Check_Amount,EC_Line_Loss
            //                ,isnull(EB.Update_User, '') as Update_User,isnull(EB.Update_Time, 1900 / 1 / 1) as Update_Time
            //                from ElectricityBilling_Log as EB
            //                where EC_SEQ_ID = @SerialNo
            //                order by Update_Time desc;";

            //strSQL += "exec dbo.sp_getDescription @Tab_Name;";

            strSQL += "exec sp_getOperationHistoryQ @SerialNo,@Tab_Name;";

            bPara.Command = strSQL;

            bPara.Parameter.Clear();
            bPara.Parameter.Add(new SqlParameter("SerialNo", Request.QueryString["pa"]));
            bPara.Parameter.Add(new SqlParameter("Tab_Name", Request.QueryString["table"]));

            dsData = dDB.QueryDataSet(bPara);
            if (dsData.Tables.Count==3)
            {
                dtTable1 = dsData.Tables[0];
                dtTable2 = dsData.Tables[1];
                dtTable3 = dsData.Tables[2];
                MakeDescription(dtTable3);

                this.GridView1.DataSource = dtTable1;
                this.GridView1.DataBind();

                this.GridView2.DataSource = dtTable2;
                this.GridView2.DataBind();
            }
        }

        protected void MakeDescription(DataTable dtTable)
        {
            try
            {
                //string strSQL = "";
                //strSQL = @"
                //    SELECT
                //            A.TABLE_NAME                as '表格名稱',
                //            B.COLUMN_NAME               as 'ColEName',
                //            B.DATA_TYPE                 as '型態',
                //            Isnull(Case when B.CHARACTER_MAXIMUM_LENGTH='-1' Then 'Max' Else Cast(B.CHARACTER_MAXIMUM_LENGTH as varchar(10)) End,'') as '長度',
                //            Isnull(( SELECT value FROM fn_listextendedproperty (NULL, 'schema', 'dbo', 'table',a .TABLE_NAME, 'column', default) 
                //                    WHERE name ='MS_Description' and objtype= 'COLUMN' and objname Collate Chinese_Taiwan_Stroke_CI_AS=b .COLUMN_NAME
                //            ),'') as 'ColCName'
                //                ,Case When Isnull(CONSTRAINT_NAME,'') != '' Then 'PK' Else '' End As '鍵'
                //        FROM
                //            INFORMATION_SCHEMA.TABLES A
                //            LEFT JOIN INFORMATION_SCHEMA.COLUMNS B ON (A.TABLE_NAME=B.TABLE_NAME)
                //                Left outer join INFORMATION_SCHEMA.KEY_COLUMN_USAGE C on OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 And C.Table_Name =A.TABLE_NAME And C.COLUMN_NAME=B.COLUMN_NAME
                //        WHERE TABLE_TYPE ='BASE TABLE' And A.TABLE_NAME=@Tab_Name
                //        ORDER BY A.TABLE_NAME,B.ordinal_position
                //";
                //strSQL = "exec dbo.sp_getDescription @Tab_Name";
                //bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@Tab_Name", ddlTable.SelectedValue) };
                ////bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@strTable", "ElectricityBilling") };
                //bPara.Command = strSQL;
                //bPara.Result = oResult;
                int i = 0;
                using (dtTable)
                {
                    BoundField bfColumn;
                    if (dtTable.Rows.Count > 0)
                    {
                        if (GridView1.Columns.Count > 1)
                        {
                            for (i = GridView1.Columns.Count - 1; i > 0; i--)
                            {
                                GridView1.Columns.RemoveAt(i);
                            }
                        }
                    }

                    if (oResult.Success)
                    {
                        for (i = 0; i < dtTable.Rows.Count - 1; i++)
                        {
                            bfColumn = new BoundField();
                            bfColumn.DataField = dtTable.Rows[i]["ColEName"].ToString();
                            bfColumn.HeaderText = dtTable.Rows[i]["ColCName"].ToString();
                            GridView1.Columns.Add(bfColumn);
                            GridView2.Columns.Add(bfColumn);
                            //bfColumn = (BoundField)GridView1.Columns[i + 1];
                            //bfColumn.DataField = oDT.Rows[i]["ColEName"].ToString();
                            //bfColumn.HeaderText = oDT.Rows[i]["ColCName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErr(ex.Message + ":" + ex.StackTrace);
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

        //protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        CheckBox chkID = (CheckBox)(e.Row.FindControl("chkID"));
        //        HiddenField hidID = (HiddenField)(e.Row.FindControl("hidMainID"));
        //        CheckBoxList chkList = (CheckBoxList)(e.Row.FindControl("chkSubList"));
        //        if (chkID != null && hidID != null && chkList != null)
        //        {
        //            var isAll = (from ls in cyc.Global.SysRoleProg
        //                         where ls.RoleID == Convert.ToInt16(Request.QueryString["pa"]) && ls.ProgID == Convert.ToInt16(hidID.Value)
        //                         select ls).FirstOrDefault();
        //            if (isAll != null) { chkID.Checked = isAll.isAllSub; }

        //            var sub = from ls in cyc.Global.SysProgSub
        //                      where ls.UpperID == Convert.ToInt32(hidID.Value) && ls.isShow == true
        //                      select ls;
        //            chkList.DataSource = sub;
        //            chkList.DataBind();

        //            var subList = from ls in cyc.Global.SysRoleProgSub where ls.RoleID == Convert.ToInt16(Request.QueryString["pa"]) && ls.ProgID == Convert.ToInt16(hidID.Value) select ls;
        //            foreach (ListItem item in chkList.Items)
        //            {
        //                var q = (from ls in subList where ls.SubID == Convert.ToInt16(item.Value) select ls).FirstOrDefault();
        //                if (q != null) { item.Selected = true; }
        //            }
        //        }
        //    }
        //}
    }
}