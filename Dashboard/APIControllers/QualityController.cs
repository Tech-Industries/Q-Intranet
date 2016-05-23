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
    public class QualityController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        [ResponseType(typeof(List<QualityViewModel>))]
        public async Task<IHttpActionResult> Get(string fType, string nameContain, string year, string month)
        {
            System.Diagnostics.Debug.WriteLine(nameContain + ": "+year + '-' + month);
            if (fType == "MTDChart")
            {
                return Ok(await QualityViewModel.MapFromAsync(db.ScrapDailyByPlants.Where(x => x.PlantCode.Equals(nameContain) && x.Year.Equals(year) && x.Month.Equals(month)).OrderBy(o => o.Day).ToList()));
            }
            else if (fType == "Snapshot")
            {
                return Ok(await QualityViewModel.MapFromAsync(db.ScrapMonthlyByPlants.Where(x => x.PlantCode.Equals(nameContain) && x.Year.Equals(year) && x.Month.Equals(month)).ToList()));
            }
            else
            {
                return null;
            }
            
        }

        public async Task<IHttpActionResult> Get(string fType, string nameContain, string year, string month, string day)
        {
           
            if (nameContain == "All")
            {
                return Ok(await QualityViewModel.MapFromAsync(db.ScrapStages.Where(x => x.PlantCode.Contains("") && x.DateDisposed.Equals(year + month + day)).ToList()));
            }
            else
            {
                if (fType == "MonthSnapShot")
                {
                    return Ok(await QualityViewModel.MapFromAsync(db.ScrapStages.Where(x => (x.CompanyCode.Equals(nameContain) || x.PlantCode.Equals(nameContain)) && x.DateDisposed.Substring(0, 6).Equals(year + month)).ToList()));
                }
                else
                {
                    return Ok(await QualityViewModel.MapFromAsync(db.ScrapStages.Where(x => (x.CompanyCode.Equals(nameContain) || x.PlantCode.Equals(nameContain)) && x.DateDisposed.Equals(year + month + day)).ToList()));
                }
            }

        }

    }
}