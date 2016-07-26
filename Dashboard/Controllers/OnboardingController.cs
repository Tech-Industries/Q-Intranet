using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Helpers;

namespace Dashboard.Controllers
{
    public class OnboardingController : Controller
    {
        // GET: Projects
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 17); 
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 18) ? 18 : 0;
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