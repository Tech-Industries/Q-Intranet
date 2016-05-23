using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Helpers;

namespace Dashboard.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            ViewBag.UID = Response.Cookies["authToken"].Value;
            if (CheckUserAuth.Check())
            {
                if (Request.Cookies.AllKeys.Contains("authToken"))
                {
                    AnonDBChecks anon = new AnonDBChecks();
                    bool eval = anon.CheckForGroup(groupID: 1);
                    if (eval)
                    {
                        return View();
                    }
                    else
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    return this.RedirectToAction("Index", "Login");
                }

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }

        }

        public ActionResult Users()
        {
            ViewBag.UID = Response.Cookies["authToken"].Value;
            if (CheckUserAuth.Check())
            {
                if (Request.Cookies.AllKeys.Contains("authToken"))
                {
                    AnonDBChecks anon = new AnonDBChecks();
                    bool eval = anon.CheckForGroup(groupID: 1);
                    if (eval)
                    {
                        return View();
                    }
                    else
                    {
                        return this.RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return this.RedirectToAction("Index", "Login");
                }

            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}