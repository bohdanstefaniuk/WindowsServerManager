using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BLL.Infrastructure;
using NLog;

namespace WebUI.FullFramework
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            ActionLogger.GetInstance().Save();
        }

        //void Application_Error(object sender, EventArgs e)
        //{
        //    // Code that runs when an unhandled error occurs

        //    // Get the exception object.
        //    Exception exc = Server.GetLastError();


        //    // Handle HTTP errors
        //    if (exc.GetType() == typeof(HttpException))
        //    {
        //        // The Complete Error Handling Example generates
        //        // some errors using URLs with "NoCatch" in them;
        //        // ignore these here to simulate what would happen
        //        // if a global.asax handler were not implemented.
        //        if (exc.Message.Contains("NoCatch") || exc.Message.Contains("maxUrlLength"))
        //            return;

        //        //Redirect HTTP errors to HttpError page
        //        Server.Transfer("HttpErrorPage.aspx");
        //    }

        //    // For other kinds of errors give the user some information
        //    // but stay on the default page


        //    // Clear the error from the server
        //    Server.ClearError();
        //}
    }
}
