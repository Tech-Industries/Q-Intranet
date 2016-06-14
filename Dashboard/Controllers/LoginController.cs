//using FormsAuth;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dashboard.ViewModels;

using Dashboard.APIControllers;
using System.Threading.Tasks;
using System.Web.Http;
using System.Diagnostics;
using Dashboard.Models;
using System.Data.Entity;
using Dashboard.Helpers;

namespace Dashboard.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        private DashboardEntities db = new DashboardEntities();
        public ActionResult Login(String UserName, String password)
        {
            // APIControllers.LoginController LC = new APIControllers.LoginController();
            //SearchResultCollection results = null;
            List<string> paths = new List<string>();
            paths.Add("LDAP://besttool.local");
            paths.Add("LDAP://ti-kc.local");
            paths.Add("LDAP://192.168.100.244");

            foreach (string path in paths)
            {
                try
                {
                    DirectoryEntry de = new DirectoryEntry(path, UserName, password, AuthenticationTypes.Secure);

                    DirectorySearcher ds = new DirectorySearcher(de);

                    ds.PropertiesToLoad.Add("sn");
                    SearchResult searchResult = ds.FindOne();
                    if (searchResult.Properties.Contains("sn"))
                    {
                        System.Diagnostics.Debug.WriteLine(searchResult.Properties["sn"][0].ToString());
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("dog");
                    }


                    List<Models.User> test = db.Users.Where(x => x.UserName == UserName).ToList();
                    int leng = test.Count();
                    if (leng > 0)
                    {

                        Debug.WriteLine(test[0].ID);
                        HttpCookie authCookie = new HttpCookie("authToken");
                        authCookie.Value = test[0].ID.ToString();
                        authCookie.Expires = DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(authCookie);
                            
                        string FullName = test[0].FirstName + " " + test[0].LastName;
                        HttpCookie cookie = new HttpCookie("FullName");
                        cookie.Value = FullName;
                        cookie.Expires = DateTime.Now.AddMinutes(120);
                        Response.Cookies.Add(cookie);

                        Session["FullName"] = FullName;


                        var h = new Helpers.Helpers();
                        h.NewLogEntry(test[0].ID, "Login", 0, "Success", "logged into Q.");
                        return RedirectToAction("Index", "Deliverables");
                    }
                    else
                    {

                        TempData["UserName"] = UserName;
                        return RedirectToAction("FirstTime", "Login");
                    }


                }
                catch
                {

                }


            }
            var hh = new Helpers.Helpers();
            hh.NewLogEntry(0, "Login", 0, "Fail", UserName + " attempted to log into Q.");
            return RedirectToAction("Index", "Login");
        }

        public ActionResult FirstTime()
        {
            ViewBag.UserName = TempData["UserName"];
            return View();
        }

        public ActionResult Logout()
        {
            try
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["authToken"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                cookie = this.ControllerContext.HttpContext.Request.Cookies["FullName"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
                return RedirectToAction("Index", "Login");
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }

        }
    }
}