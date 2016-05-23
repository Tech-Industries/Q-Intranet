using Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dashboard.Controllers
{
    public class MachineController : Controller
    {
        // GET: Machine
        public ActionResult Overview()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 4);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 4) ? 4 : 0;
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