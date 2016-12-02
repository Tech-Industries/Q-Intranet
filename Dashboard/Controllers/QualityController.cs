using Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class QualityController : Controller
    {
        // GET: Quality
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 27);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 28) ? 28 : 0;
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
        public ActionResult InspectionSheet()
        {
            return View();
        }
        public ActionResult InspectionRequest()
        {
            return View();
        }
    }
}
    