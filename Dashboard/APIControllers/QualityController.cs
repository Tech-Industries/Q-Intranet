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

        [Route("api/v1/quality/names")]
        [HttpGet]
        public async Task<IHttpActionResult> GetNames(int plantid)
        {
            var names = await db.EmployeeMasters.Where(x => x.PlantID == plantid).Select(x => x.DisplayName).ToListAsync();
            return Ok(names);
        }

        [Route("api/v1/quality/jobs")]
        [HttpGet]
        public async Task<IHttpActionResult> GetJobs(int plantid)
        {
            var jobs = await db.DistinctJobs.Where(x => x.PlantID == plantid).OrderBy(x => x.Job).ToListAsync();
            return Ok(jobs);
        }

        [Route("api/v1/quality/jobs/{job}/suffix")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSuffix(int plantid, string job)
        {
            var suffix = await db.DistinctSuffixes.Where(x => x.Job == job && x.PlantID == plantid).OrderBy(x => x.Suffix).ToListAsync();
            return Ok(suffix);
        }

        [Route("api/v1/quality/jobs/{job}/suffix/{suffix}/seq")]
        [HttpGet]
        public async Task<IHttpActionResult> GetSeq(int plantid, string job, string suffix)
        {
            var seq = await db.OpenSequences.Where(x => x.Job == job && x.Suffix == suffix && x.PlantID == plantid).OrderBy(x => x.Seq).ToListAsync();
            return Ok(seq);
        }

        [Route("api/v1/quality/inspectiontypes")]
        [HttpGet]
        public async Task<IHttpActionResult> GetInspectionTypes(int plantid)
        {
            var types = await db.InspectionTypes.Where(x => x.PlantID == plantid && x.Active == true).OrderBy(o => o.Type).ToListAsync();
            return Ok(types);
        }



        [ResponseType(typeof(List<QualityViewModel>))]
        public async Task<IHttpActionResult> Get(string fType, string nameContain, string year, string month)
        {
            System.Diagnostics.Debug.WriteLine(nameContain + ": " + year + '-' + month);
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