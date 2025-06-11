using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._uc
{
    public partial class ucYesNo : System.Web.UI.UserControl
    {
        //public string Text
        //{
        //    set
        //    {

        //        this.hidYesNo.Value = value.ToString();
        //        this.lblYesNo.Text = (value ? "是" : "否");
        //    }
        //}
        public bool Value
        {
            get { return Convert.ToBoolean(this.hidYesNo.Value); }
            set
            {
                this.hidYesNo.Value = value.ToString();
                this.lblYesNo.Text = (value ? "是" : "否");
            }
        }
        public string ValueOrginal
        {
            set
            {
                bool bTry = false;
                bool.TryParse(value, out bTry);
                Value = bTry;
            }
        }
    }
}