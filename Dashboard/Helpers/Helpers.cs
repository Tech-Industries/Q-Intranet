using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dashboard.Models;
using Dashboard.ViewModels;
using Dashboard.APIControllers;

namespace Dashboard.Helpers
{
    public class Helpers
    {
        DashboardEntities db = new DashboardEntities();

        public void NewLogEntry(int UserID, string Reference, int ReferenceID, string Action, string Description)
        {
            Log l = new Log();
            l.UserID = UserID;
            l.Reference = Reference;
            l.ReferenceID = ReferenceID;
            l.Action = Action;
            l.Description = Description;
            l.DateCreated = DateTime.Now;
            LogController lc = new LogController();
            lc.Post(l);
        }
    }
}