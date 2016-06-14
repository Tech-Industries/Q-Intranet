using Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Dashboard.Controllers
{
    public class UltraController : Controller
    {
        // GET: Ultra
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 10);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 10) ? "Read" : "null";
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 9) ? "Admin" : ViewBag.RelPer;
                    ViewBag.UID = Response.Cookies["authToken"].Value;
                    return View();
                }
                else
                {
                    return this.RedirectToAction("Index", "Ultra");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        public ActionResult Admin()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 10);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 9) ? 9 : 0;
                    ViewBag.UID = Response.Cookies["authToken"].Value;
                    return View();
                }
                else
                {
                    return this.RedirectToAction("Admin", "Ultra");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}