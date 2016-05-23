using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.Helpers;
using System.IO;
using System.Net;
using Dashboard.Controllers;
using Dashboard.APIControllers;

namespace Dashboard.Controllers
{
    public class PrevMaintController : Controller
    {
        // GET: PrevMaint
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 12);
                if (eval)
                {
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