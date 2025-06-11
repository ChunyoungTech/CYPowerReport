using System;
using cyc.Page;
using cyc.Comm;

namespace WebApp._sys
{
    public partial class GrowattAPISetting : BasePageDB
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtUrl.Text = SysQuery.GetSysSettingValue("GrowattApiUrl");
                txtAccount.Text = SysQuery.GetSysSettingValue("GrowattAccount");
                txtPassword.Attributes["value"] = SysQuery.GetSysSettingValue("GrowattPassword");
                txtToken.Text = SysQuery.GetSysSettingValue("GrowattToken");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SysQuery.UpdateSysSetting("GrowattApiUrl", txtUrl.Text.Trim());
            SysQuery.UpdateSysSetting("GrowattAccount", txtAccount.Text.Trim());
            SysQuery.UpdateSysSetting("GrowattPassword", txtPassword.Text.Trim());
            SysQuery.UpdateSysSetting("GrowattToken", txtToken.Text.Trim());
            ShowResult("儲存完成", false, false);
        }

        protected void btnReload_Click(object sender, EventArgs e)
        {
            DateTime theDate;
            if (!DateTime.TryParse(txtDate.Text.Trim(), out theDate))
            {
                oResult.Error("日期格式錯誤");
                ShowResult(oResult.Message, false, false);
                return;
            }
            try
            {
                dDB.Execute("EXEC sp_ImportGrowattDaily @TheDate", new { TheDate = theDate.ToString("yyyy/MM/dd") });
                ShowResult("重新取得完成", false, false);
            }
            catch (Exception ex)
            {
                Global.WriteSysError(ex.Message, oResult);
                ShowResult("重新取得失敗", false, false);
            }
        }
    }
}
