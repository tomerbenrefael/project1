using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Configuration;
using project1.Models;
using System.Security.Policy;

namespace project1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        readonly int LIMIT_ACTIONS_PER_DAY = Convert.ToInt32(ConfigurationManager.AppSettings["limitedActionsPerUser"]);

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapRoute(
            "Edit",                                              // Route name
            "{controller}/{action}/{id}",                           // URL with parameters
            new { controller = "Employee", action = "Edit" }  // Parameter defaults
        );
            //RouteTable.Routes.MapRoute("" +
            //    "Edit", "~{controller}/{action}/{id}",
            //    new { Controller = "Employee", Action = "Edit", empID = UrlParameter.Optional });
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        void Application_AcquireRequestState(object sender,EventArgs e)
        {

            if (Request.Url.AbsolutePath!= "/Login/Logout" && HttpContext.Current.Session != null && HttpContext.Current.Session["userid"] != null)
            {
                int userID = (int)HttpContext.Current.Session["userid"];
                var t = new Project1DBEntities();
                var user = t.Users.Where(g => g.ID == userID).FirstOrDefault();

                if (user != null)
                {
                   
                    if (user.LastActionDate.HasValue)
                    {
                        if (user.LastActionDate.Value.Date == DateTime.Now.Date)
                        {
                            if (user.NumOfActions > LIMIT_ACTIONS_PER_DAY)
                            {
                                Response.Redirect("/Login/Logout");
                            }
                        }
                    }
                }

            }
            //int? userID = (int)Session["userid"];
            //if (userID.HasValue)
            //{
            //    var t = new Project1DBEntities();
            //   // var currentUser = 

            //}


        }
         void Application_PostRequestHandlerExecute(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["userid"] != null)
            {
                int userID = (int)HttpContext.Current.Session["userid"];
                var t = new Project1DBEntities();
                var user = t.Users.Where(g => g.ID == userID).FirstOrDefault();

                if (user != null)
                {

                    if (user.LastActionDate.HasValue)
                    {
                        if (user.LastActionDate.Value.Date == DateTime.Now.Date)
                        {
                            user.NumOfActions += 1;
                            user.LastActionDate = DateTime.Now;
                        }
                        else
                        {
                            user.NumOfActions = 1;
                            user.LastActionDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        user.LastActionDate = DateTime.Now;
                        user.NumOfActions = 1;

                    }

                    t.SaveChanges();
                }
            }
        }
    }
}
