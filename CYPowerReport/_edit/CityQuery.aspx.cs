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


    public partial class CityQuery : cyc.Page.BasePageDB
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
                //switch (strKind)
                //{
                //    case "1":
                //        bPara.Command = "select C_City_ID,C_City_Name from City where C_Upper_ID='00' order by C_City_ID";
                //        bPara.Result = oResult;
                //        using (DataTable oDT = bDB.QueryDT(bPara))
                //        {
                //            if (oResult.Success)
                //            {
                //                this.ddlCode1.DataSource = oDT;
                //                ddlCode1.DataTextField = "C_City_Name";
                //                ddlCode1.DataValueField = "C_City_ID";
                //                this.ddlCode1.DataBind();

                //                ddlCode1.SelectedValue = "07";
                //            }
                //        }
                //        getRegionData();
                //        break;

                //}

                getCityData();
            }
            catch
            {

            }
        }

        protected void getCityData()
        {
            try
            {
                //bPara.Command = "select C_City_ID as CodeID,C_City_Name as CodeName from City where C_Upper_ID='00' order by C_City_ID";
                ////bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@C_Upper_ID", ddlCode1.SelectedValue) };
                //bPara.Result = oResult;
                using (DataTable oDT = dDB.QueryDataTable("select C_City_ID as CodeID,C_City_Name as CodeName from City where C_Upper_ID='00' order by C_City_ID"))
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

        //protected void getRegionData()
        //{
        //    try
        //    {
        //        bPara.Command = "select C_City_ID as CodeID,C_City_Name as CodeName from City where C_Upper_ID=@C_Upper_ID";
        //        bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@C_Upper_ID", ddlCode1.SelectedValue) };
        //        bPara.Result = oResult;
        //        using (DataTable oDT = bDB.QueryDT(bPara))
        //        {
        //            if (oResult.Success)
        //            {
        //                this.ListBox1.DataSource = oDT;
        //                ListBox1.DataTextField = "CodeName";
        //                ListBox1.DataValueField = "CodeID";
        //                this.ListBox1.DataBind();
        //            }
        //        }

        //    }
        //    catch
        //    {

        //    }

        //}

        #endregion

        #region 控制項事件

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            ListItem objLi;
            objLi = ListBox1.SelectedItem;

            ListBox2.Items.Add(objLi);
            ListBox1.Items.RemoveAt(ListBox1.SelectedIndex);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            ListItem objLi;
            objLi = ListBox2.SelectedItem;

            ListBox1.Items.Add(objLi);
            ListBox2.Items.RemoveAt(ListBox2.SelectedIndex);

        }


        //protected void ddlCode1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    getRegionData();
        //}

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
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Reload", "window.parent.ReloadCityData('" + strValue + "');parent.jQuery.fancybox.close();", true);
                    break;

            }

        }
        #endregion
    }
}