using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Dashboard.Models;
using Dashboard.APIControllers;

namespace Dashboard.Helpers
{

    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string IsAuth(this HtmlHelper html, int groupID)
        {
            string cssClass = "hidden";

            AnonDBChecks anon = new AnonDBChecks();
            bool evaluation = anon.CheckForGroup(groupID);
            return evaluation ?  String.Empty : cssClass;

        }


        public static MvcHtmlString IfAuth(this MvcHtmlString value, int groupID)
        {
            //bool evaluation = groupid == 1 && userid == 1 ? true : false;
            AnonDBChecks anon = new AnonDBChecks();
            bool evaluation = anon.CheckForGroup(groupID);
            return evaluation ?  value : MvcHtmlString.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

    }

    public class AnonDBChecks
    {
        DashboardEntities db = new DashboardEntities();
        public bool CheckForGroup(int? groupID)
        {
            int userID = int.Parse(HttpContext.Current.Request.Cookies["authToken"].Value);
            System.Diagnostics.Debug.WriteLine(userID.ToString()+ '-'+groupID.ToString());
            List<UserGroup> list = db.UserGroups.Where(x => x.UserID == userID && x.GroupID == groupID).ToList();
            System.Diagnostics.Debug.WriteLine(list.Count);
            return list.Count > 0 ? true : false;

        }
    }
}
