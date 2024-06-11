using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebAppForm
{
    public class Global : HttpApplication
    {
        ILog log = log4net.LogManager.GetLogger(typeof(Global));
        //string IP = new EventLogging().GetIP();
        

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure();
        }
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            var url = Request.Url.AbsoluteUri;
            var path = Request.Url.AbsolutePath;
            var host = Request.Url.Host;


            log.Info(url + " " + path + " " + host);

        }

        protected void Page_Error(object sender, EventArgs e)
        {
            Exception Ex = Server.GetLastError();
            Server.ClearError();
            string error = "IP: " + new EventLogging().GetIP() + "\nURL: " + System.Web.HttpContext.Current.Request.Url + "\n";
            error += String.Format("Error Occured\nMessage: {0}\nInner Exception: {1}", Ex.Message, Ex.InnerException);
            log.Error(error);
        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs  
            Exception Ex = Server.GetLastError();            
            Server.ClearError();
            string error = "IP: " + new EventLogging().GetIP() + "\nURL: " + System.Web.HttpContext.Current.Request.Url + "\n";
            error += String.Format("Error Occured\nMessage: {0}\nInner Exception: {1}", Ex.Message, Ex.InnerException);
            log.Error(error);
            //Server.Transfer("ErrorPages\\OOPS.aspx");
        }
    }
}