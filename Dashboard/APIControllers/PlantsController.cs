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
using System.Web;

namespace Dashboard.APIControllers
{
    public class PlantsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<object>))]
        public List<object> Get()
        {
            return db.Plants.ToList<object>();
            
        }

    }
}