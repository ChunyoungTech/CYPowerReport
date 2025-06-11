using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApp._uc
{
    public partial class ucDate : System.Web.UI.UserControl
    {
        public delegate void DateChangedEventHandler(object sender, EventArgs e);
        public event DateChangedEventHandler DateChanged;

        public bool AutoPostBack
        {
            get { return txtDate.AutoPostBack; }
            set { txtDate.AutoPostBack = value; }
        }

        public bool Enabled
        {
            get { return this.txtDate.Visible; }
            set
            {
                this.txtDate.Visible = value;
                this.lblDate.Visible = !this.txtDate.Visible;
            }
        }

        public string Text
        {
            get { return this.txtDate.Text.Trim(); }
            set
            {
                this.txtDate.Text = "";
                DateTime dDate = new DateTime();
                if (DateTime.TryParse(value, out dDate))
                {
                    this.txtDate.Text = dDate.ToString("yyyy/MM/dd");
                    this.lblDate.Text = dDate.ToString("yyyy/MM/dd");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (txtDate.Enabled)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "akey" + ClientID, "<script type=\"text/javascript\">" + "$('#" + txtDate.ClientID + "').datepicker($.datepicker.regional['zh-TW']);" + "</script>", false);
        }

        protected void txtDate_TextChanged(object sender, EventArgs e)
        {
            if (AutoPostBack) { DateChanged(this, e); }
        }
    }
}