﻿using System;
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
using System.Web.Http.Results;
using PdfSharp;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.Rendering;
using System.Diagnostics;

namespace Dashboard.APIControllers
{
    

    public class Operations2Controller : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        #region Opportunities

        [Route("api/v1/operations/opportunities")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOpportunities()
        {

            var opps = await db.Opportunities.ToListAsync();
            if (!opps.Any())
            {
                return NotFound();
            }
            return Ok(opps);
        }

        [Route("api/v1/operations/opportunities/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetOpportunity(int id)
        {

            var opps = await db.Opportunities.FindAsync(id);
            if (opps == null)
            {
                return NotFound();
            }
            return Ok(opps);
        }

        [Route("api/v1/operations/opportunities")]
        [HttpPost]
        public async Task<IHttpActionResult> PostOpportunities(Opportunity o)
        {
            db.Opportunities.Add(o);
            await db.SaveChangesAsync();

            return Ok(o);
        }

        [Route("api/v1/operations/opportunities/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutOpportunities(int id, Opportunity o)
        {
            Opportunity oldOp = await db.Opportunities.FindAsync(id);


            if (oldOp == null || o == null)
            {
                return NotFound();
            }
            try
            {
                var entry = db.Entry(oldOp);

                oldOp = o;

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldOp);
        }

        [Route("api/v1/operations/opportunities/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteOpportunity(int id)
        {
            try
            {
                db.Opportunities.Remove(await db.Opportunities.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return Ok();
        }
        #endregion

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

        static Document CreateDocument(string body)
        {
            Document document = new Document();

            Section section = document.AddSection();

            Paragraph paragraph = section.AddParagraph();

            paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);

            paragraph.AddFormattedText(body);

            return document;
        }

        #region Staging
        

        [Route("api/v1/operations/staging")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStaging(int DaysOut = 14)
        {
            var dateComp = DateTime.Now.AddDays(DaysOut).Date;
            var stagDets = await db.StagingTopLevelStatus.Where(x => x.DateStageDue <= dateComp && x.Suffix != "" && x.DATE_CLOSED.ToString() == "1900-01-01" ).OrderBy(o => o.Job).ToListAsync();
            if (!stagDets.Any())
            {
                return NotFound();
            }
            return Ok(stagDets);
        }

        [Route("api/v1/operations/staging")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingByJob(string Job, string Suffix = "")
        {
            
            var stagDets = await db.StagingTopLevelStatus.Where(x => x.Job == Job && x.Suffix == Suffix).OrderBy(o => o.Job).ToListAsync();
            if (!stagDets.Any())
            {
                return NotFound();
            }
            return Ok(stagDets);
        }



        [Route("api/v1/operations/stagingdetail")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingDetail(string Job, string Suffix = "")
        {
            var stagDets = await db.StagingDetailAuditStatus.Where(x => x.Job == Job && x.Suffix == Suffix).OrderBy(o => o.Job).ToListAsync();
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

        [Route("api/v1/operations/stagingaudit/{id:int}/checklist")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingCheckList(int id)
        {
            var stagDets = await db.StagingCheckLists.Where(x => x.StagingDetailID == id).ToListAsync();
            if (!stagDets.Any())
            {
                return NotFound();
            }
            return Ok(stagDets);
        }

        [Route("api/v1/operations/stagingaudit")]
        [HttpPost]
        public async Task<IHttpActionResult> PostItem(StagingAudit s)
        {
            db.StagingAudits.Add(s);
            await db.SaveChangesAsync();

            return Ok(s);
        }

        [Route("api/v1/operations/stagingaudit/detail/{id:int}")]
        [HttpPut]
        public async Task<IHttpActionResult> PutStagingAuditDetail(int id, StagingAuditDetail s)
        {
            StagingAuditDetail oldDet = db.StagingAuditDetails.Find(id);

            
            if (oldDet == null || s == null)
            {
                return NotFound();
            }
            try
            {
                var entry = db.Entry(oldDet);
                if (oldDet.DateCompleted == null && s.Status == "Complete")
                {
                    oldDet.DateCompleted = DateTime.Now;
                }
                else
                {
                    oldDet.DateCompleted = null;
                }
                oldDet.Status = s.Status;
                
                

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldDet);
        }

        [Route("api/v1/operations/stagingitems")]
        [HttpGet]
        public async Task<IHttpActionResult> GetAllStagingItems()
        {
            var stagItems = await db.StagingItems.ToListAsync();
            if (!stagItems.Any())
            {
                return NotFound();
            }
            return Ok(stagItems);
        }

        [Route("api/v1/operations/stagingitems/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingItems(int id)
        {
            var stagItems = await db.StagingItems.Where(x => x.StagingDetailID == id).ToListAsync();
            if (!stagItems.Any())
            {
                return NotFound();
            }
            return Ok(stagItems);
        }


        [Route("api/v1/operations/stagingitems")]
        [HttpPost]
        public async Task<IHttpActionResult> PostItem(StagingItem s)
        {
            s.DateIssued = DateTime.Now;
            db.StagingItems.Add(s);
            await db.SaveChangesAsync();

            return Ok(s);
        }

        [Route("api/v1/operations/stagingitems/{id:int}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStagingItems(int id, StagingItem s)
        {
            var oldItem = await db.StagingItems.FindAsync(id);

            if (oldItem == null || s == null)
            {
                return NotFound();
            }
            try
            {

                var entry = db.Entry(oldItem);

                oldItem.DateReturned = DateTime.Now;


                //entry.State = EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            return Ok(oldItem);


        }

        [Route("api/v1/operations/stagingitems/{id:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteItem(int id)
        {
            try
            {
                db.StagingItems.Remove(await db.StagingItems.FindAsync(id));
                await db.SaveChangesAsync();
            }
            catch
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return Ok();
        }



        [Route("api/v1/operations/stagingsnapshot")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingSnapshot()
        {
            var stagItems = await db.StagingSnapshots.ToListAsync();
            if (!stagItems.Any())
            {
                return NotFound();
            }
            return Ok(stagItems[0]);
        }

        [Route("api/v1/operations/stagingsnapshot/trend")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStagingSnapshotTrend(string Period)
        {
            int Year = DateTime.Now.Year;
            int Month = DateTime.Now.Month;
            try { 
            Year = int.Parse(Period.Split('-')[0]);
            Month = int.Parse(Period.Split('-')[1]);
            }
            catch(Exception e)
            {
                return BadRequest(e.ToString());
            }
            var stagItems = await db.StagingSnapshotTrends.Where(x => x.Year == Year && x.Month == Month).OrderBy(o => o.DatePulled).ToListAsync();
            if (!stagItems.Any())
            {
                return NotFound();
            }
            return Ok(stagItems);

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