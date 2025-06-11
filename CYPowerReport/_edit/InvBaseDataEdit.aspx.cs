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

namespace WebApp._edit
{
    public partial class InvBaseDataEdit : cyc.Page.BasePageEdit
    {
        //InvBaseData oData;
        int iID = 0;
        protected override void OnInit(EventArgs e)
        {
            int.TryParse(Request.QueryString["pa"], out iID);
            base.OnInit(e);
        }
        #region #繼承

        protected override EditPageOption SetEditOption()
        {
            return new EditPageOption() { Confirm = btnConfirm, Session = true, Parent = true, Parameter = "app,pa" };
        }

        protected override void LoadData()
        {
            if (iID != 0)
            {
                var xList = dDB.QueryList<InvBaseData>("SELECT CBD_SEQ_ID,IBD_INV_ID,IBD_INV_NAME,IBD_INV_KW,IBD_INV_KWP FROM InvBaseData WHERE CBD_SEQ_ID=@ID order by IBD_INV_ID", new { ID = iID }).ToList();
                var uList = dDB.QueryList<InvBaseData>("SELECT CBD_SEQ_ID,0 as IBD_INV_ID,INV_NAME as IBD_INV_NAME,null as IBD_INV_KW,null as IBD_INV_KWP from InvUpload where CBD_SEQ_ID=@ID group by CBD_SEQ_ID,INV_NAME order by dbo.GetInt(INV_NAME)", new { ID = iID }).ToList();
                if (xList.Count == 0 && uList.Count > 0)
                    xList.AddRange(uList);

                //xList = xList.GroupBy(p => p.IBD_INV_NAME).Select(q => q.First()).ToList();

                if (oResult.Success)
                {
                    for (int idx = xList.Count; idx < 300; idx++)
                        xList.Add(new InvBaseData());

                    GridView1.DataSource = xList;
                    GridView1.DataBind();

                    GridView2.DataSource = uList;
                    GridView2.DataBind();
                }
            }
        }

        protected override void SaveData()
        {
            var logData = new cyc.ExecLog.LogKeyItem 
            {
                ExecID = Convert.ToInt32(HttpContext.Current.Request.QueryString["app"]),
                UserID = bUser.User.ID,
                KeyValue = iID.ToString()
            };

            List<InvBaseData> invList = new List<InvBaseData>();
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtName = (TextBox)row.FindControl("txtName");
                    if (txtName != null && !string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        TextBox txtKW = (TextBox)row.FindControl("txtKW");
                        TextBox txtKWP = (TextBox)row.FindControl("txtKWP");
                        //TextBox txtSEQ = (TextBox)row.FindControl("txtSEQ");
                        var inv = new InvBaseData()
                        {
                            //IBD_SEQ_ID = Convert.ToInt32(txtSEQ.Text),
                            CBD_SEQ_ID = iID,
                            IBD_INV_NAME = txtName.Text,
                            IBD_INV_ID = row.RowIndex + 1,
                            Update_Time = DateTime.Now,
                            Update_User = bUser.User.ID
                        };
                        if (decimal.TryParse(txtKW.Text, out decimal _dKW)) { inv.IBD_INV_KW = _dKW; }
                        if (decimal.TryParse(txtKWP.Text, out decimal _dKWP)) { inv.IBD_INV_KWP = _dKWP; }

                        invList.Add(inv);
                    }
                }
            }

            //檢查是否有重複名稱
            if (invList.GroupBy(p => p.IBD_INV_NAME).Any(p => p.Count() > 1))
                oResult.Error("[逆變器名稱]不可重複");

