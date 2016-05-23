using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.APIControllers;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                ViewBag.UID = Response.Cookies["authToken"].Value;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }

    public class CheckUserAuth
    {
        public static bool Check()
        {
            if (HttpContext.Current.Request.Cookies.AllKeys.Contains("authToken"))
            {
                var id = HttpContext.Current.Request.Cookies["authToken"].Value;
                HttpContext.Current.Response.Cookies["authToken"].Expires = DateTime.Now.AddMinutes(120);
                HttpContext.Current.Response.Cookies["authToken"].Value = id;
                var fullName = HttpContext.Current.Request.Cookies["FullName"].Value;
                HttpContext.Current.Response.Cookies["FullName"].Expires = DateTime.Now.AddMinutes(120);
                HttpContext.Current.Response.Cookies["FullName"].Value = fullName;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
