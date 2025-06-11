using System;
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

namespace cyc.UC
{
    public abstract class ucPager : System.Web.UI.UserControl
    {
        public delegate void PageChangedEventHandler(object sender, PagerChangeArgs e);
        public event PageChangedEventHandler PageChanged;

        protected abstract DropDownList ddlPageSize { get; }
        protected abstract TextBox txtToGo { get; }
        protected abstract Label lblTotalPage { get; }
        protected abstract Label lblTotalCnt { get; }
        protected abstract Panel pnlVisible { get; }

        public string TargetID { get; set; }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Cookies["pagesize"] != null && ddlPageSize.Items.FindByValue(Request.Cookies["pagesize"].Value) != null)
                {
                    ddlPageSize.SelectedValue = Request.Cookies["pagesize"].Value;
                }
                this.Refresh();
            }
        }

        public void Refresh()
        {
            GridView oGrid = (GridView)this.Parent.FindControl(TargetID);
            if (oGrid != null)
            {
                oGrid.PageSize = Convert.ToInt32(this.ddlPageSize.SelectedValue);
                this.txtToGo.Text = (oGrid.PageCount > 0 ? oGrid.PageIndex + 1 : 0).ToString();
                this.lblTotalPage.Text = oGrid.PageCount.ToString();
                this.pnlVisible.Visible = (oGrid.PageCount != 0);
            }
            else
                this.pnlVisible.Visible = false;
        }
        //顯示全部筆數
        public void showTotalCnt(int icnt)
        {
            this.lblTotalCnt.Text = icnt.ToString();
        }
        //觸發事件
        protected void RaisePagerEvent(char x)
        {
            int iPage = 1, iTotal = Convert.ToInt32(lblTotalPage.Text);
            if (iTotal == 0) { return; }

            int.TryParse(txtToGo.Text, out iPage);
            switch (x)
            {
                case 'F':
                    iPage = 1;
                    break;
                case 'P':
                    iPage -= 1;
                    break;
                case 'N':
                    iPage += 1;
                    break;
                case 'L':
                    iPage = Convert.ToInt32(lblTotalPage.Text);
                    break;
                default:
                    break;
            }

            if (iPage < 1) { iPage = 1; }
            if (iPage > iTotal) { iPage = iTotal; }
            txtToGo.Text = iPage.ToString();

            PagerChangeArgs args = new PagerChangeArgs()
            {
                CurrentPage = Convert.ToInt32(this.txtToGo.Text),
                PageSize = Convert.ToInt32(this.ddlPageSize.SelectedValue),
                TotalPages = Convert.ToInt32(this.lblTotalPage.Text)
            };
            PageChanged(this, args);
            this.Refresh();
        }
        ////檢查參數合理性
        //private void CheckPager()
        //{
        //    if (!pin.Comm.Check.isInteger(this.txtToGo.Text))
        //        this.txtToGo.Text = "1";
        //    else
        //    {
        //        if (Convert.ToInt32(this.txtToGo.Text) - 1 < 0)
        //            this.txtToGo.Text = "1";
        //        else if (Convert.ToInt32(this.txtToGo.Text) > Convert.ToInt32(this.lblTotalPage.Text))
        //            this.txtToGo.Text = this.lblTotalPage.Text;
        //    }
        //}

        //protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //this.txtToGo.Text = "1";
        //    RaisePagerEvent('F');
        //}
    }

    public class PagerChangeArgs : EventArgs
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }

    public class DeptControl
    {
        public static void DeptCreate(DropDownList ddlDept, cyc.UserInfo oUser, bool isShowAll, bool isShowTop)
        {
            ddlDept.Items.Clear();
            if (!isShowAll)
            {
                //if (oUser.UserRole.Any(p => p.LevelNo == 1))
                if ((from lsU in oUser.Roles
                     join lsR in cyc.Global.SysRole.List.Where(p => p.LevelNo == 1) on lsU equals lsR.ID
                     select lsR).Count() > 0)
                {
                    foreach (var dept in cyc.Global.SysDept.List.Where(p => p.UpperID == 0))
                    {
                        ddlDept.Items.Add(new ListItem(dept.Name, dept.ID.ToString()));
                        GetNextDept(ddlDept, dept.ID);
                    }
                }
                else
                {
                    ddlDept.Items.Add(new ListItem(oUser.Dept.Name, oUser.Dept.ID.ToString()));
                    GetNextDept(ddlDept, oUser.Dept.ID);
                }
            }
            else
            {
                if (isShowTop)
                {
                    ddlDept.Items.Add(new ListItem("", "0"));
                }
                else if (cyc.Global.SysDept.List.Count(p => p.UpperID == 0) > 1)
                {
                    ddlDept.Items.Add(new ListItem("全部", ""));
                }
                foreach (var dept in cyc.Global.SysDept.List.Where(p => p.UpperID == 0))
                {
                    ddlDept.Items.Add(new ListItem(dept.Name, dept.ID.ToString()));
                    GetNextDept(ddlDept, dept.ID);
                }
            }
        }

        private static void GetNextDept(DropDownList ddlDept, int iID, string sPrefix = "")
        {
            sPrefix += "　";
            foreach (var dept in cyc.Global.SysDept.List.Where(p => p.UpperID == iID))
            {
                ddlDept.Items.Add(new ListItem(sPrefix + dept.Name, dept.ID.ToString()));
                GetNextDept(ddlDept, dept.ID, sPrefix);
            }
        }

        public static void GetNextDept(int iID, ref List<int> oList)
        {
            foreach (var dept in cyc.Global.SysDept.List.Where(p => p.UpperID == iID))
            {
                oList.Add(dept.ID);
                GetNextDept(dept.ID, ref oList);
            }
        }

        private static bool GetNextDept(int iID, cyc.SysDept oDept)
        {
            var lsDept = cyc.Global.SysDept.List.Where(p => p.UpperID == oDept.ID);
            foreach (var cDept in lsDept)
            {
                if (cDept.ID == iID)
                    return true;
                else
                    return GetNextDept(iID, cDept);
            }
            return false;
        }

        public static bool CheckDeptLimite(cyc.UserInfo oUser, int iID)
        {
            //if (oUser.UserRole.Any(p => p.LevelNo == 1) || oUser.Dept.ID == iID)
            if ((from lsU in oUser.Roles
                 join lsR in cyc.Global.SysRole.List.Where(p => p.LevelNo == 1) on lsU equals lsR.ID
                 select lsR).Count() > 0 || oUser.Dept.ID == iID)
                return true;
            return GetNextDept(iID, oUser.Dept);
        }

        public static string GetDeptLimitSQL(cyc.UserInfo oUser, string sColumn)
        {
            List<int> oList = new List<int>() { oUser.Dept.ID };
            GetNextDept(oUser.Dept.ID, ref oList);
            return string.Format("{0} in ({1})", sColumn, string.Join(",", oList.ToArray()));
        }
    }
}
