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
    public class GoalsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        // GET: api/Organizations
        [ResponseType(typeof(List<GoalsViewModel>))]
        public async Task<IHttpActionResult> Get(string fType, string nameContain, string month, string year)
        {
            
            
                return Ok(await GoalsViewModel.MapFromAsync(db.Goals.Where(x => x.Type.Equals(fType) && x.PlantCode.Equals(nameContain) && x.Year.Equals(year) && x.Month.Equals(month)).ToList()));
            

        }
    }
}