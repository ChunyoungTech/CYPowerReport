using System;


namespace WebApp
{
    public class Global : System.Web.HttpApplication
    {
        //static AutoSignal oSignal = null; // (暫不用)
        AutoImport oImportTask = null;
        protected void Application_Start(object sender, EventArgs e)
        {
            if (cyc.Comm.SysQuery.GetAppSettingValue("DoAutoImport") == "1")
            {
                oImportTask = new AutoImport();
                oImportTask.Run();
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (oImportTask != null) { oImportTask.Dispose(); }
            cyc.Global.Close();
        }
    }
}