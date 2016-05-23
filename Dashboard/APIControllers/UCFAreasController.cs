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
    public class UCFAreasController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID)
        {
            return db.UCFAreas.Where(x => x.PlantID == ID).OrderBy(o => o.Name).ToList<object>();
            
        }


        [ResponseType(typeof(List<object>))]
        public void Post(UCFArea area)
        {
            db.UCFAreas.Add(area);
            db.SaveChangesAsync();
        }
    }
}