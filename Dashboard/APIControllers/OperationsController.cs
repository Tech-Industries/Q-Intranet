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
    

    public class Operations2Controller : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        #region Shipping

        [Route("api/v1/operations/shipping")]
        [HttpGet]
        public async Task<IHttpActionResult> GetShippingPlan()
        {
            var plans = await db.ShipAdjDates.OrderBy(o => o.DateDue).ToListAsync();
            if (!plans.Any())
            {
                return NotFound();
            }
            return (Ok(plans));
        }

        [Route("api/v1/operations/shipping")]
        [HttpPost]
        public async Task<IHttpActionResult> PostShippingAdjDate(ShippingPlanAdjShipDate s)
        {
            db.ShippingPlanAdjShipDates.Add(s);
            await db.SaveChangesAsync();


            return Ok(await ShipAdjViewModel.MapFromAsync(s));
        }

        [Route("api/v1/operations/shipping/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutShippingAdjDate(int id, [FromBody]ShippingPlanAdjShipDate s)
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
            }

            if (id != s.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
            }

            var ups = await db.ShippingPlanAdjShipDates.FindAsync(id);
            ups.AdjShipDate = s.AdjShipDate;

            db.ShippingPlanAdjShipDates.Attach(ups);
            var entry = db.Entry(ups);
            entry.Property(e => e.AdjShipDate).IsModified = true;
            // other changed properties
            db.SaveChanges();
            return Ok();


        }

        #endregion

        #region Staging

        [Route("api/v1/operations/staging")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStaging(int DaysOut = 14)
        {
            var dateComp = DateTime.Now.AddDays(DaysOut).Date;
            var stagDets = await db.StagingTopLevels.Where(x => x.DateStageDue <= dateComp).OrderBy(o => o.Job).ToListAsync();
            if (!stagDets.Any())
            {
                return NotFound();
            }
            return Ok(stagDets);
        }



        [Route("api/v1/operations/stagingdetail")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingDetail(string Job)
        {
            var stagDets = await db.StagingDetails.Where(x => x.Job == Job).OrderBy(o => o.Job).ToListAsync();
            if (!stagDets.Any())
            {
                return NotFound();
            }
            return Ok(stagDets);
        }

        [Route("api/v1/operations/stagingcriteria")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingCriteria()
        {
            var stagDets = await db.StagingCriterias.Where(x => x.Active == true).ToListAsync();
            if (!stagDets.Any())
            {
                return NotFound();
            }
            return Ok(stagDets);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}