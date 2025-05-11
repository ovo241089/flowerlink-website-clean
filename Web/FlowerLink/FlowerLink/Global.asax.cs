using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace FlowerLink
{
    public class MvcApplication : System.Web.HttpApplication
    {
        void Application_BeginRequest(object sender, EventArgs e)
        {
            //var a = String.IsNullOrEmpty(HttpContext.Current.Request["lang"]);
            if (String.IsNullOrEmpty(HttpContext.Current.Request["lang"]))
            {
                HttpCookie myCookie = new HttpCookie("lang");
                myCookie.Value = "en";
                myCookie.Expires = DateTime.Now.AddDays(365d);
                HttpContext.Current.Response.Cookies.Add(myCookie);
            }
        }
        protected void Application_Start()
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
