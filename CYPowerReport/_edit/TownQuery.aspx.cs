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


    public partial class TownQuery : cyc.Page.BasePageDB
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
                getCityData();
                getRegionData();
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
                        this.ddlCity.DataSource = oDT;
                        ddlCity.DataTextField = "CodeName";
                        ddlCity.DataValueField = "CodeID";
                        this.ddlCity.DataBind();
                    }
                }

            }
            catch
            {

            }

        }

        protected void getRegionData()
        {
            try
            {
                bPara.Command = "select C_City_ID as CodeID,C_City_Name as CodeName from City where C_Upper_ID=@C_Upper_ID";
                bPara.Parameter = new List<SqlParameter>() { new SqlParameter("@C_Upper_ID", ddlCity.SelectedValue) };
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
            //ListItem objLi;
            //objLi = ListBox1.SelectedItem;
            //if (objLi != null)
            //{
            //    ListBox2.Items.Add(objLi);
            //    ListBox1.Items.RemoveAt(ListBox1.SelectedIndex);
            //}
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

            //string strInfo = "alert('已加入角色');";
            switch (ViewState["strKind"].ToString())
            {
                case "1":
                case "1_1":
                    //strInfo += "";
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "Reload", "window.parent.ReloadTownData('" + strValue + "');parent.jQuery.fancybox.close();", true);
                    break;

            }
        }

        protected void ddlCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            getRegionData();
        }
        #endregion

    }
}