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
    public class FlashController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(FlashReportRollUp))]
        public async Task<IHttpActionResult> Get(int PlantID, string Year, string Month )
        {
            if(PlantID == 0)
            {
                return Ok(await FlashRollupViewModel.MapFromAsync(db.FlashReportRollUps.Where(x => x.PlantCode == "All" && x.Year == Year && x.Month == Month).ToList()));
            }
            else
            {
                return Ok(await FlashRollupViewModel.MapFromAsync(db.FlashReportRollUps.Where(x => x.PlantID == PlantID && x.Year == Year && x.Month == Month).ToList()));
            }
        }
    }
}