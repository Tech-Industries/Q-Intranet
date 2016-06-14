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

namespace Dashboard.APIControllers
{
    public class PageController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        
        public List<object> Get(string Name)
        {
            return db.Pages.Where(x => x.Name == Name).ToList<object>();
        }
       
    }
}