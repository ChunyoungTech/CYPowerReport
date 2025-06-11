using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._edit
{

    public partial class CaseBaseDataQuery : cyc.Page.BasePageDB
    {
        #region Page事件
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!cyc.Comm.Login.CheckSession()) { Session["invalid"] = "未登入或逾時登出"; Response.Redirect("~/invalid.aspx"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["pa"]))
                {
                    string strKind = Request.QueryString["pa"];
                    ViewState["strKind"] = strKind;
                    getData(strKind);
                }

            }
        }

        #endregion

        #region 初始值
        protected void getData(string strKind)
        {
            try
            {
                switch (strKind)
                {
                    case "1":
                        //bPara.Command = "select distinct CD_TYPE as CD_TYPE from CutomerData order by CD_TYPE";
                        //bPara.Result = oResult;
                        using (DataTable oDT = dDB.QueryDataTable("select distinct CD_TYPE as CD_TYPE from CutomerData order by CD_TYPE"))
                        {
                            if (oResult.Success)
                            {
                                this.ddlCode1.DataSource = oDT;
                                ddlCode1.DataTextField = "CD_TYPE";
                                ddlCode1.DataValueField = "CD_TYPE";
                                this.ddlCode1.DataBind();
                            }
                        }
                        getCaseBaseData();
                        break;

                }


            }
            catch
            {

            }
        }

        protected void getCaseBaseData()
        {
            try
            {
                string strSQL = @"select CBD_SEQ_ID as CodeID,CBD_Case_Name as CodeName 
                    from CaseBaseData as CBD
                    inner join CutomerData as CD on CD.CD_SEQ_ID = CBD.CBD_Case_Owner
                    where CD_TYPE=@CD_TYPE";
                bPara.Command = strSQL;
                bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@CD_TYPE", ddlCode1.SelectedValue) };
                bPara.Result = oResult;
                using (DataTable oDT = dDB.QueryDataTable(bPara))
                {
                    if (oResult.Success)
                    {
                        this.ListBox1.DataSource = oDT;
                        ListBox1.DataTextField = "CodeName";
                        ListBox1.DataValueField = "CodeID";
                        this.ListBox1.DataBind();
                    }
                }

            }
            catch
            {

            }

        }

        #endregion

        #region 控制項事件

        //protected void btnAdd_Click(object sender, EventArgs e)
        //{
        //    //ListItem objLi;
        //    //objLi = ListBox1.SelectedItem;
        //    //if (objLi!=null)
        //    //{
        //    //    ListBox2.Items.Add(objLi);
        //    //    ListBox1.Items.RemoveAt(ListBox1.SelectedIndex);
        //    //}
        //    ListItem objLi;
        //    try
        //    {
        //        //for (int i= 0; i < ListBox1.Items.Count; i++)
        //        //{
        //        //    if (ListBox1.Items[i].Selected)
        //        //    {
        //        //        objLi = ListBox1.Items[i];
        //        //        ListBox2.Items.Add(objLi);
        //        //        //ListBox1.Items.RemoveAt(i);
        //        //    }
        //        //}
        //        for (int i = ListBox1.Items.Count - 1; i >= 0; i--)
        //        {
        //            if (ListBox1.Items[i].Selected)
        //            {
        //                objLi = ListBox1.Items[i];
        //                ListBox2.Items.Add(objLi);
        //                ListBox1.Items.RemoveAt(i);
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string strError;
        //        strError = ex.Message + "\r\n" + ex.StackTrace;
        //    }

        //}

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                if (ListBox1.Items[i].Selected)
                {
                    if (!ListBox2.Items.Cast<ListItem>().Any(p => p.Value == ListBox1.Items[i].Value))
                    {
                        ListBox2.Items.Add(ListBox1.Items[i]);
                    }
                }
            }
        }
        protected void btnAddAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ListBox1.Items.Count; i++)
            {
                if (!ListBox2.Items.Cast<ListItem>().Any(p => p.Value == ListBox1.Items[i].Value))
                {
                    ListBox2.Items.Add(ListBox1.Items[i]);
                }
            }
            //ListItem objLi;
            //for (int i = 0; i < ListBox1.Items.Count; i++)
            //{
            //    objLi = ListBox1.Items[i];
            //    ListBox2.Items.Add(objLi);
            //}
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            for (int i = ListBox2.Items.Count - 1; i >= 0; i--)
            {
                if (ListBox2.Items[i].Selected)
                {
                    ListBox2.Items.RemoveAt(i);
                }
            }
            //ListItem objLi;
            //objLi = ListBox2.SelectedItem;
            //if (objLi != null)
            //{
            //    ListBox1.Items.Add(objLi);
            //    ListBox2.Items.RemoveAt(ListBox2.SelectedIndex);
            //}
        }

        protected void btnRemoveAll_Click(object sender, EventArgs e)
        {
            ListBox2.Items.Clear();
        }

        protected void ddlCode1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getCaseBaseData();
        }

        protected void btnJoinSelect_Click(object sender, EventArgs e)
        {
            string strValue = "";
            if (ListBox2.Items.Count>0)
            {
                for (int i=0;i< ListBox2.Items.Count;i++)
                {
                    strValue += ListBox2.Items[i].Value + "*";

                }
                strValue = strValue.Substring(0, strValue.Length - 1);
            }

            string strInfo = "alert('已加入角色');";
            switch (ViewState["strKind"].ToString())
            {
                case "1":
                    strInfo += "";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Reload", "window.parent.ReloadCaseBaseDataData('" + strValue + "');parent.jQuery.fancybox.close();", true);
                    break;

            }

        }

        //private void txtName_Enter(object sender, System.EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(txtName.Text))
        //    {
        //        btnQuery_Click(sender, e);
        //    }
        //}

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strSQL = @"select CBD_SEQ_ID as CodeID,CBD_Case_Name as CodeName 
                from CaseBaseData as CBD
                inner join CutomerData as CD on CD.CD_SEQ_ID = CBD.CBD_Case_Owner
                where CD_TYPE=@CD_TYPE";
            if (txtName.Text != "")
            {
                strSQL += " and CBD_Case_Name like '%" + txtName.Text.Trim() + "%'";
            }
            bPara.Command = strSQL;
            bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@CD_TYPE", ddlCode1.SelectedValue) };
            bPara.Result = oResult;
            using (DataTable oDT = dDB.QueryDataTable(bPara))
            {
                if (oResult.Success)
                {
                    this.ListBox1.DataSource = oDT;
                    ListBox1.DataTextField = "CodeName";
                    ListBox1.DataValueField = "CodeID";
                    this.ListBox1.DataBind();
                }
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
        }
        #endregion
    }
}