            if (oResult.Success)
            {
                var oldList = dDB.QueryList<InvBaseData>("select * from InvBaseData where CBD_SEQ_ID=@ID", new { ID = iID });

                var insList = from n in invList
                              join o in oldList on n.IBD_INV_ID equals o.IBD_INV_ID into OO
                              from o in OO.DefaultIfEmpty()
                              where o == null
                              select n;

                var delList = from o in oldList
                              join n in invList on o.IBD_INV_ID equals n.IBD_INV_ID into NN
                              from n in NN.DefaultIfEmpty()
                              where n == null
                              select o;

                var updList = from o in oldList
                              join n in invList on o.IBD_INV_ID equals n.IBD_INV_ID
                              where o.IBD_INV_NAME != n.IBD_INV_NAME || o.IBD_INV_KW != n.IBD_INV_KW || o.IBD_INV_KWP != n.IBD_INV_KWP
                              select n;

                if (oResult.Success)
                {
                    foreach (var inv in delList)
                    {
                        dDB.Execute(@"delete from InvBaseData where CBD_SEQ_ID=@CBD_SEQ_ID AND IBD_INV_ID=@IBD_INV_ID", inv);
                        if (oResult.Success)
                        {
                            logData.ExecType = "delete";
                            logData.ExecDesc = string.Format("刪除逆變器資料，ID={0} 逆變器名稱={1} KW={2} KWP={3}", inv.CBD_SEQ_ID, inv.IBD_INV_NAME, inv.IBD_INV_KW, inv.IBD_INV_KWP);
                            cyc.ExecLog.WriteKeyLog(logData, dDB);
                        }
                        if (!oResult.Success) break;
                    }
                }

                if (oResult.Success)
                {
                    foreach (var inv in insList)
                    {
                        dDB.Execute(@"
insert into InvBaseData (CBD_SEQ_ID,IBD_INV_ID,IBD_INV_NAME,IBD_INV_KW,IBD_INV_KWP,Update_User,Update_Time) 
values (@CBD_SEQ_ID,@IBD_INV_ID,@IBD_INV_NAME,@IBD_INV_KW,@IBD_INV_KWP,@Update_User,@Update_Time)", inv);
                        if (oResult.Success)
                        {
                            logData.ExecType = "insert";
                            logData.ExecDesc = string.Format("新增逆變器資料，ID={0} 逆變器名稱={1} KW={2} KWP={3}", inv.CBD_SEQ_ID, inv.IBD_INV_NAME, inv.IBD_INV_KW, inv.IBD_INV_KWP);
                            cyc.ExecLog.WriteKeyLog(logData, dDB);
                        }
                        if (!oResult.Success) break;
                    }
                }

                if (oResult.Success)
                {
                    foreach (var inv in updList)
                    {
                        dDB.Execute(@"
update InvBaseData set IBD_INV_NAME=@IBD_INV_NAME,IBD_INV_KW=@IBD_INV_KW,IBD_INV_KWP=@IBD_INV_KWP,
Update_User=@Update_User,Update_Time=@Update_Time where CBD_SEQ_ID=@CBD_SEQ_ID AND IBD_INV_ID=@IBD_INV_ID", inv);
                        if (oResult.Success)
                        {
                            logData.ExecType = "update";
                            logData.ExecDesc = string.Format("修改逆變器資料，ID={0} 逆變器名稱={1} KW={2} KWP={3}", inv.CBD_SEQ_ID, inv.IBD_INV_NAME, inv.IBD_INV_KW, inv.IBD_INV_KWP);
                            cyc.ExecLog.WriteKeyLog(logData, dDB);
                        }
                        if (!oResult.Success) break;
                    }
                }
            }
        }

        #endregion

        #region DATA
        class InvBaseData
        {
            public int IBD_SEQ_ID { get; set; }
            public int CBD_SEQ_ID { get; set; }
            public int IBD_INV_ID { get; set; }
            public string IBD_INV_NAME { get; set; }
            public decimal? IBD_INV_KW { get; set; }
            public decimal? IBD_INV_KWP { get; set; }
            public int Update_User { get; set; }
            public DateTime Update_Time { get; set; }
        }
        #endregion
    }
}