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
        public async Task<IHttpActionResult> Get(int PlantID, string Year, string Month)
        {
            if (PlantID == 0)
            {
                return Ok(await FlashRollupAllViewModel.MapFromAsync(db.FlashReportRollUpAlls.Where(x => x.Year == Year && x.Month == Month).ToList()));
            }
            else
            {
                return Ok(await FlashRollupViewModel.MapFromAsync(db.FlashReportRollUps.Where(x => x.PlantID == PlantID && x.Year == Year && x.Month == Month).ToList()));
            }
        }

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int PlantID, int Year, int Month, int Range)
        {
            int lowerMonth = Month - Range;
            int lowerYear = Year;
            if (lowerMonth <= 0)
            {
                lowerMonth += 12;
                lowerYear -= 1;
            }
            Console.WriteLine(lowerYear + " - " + lowerMonth);
            Console.WriteLine(Year + " - " + Month);
            if (Year == lowerYear)
            {
                return db.OnTimeDeliveryTrendInts.Where(x => x.PlantID == PlantID && ((x.Year == Year && x.Month <= Month) && (x.Year == lowerYear && x.Month >= lowerMonth))).OrderBy(o => o.Month).ToList<object>();
            }
            else
            {
                return db.OnTimeDeliveryTrendInts.Where(x => x.PlantID == PlantID && ((x.Year == Year && x.Month <= Month) || (x.Year == lowerYear && x.Month >= lowerMonth))).OrderBy(o => o.Year).ThenBy(o => o.Month).ToList<object>();
            }

        }
    }
}