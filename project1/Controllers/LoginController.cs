using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using project1.Models;

namespace project1.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Message = "Login Page";
            return View();
        }
        public ActionResult Logout()
        {
            if (Session["username"] != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Index");          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User userObj)
        {
            if (ModelState.IsValid)
            {
                int LIMIT_ACTIONS_PER_DAY = Convert.ToInt32(ConfigurationManager.AppSettings["limitedActionsPerUser"]);
                using (Project1DBEntities db = new Project1DBEntities())
                {
                    var obj = db.Users.Where(x => x.UserName == userObj.UserName &&
                    x.Password == userObj.Password).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.NumOfActions.HasValue &&
                           obj.LastActionDate.HasValue &&
                           obj.LastActionDate.Value.Date == DateTime.Now.Date &&
                           obj.NumOfActions.Value > LIMIT_ACTIONS_PER_DAY)
                        {
                            TempData["message"] = "Used all your actions for today, come back tomorrow!";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            Session["username"] = obj.UserName;
                            Session["userid"] = obj.ID;
                        }
                    }
                    else
                    {
                        //ViewBag.ErrorMessage = "Wrong Credentials!";
                        TempData["message"] = "Login Failed!";
                        return RedirectToAction("Index");
                        //  return RedirectToAction("Index", "Login");
                    }
                   
                }
            }
            // return View(userObj)
            return RedirectToAction("Index", "Home");

        }

    }
}