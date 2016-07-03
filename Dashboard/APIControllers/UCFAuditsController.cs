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
    public class UCFAuditsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        
        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID)
        {
            return db.UCFAreas.Where(x => x.PlantID == ID).OrderBy(o => o.Name).ToList<object>();

        }


        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID, int Year, int Month, int Range)
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
                return db.UCFPlantRollupByMonthHalves.Where(x => x.PlantID == ID && (x.Year == Year && x.Month <= Month) && (x.Year == lowerYear && x.Month >= lowerMonth)).OrderBy(o => o.Month).ThenBy(o => o.Half).ToList<object>();
            }
            else
            {
                return db.UCFPlantRollupByMonthHalves.Where(x => x.PlantID == ID && (x.Year == Year && x.Month <= Month) || (x.Year == lowerYear && x.Month >= lowerMonth)).OrderBy(o => o.Year).ThenBy(o => o.Month).ThenBy(o => o.Half).ToList<object>();
            }

        }

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int PlantID, int AreaID, int Year, int Month, int Range)
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
                return db.UCFAreaTrendByMonthHalves.Where(x => x.PlantID == PlantID && x.AreaID == AreaID && (x.Year == Year && x.Month <= Month) && (x.Year == lowerYear && x.Month >= lowerMonth)).OrderBy(o => o.Month).ThenBy(o => o.Half).ToList<object>();
            }
            else
            {
                return db.UCFAreaTrendByMonthHalves.Where(x => x.PlantID == PlantID && x.AreaID == AreaID && ((x.Year == Year && x.Month <= Month) || (x.Year == lowerYear && x.Month >= lowerMonth))).OrderBy(o => o.Year).ThenBy(o => o.Month).ThenBy(o => o.Half).ToList<object>();
            }

        }

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int PlantID, string Year, string Month)
        {
            int y = Int32.Parse(Year);
            int m = Int32.Parse(Month);

            return db.UCFAveragesByMonths.Where(x => x.PlantID == PlantID && x.Year == y && x.Month == m).ToList<object>();
        }

        [ResponseType(typeof(List<object>))]
        public List<object> Get(int ID, string Type)
        {
            if (Type == "PreviousScores")
            {
                UCFAudit audit = db.UCFAudits.Where(x => x.AreaID == ID).OrderByDescending(o => o.DateCompleted).ToList<UCFAudit>()[0];
                return db.UCFPreviousScoresByChallenges.Where(x => x.AuditID == audit.ID).OrderBy(o => o.ChallengeID).ToList<object>();
            }
            else if (Type == "TopLevelHistory")
            {

                return db.UCFAuditHIstoryTopLevels.Where(x => x.AreaID == ID).OrderByDescending(o => o.DateCompleted).ToList<object>();
            }
            else if (Type == "Detail")
            {

                return db.UCFAuditDetailLevels.Where(x => x.AuditID == ID).OrderBy(o => o.CategoryID).ThenBy(o => o.ChallengeID).ThenBy(o => o.CriteriaID).ToList<object>();
            }
            else
            {
                return new List<object>();
            }
        }

        [ResponseType(typeof(UCFAuditsViewModel))]
        public async Task<IHttpActionResult> Post(UCFAudit audit)
        {
            audit.DateCompleted = DateTime.Now;
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.UCFAudits.Add(audit);
            await db.SaveChangesAsync();

            return Ok(UCFAuditsViewModel.MapFrom(audit));

        }
    }
}