using Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class UtilityController : Controller
    {
        // GET: Slide
        public ActionResult Employees()
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

        public ActionResult ZipFiles()
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
}