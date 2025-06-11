using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using cyc.Page;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System.Globalization;

namespace WebApp._edit
{
    public partial class CaseInvUploadEdit : cyc.Page.BasePageEdit
    {
        //CaseUpload CData;
        //InvUpload IData;
        //Remark RData;

        protected int iID = 0;
        DateTime dDate;
        //protected override void OnInit(EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {

        //    }
        //    base.OnInit(e);
        //}

        protected override void OnLoad(EventArgs e)
        {
            string[] sPa = Request.QueryString["pa"].Split('_');
            if (sPa.Length == 2)
            {
                if (!DateTime.TryParse(sPa[0], out dDate) || !int.TryParse(sPa[1], out iID))
                    oResult.Error("參數錯誤");
            }
            else
                oResult.Error("參數錯誤");

            //if (!DateTime.TryParse(Request.QueryString["da"], out dDate) || !int.TryParse(Request.QueryString["pa"], out iID))
            //    oResult.Error("參數錯誤");
            base.OnLoad(e);
        }

        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            //return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "app,pa" };
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "pa", IsIntPa = false };
        }

        protected override void LoadData()
        {
            //this.hidID.Value = Request.QueryString["pa"].ToString();
            //this.hidDate.Value = Request.QueryString["da"].ToString();
            if (iID != 0)
            {
                var xList = dDB.QueryList<EditData>(@"
select CBD_SEQ_ID,DATA_DATE,UPLOAD_AMT,UPLOAD_RATE,UPLOAD_PR,REMARK,REMARK_User,REMARK_Time,INV_NAME,INV_AMT,INV_RATE,INV_PR
from vCaseInvUploadEdit where CBD_SEQ_ID=@ID AND DATA_DATE=@Date order by dbo.GetInt(INV_NAME)", new { ID = iID, Date = dDate }).ToList();

                if (oResult.Success)
                {
                    if (xList.Count > 0)
                    {
                        //每日案場發電量
                        txtAMT.Text = xList[0].UPLOAD_AMT.ToString();
                        txtRATE.Text = xList[0].UPLOAD_RATE.ToString();
                        txtPR.Text = xList[0].UPLOAD_PR.ToString();
                        //異動
                        txtREMARK.Text = xList[0].REMARK;
                        REMARK_User.Text = xList[0].REMARK_User;
                        if (xList[0].REMARK_Time != null) REMARK_Time.Text = ((DateTime)xList[0].REMARK_Time).ToString();

                        //for (int idx = xList.Count; idx < 50; idx++)
                        //    xList.Add(new EditData());

                        GridView1.DataSource = xList;
                        GridView1.DataBind();
                    }
                    else
                        oResult.Error("查無資料");
                }

                //bPara.Command = "SELECT * FROM vCaseInvUploadEdit WHERE CBD_SEQ_ID=@ID AND DATA_DATE=@Date order by dbo.GetInt(INV_NAME)";
                //bPara.Parameter.Add(new SqlParameter("ID", iID));
                //bPara.Parameter.Add(new SqlParameter("Date", this.hidDate.Value));
                //DataSet dsData = bDB.QueryDS(bPara);
                //if (dsData == null)
                //    oResult.Error("查無資料");
                //else
                //{
                //    int i = 1;

                //    foreach (DataRow od in dsData.Tables[0].Rows)
                //    {
                //        if (i == 1)
                //        {
                //            //每日案場發電量
                //            txtAMT.Text = od["UPLOAD_AMT"].ToString();
                //            txtRATE.Text = od["UPLOAD_RATE"].ToString();
                //            txtPR.Text = od["UPLOAD_PR"].ToString();
                //            //異動
                //            txtREMARK.Text = ((od["REMARK"] == null) ? "0" : od["REMARK"].ToString());
                //            REMARK_User.Text = ((od["REMARK_User"] == null) ? "0" : od["REMARK_User"].ToString());
                //            REMARK_Time.Text = ((od["REMARK_Time"] == null) ? "0" : od["REMARK_Time"].ToString());
                //        }

                //        //每日逆變器發電量
                //        ((TextBox)INV_Table.FindControl("txtNAME_" + i.ToString())).Text = od["INV_NAME"].ToString();
                //        ((TextBox)INV_Table.FindControl("txtAMT_" + i.ToString())).Text = ((od["INV_AMT"] == null) ? "0" : od["INV_AMT"].ToString());
                //        ((TextBox)INV_Table.FindControl("txtRATE_" + i.ToString())).Text = ((od["INV_RATE"] == null) ? "0" : od["INV_RATE"].ToString());
                //        ((TextBox)INV_Table.FindControl("txtPR_" + i.ToString())).Text = ((od["INV_PR"] == null) ? "0" : od["INV_PR"].ToString());

                //        i++;

                //    }
                //}
            }
        }

        protected override void SaveCheck()
        {

        }



        protected override void SaveData()
        {
            //DateTime theDay=DateTime.Parse(this.hidDate.Value);

            //案場每日上傳資料更新
            var CData = new CaseUpload
            {
                CBD_SEQ_ID = iID,
                DATA_DATE = dDate,
                CU_Year = dDate.Year.ToString(),
                CU_Month = dDate.Month.ToString().PadLeft(2, '0'),
                CU_Week = (GetWeekOfYear(dDate)).ToString().PadLeft(2, '0')
            };
            if (decimal.TryParse(txtAMT.Text, out decimal dAMT)) CData.UPLOAD_AMT = dAMT;
            if (decimal.TryParse(txtRATE.Text, out decimal dRATE)) CData.UPLOAD_RATE = dRATE;
            if (decimal.TryParse(txtPR.Text, out decimal dPR)) CData.UPLOAD_PR = dPR;
            //using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
            //{
            //    bPara.Command = "DELETE CaseUpload WHERE CBD_SEQ_ID=@CBD_SEQ_ID AND DATA_DATE=@Date;" +
            //        "INSERT INTO CaseUpload (CBD_SEQ_ID,UPLOAD_AMT,UPLOAD_RATE,UPLOAD_PR,DATA_DATE,CU_Year,CU_Month,CU_Week) " +
            //        "VALUES (@CBD_SEQ_ID,@Case_AMT,@Case_RATE,@Case_PR,@Date,@CU_Year,@CU_Month,@CU_Week);" +
            //        "SELECT CAST(SCOPE_IDENTITY() as int)";

            //    try
            //    {
            //        CData.CBD_SEQ_ID = bDB.oConn.Query<int>(bPara.Command, CData).Single();
            //    }
            //    catch (Exception ex) { cyc.Global.WriteError(ex.Message + ":" + ex.StackTrace, oResult); }
            //}

            //異常處置備註更新
            var RData = new Remark
            {
                CBD_SEQ_ID = iID,
                DATA_DATE = dDate,
                REMARK = txtREMARK.Text.Trim(),
                REMARK_User = bUser.User.Name,
                REMARK_Time = DateTime.Now
            };
            //using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
            //{
            //    bPara.Command = "DELETE REMARK WHERE CBD_SEQ_ID=@CBD_SEQ_ID AND DATA_DATE=@Date;" +
            //        "INSERT INTO REMARK (CBD_SEQ_ID,DATA_DATE,REMARK,REMARK_User,REMARK_Time) " +
            //        "VALUES (@CBD_SEQ_ID,@Date,@REMARK,@REMARK_User,@REMARK_Time);" +
            //        "SELECT CAST(SCOPE_IDENTITY() as int)";

            //    try
            //    {
            //        RData.CBD_SEQ_ID = bDB.oConn.Query<int>(bPara.Command, RData).Single();
            //    }
            //    catch (Exception ex) { cyc.Global.WriteError(ex.Message + ":" + ex.StackTrace, oResult); }
            //}

            //逆變器每日上傳資料更新
            //for (int i = 1; i <= 25 ;i++)
            //{
            //    if (((TextBox)INV_Table.FindControl("txtNAME_" + i.ToString())).Text == "")
            //    {
            //        continue;
            //    }

            //    IData = new InvUpload
            //    {
            //        CBD_SEQ_ID = int.Parse(this.hidID.Value),
            //        DATA_DATE = this.hidDate.Value,
            //        INV_NAME = ((TextBox)INV_Table.FindControl("txtNAME_" + i.ToString())).Text.Trim(),
            //        INV_AMT = decimal.Parse(((TextBox)INV_Table.FindControl("txtAMT_" + i.ToString())).Text.Trim()),
            //        INV_RATE = decimal.Parse(((TextBox)INV_Table.FindControl("txtRATE_" + i.ToString())).Text.Trim()),
            //        INV_PR = decimal.Parse(((TextBox)INV_Table.FindControl("txtPR_" + i.ToString())).Text.Trim()),
            //        IU_Year = (theDay.Year).ToString(),
            //        IU_Month = (theDay.Month).ToString().PadLeft(2, '0'),
            //        IU_Week = (GetWeekOfYear(theDay)).ToString().PadLeft(2, '0')
            //    };
            //    using (cyc.DB.SqlDBConn oDB = new cyc.DB.SqlDBConn())
            //    {
            //        bPara.Command = "DELETE InvUpload WHERE CBD_SEQ_ID=@CBD_SEQ_ID AND DATA_DATE=@Date AND INV_NAME=@INV_NAME;" +
            //            "INSERT INTO InvUpload (CBD_SEQ_ID,INV_NAME,INV_AMT,INV_RATE,INV_PR,DATA_DATE,IU_Year,IU_Month,IU_Week) " +
            //            "VALUES (@CBD_SEQ_ID,@INV_NAME,@Inv_AMT,@Inv_RATE,@Inv_PR,@Date,@IU_Year,@IU_Month,@IU_Week);" +
            //            "SELECT CAST(SCOPE_IDENTITY() as int)";

            //        try
            //        {
            //            IData.CBD_SEQ_ID = bDB.oConn.Query<int>(bPara.Command, IData).Single();
            //        }
            //        catch (Exception ex) { cyc.Global.WriteError(ex.Message + ":" + ex.StackTrace, oResult); }
            //    }
            //}
            List<InvUpload> invList = new List<InvUpload>();
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txt_INV_NAME = (TextBox)row.FindControl("txt_INV_NAME");
                    if (txt_INV_NAME != null && !string.IsNullOrWhiteSpace(txt_INV_NAME.Text))
                    {
                        TextBox txt_INV_AMT = (TextBox)row.FindControl("txt_INV_AMT");
                        TextBox txt_INV_RATE = (TextBox)row.FindControl("txt_INV_RATE");
                        TextBox txt_INV_PR = (TextBox)row.FindControl("txt_INV_PR");
                        if (txt_INV_AMT != null && txt_INV_RATE != null && txt_INV_PR != null)
                        {
                            var inv = new InvUpload
                            {
                                CBD_SEQ_ID = iID,
                                DATA_DATE = dDate,
                                INV_NAME = txt_INV_NAME.Text,
                                IU_Year = dDate.Year.ToString(),
                                IU_Month = dDate.Month.ToString().PadLeft(2, '0'),
                                IU_Week = GetWeekOfYear(dDate).ToString().PadLeft(2, '0')
                            };
                            if (decimal.TryParse(txt_INV_AMT.Text, out decimal _dAMT)) inv.INV_AMT = _dAMT;
                            if (decimal.TryParse(txt_INV_RATE.Text, out decimal _dRATE)) inv.INV_RATE = _dRATE;
                            if (decimal.TryParse(txt_INV_PR.Text, out decimal _dPR)) inv.INV_PR = _dPR;

                            invList.Add(inv);
                        }
                    }
                }
            }

            //檢查是否有重複名稱
            if (invList.GroupBy(p => p.INV_NAME).Any(p => p.Count() > 1))
                oResult.Error("[逆變器名稱]不可重複");

            if (oResult.Success)
            {
                using (cyc.DB.SqlDapperConn oDB = new cyc.DB.SqlDapperConn(oResult, "", true))
                {
                    //案場每日上傳資料更新
                    if (oResult.Success)
                        oDB.Execute("DELETE CaseUpload WHERE CBD_SEQ_ID=@CBD_SEQ_ID AND DATA_DATE=@DATA_DATE", CData);
                    if (oResult.Success)
                        oDB.Execute("INSERT INTO CaseUpload (CBD_SEQ_ID,UPLOAD_AMT,UPLOAD_RATE,UPLOAD_PR,DATA_DATE,CU_Year,CU_Month,CU_Week) VALUES (@CBD_SEQ_ID,@UPLOAD_AMT,@UPLOAD_RATE,@UPLOAD_PR,@DATA_DATE,@CU_Year,@CU_Month,@CU_Week)", CData);
                    //異常處置備註更新
                    if (oResult.Success)
                        oDB.Execute("DELETE REMARK WHERE CBD_SEQ_ID=@CBD_SEQ_ID AND DATA_DATE=@DATA_DATE", RData);
                    if (oResult.Success)
                        oDB.Execute("INSERT INTO REMARK(CBD_SEQ_ID,DATA_DATE,REMARK,REMARK_User,REMARK_Time) VALUES (@CBD_SEQ_ID,@DATA_DATE,@REMARK,@REMARK_User,@REMARK_Time)", RData);
                    //逆變器每日上傳資料更新
                    if (oResult.Success && invList.Count > 0)
                    {
                        oDB.Execute("DELETE InvUpload WHERE CBD_SEQ_ID=@CBD_SEQ_ID AND DATA_DATE=@DATA_DATE AND INV_NAME=@INV_NAME", invList);
                        if (oResult.Success)
                            oDB.Execute("INSERT INTO InvUpload (CBD_SEQ_ID,INV_NAME,INV_AMT,INV_RATE,INV_PR,DATA_DATE,IU_Year,IU_Month,IU_Week) VALUES (@CBD_SEQ_ID,@INV_NAME,@INV_AMT,@INV_RATE,@INV_PR,@DATA_DATE,@IU_Year,@IU_Month,@IU_Week)", invList);
                    }
                    oDB.ResultTransaction();
                }
            }
        }

        //取日期週數
        private int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        #endregion

        #region DATA

        class EditData
        {
            public int CBD_SEQ_ID { get; set; }
            public DateTime DATA_DATE { get; set; }
            public decimal UPLOAD_AMT { get; set; }
            public decimal UPLOAD_RATE { get; set; }
            public decimal UPLOAD_PR { get; set; }
            public string REMARK { get; set; }
            public string REMARK_User { get; set; }
            public DateTime? REMARK_Time { get; set; }
            public string INV_NAME { get; set; }
            public decimal? INV_AMT { get; set; }
            public decimal? INV_RATE { get; set; }
            public decimal? INV_PR { get; set; }
        }

        public class CaseUpload
        {
            public int CBD_SEQ_ID { get; set; }
            public DateTime DATA_DATE { get; set; }
            public decimal? UPLOAD_AMT { get; set; }
            public decimal? UPLOAD_RATE { get; set; }
            public decimal? UPLOAD_PR { get; set; }
            public string CU_Year { get; set; }
            public string CU_Month { get; set; }
            public string CU_Week { get; set; }
        }
        public class InvUpload
        {
            public int CBD_SEQ_ID { get; set; }
            public DateTime DATA_DATE { get; set; }
            public string INV_NAME { get; set; }
            public decimal? INV_AMT { get; set; }
            public decimal? INV_RATE { get; set; }
            public decimal? INV_PR { get; set; }
            public string IU_Year { get; set; }
            public string IU_Month { get; set; }
            public string IU_Week { get; set; }
        }
        public class Remark
        {
            public int CBD_SEQ_ID { get; set; }
            public DateTime DATA_DATE { get; set; }
            public string REMARK { get; set; }
            public string REMARK_User { get; set; }
            public DateTime REMARK_Time { get; set; }
        }
        #endregion
    }


}