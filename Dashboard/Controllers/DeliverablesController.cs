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
    public class DeliverablesController : Controller
    {
        // GET: Deliverables
        public ActionResult Index()
        {
            if (CheckUserAuth.Check())
            {
                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 2);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 3) ? 3 : 0;
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
        public ActionResult Reviewables()
        {
            if (CheckUserAuth.Check())
            {

                AnonDBChecks anon = new AnonDBChecks();
                bool eval = anon.CheckForGroup(groupID: 2);
                if (eval)
                {
                    ViewBag.RelPer = anon.CheckForGroup(groupID: 3) ? 3 : 0;
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

        [System.Web.Http.HttpPost]
        public string UploadFile(object sender, EventArgs e)
        {
            
            try
            {
                var concat = "";
                var path = "";
                foreach (string f in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[f];
                    
                    
                    var fName = Path.GetFileName(file.FileName);


                    path = "C:\\temp\\uploads\\"+ fName;
                    concat += "{NewPath: " + path+"}";
                    file.SaveAs(path);
                    
                    
                    
                }

                return path;
            }
            catch(Exception ex)
            {
               return ex.ToString();
            }
        }

    }
}