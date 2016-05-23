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
    public class DeliverableDetailController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        // GET: api/Organizations


        public List<object> Get(int DelID)
        {
            return db.DeliverableReviewers.Join(db.Users, dc => dc.UserID, u => u.ID, (dc, u) => new { dc, u }).Where(x => x.dc.DelID == DelID).Select(x => new { x.dc.ID, x.dc.UserID, x.u.FirstName, x.u.LastName }).ToList<object>();
        }



        //// POST: Deliverable Reviewers
        //[ResponseType(typeof(DeliverableReviewersViewModel))]
        //public async Task<IHttpActionResult> Post(DeliverableReviewer delr)
        //{
        //    if (!ModelState.IsValid) { return BadRequest(ModelState); }

        //    db.DeliverableReviewers.Add(delr);
        //    await db.SaveChangesAsync();

        //    return Ok(DeliverableReviewersViewModel.MapFrom(delr));
        //}

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(long ID, DeliverableDetail deldet)
        {
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
                return BadRequest(ModelState);
            }

            if (ID != deldet.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
                return BadRequest();
            }

            DeliverableDetail upDel = (DeliverableDetail)db.DeliverableDetails.Where(x => x.ID == ID).First();
            
            upDel.DateCompleted = DateTime.Now;


            db.DeliverableDetails.Attach(upDel);
            var entry = db.Entry(upDel);
            entry.Property(e => e.DateCompleted).IsModified = true;
            // other changed properties
            db.SaveChanges();

            //db.Entry(deldet).State = EntityState.Modified;
            //System.Diagnostics.Debug.WriteLine("Modified");
            try
            {
                System.Diagnostics.Debug.WriteLine("Saved "+ ID.ToString()+" | "+deldet.DateCompleted.ToString());
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DelDetExists(ID))
                {
                    System.Diagnostics.Debug.WriteLine("Catch If");
                    return NotFound();
                }
                else {
                    System.Diagnostics.Debug.WriteLine("Catch If Else");
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public async Task<IHttpActionResult> Put (DeliverableDetail deldet)
        {
            return await this.Put(deldet.ID, deldet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DelDetExists(long ID)
        {
            return db.DeliverableDetails.Count(e => e.ID == ID) > 0;
        }

    }
}