using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dashboard.APIControllers;

namespace Dashboard.Controllers
{
    public class CookiesController : Controller
    {
        string[] cookies = { "PlantCookie", "MonthCookie", "YearCookie" };
        string[] defaults = { "All", DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString() };

        public ActionResult SetCookies(string plantName, string month, string year)
        {
            string[] variables = { plantName, month, year };

            int incr = 0;
            foreach (string v in variables)
            {
                if (HttpContext.Request.Cookies[cookies[incr]] != null)
                {
                    HttpCookie c = HttpContext.Request.Cookies[cookies[incr]];
                    c.Expires = DateTime.Now.AddDays(7);
                    c.Value = variables[incr];
                    Response.Cookies.Set(c);
                }
                else
                {
                    HttpCookie c = new HttpCookie(cookies[incr]);
                    c.Expires = DateTime.Now.AddDays(7);
                    c.Value = "All";
                    Response.Cookies.Add(c);
                }
                incr += 1;
            }

            return null;
        }



    }
}
