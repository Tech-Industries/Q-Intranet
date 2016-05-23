using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Helpers;

namespace Dashboard.Controllers
{
    public class BugsController : Controller
    {
        // GET: Bugs
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 8);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 7) ? 7 : 0;
                    ViewBag.UID = Response.Cookies["authToken"].Value;
                    return View();
                }
                else
                {
                    return this.RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
    }
}