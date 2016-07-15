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
    public class DeliverableReviewersController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        // GET: api/Organizations


        public List<object> Get(int DelID)
        {
            return db.DeliverableReviewers.Join(db.Users, dc => dc.UserID, u => u.ID, (dc, u) => new { dc, u }).Where(x => x.dc.DelID == DelID).Select(x => new { x.dc.ID, x.dc.UserID, x.u.FirstName, x.u.LastName, x.dc.DateAdded }).ToList<object>();
//            return db.DeliverableReviewers.Join(db.Users, dr => dr.UserID, u => u.ID, (dr, u) => new { dr, u }).Join(db.DeliverableDetails, j => j.dr.DelID, dd => dd.DelID, (j, dd) => new { j, dd }).Where(x => x.j.dr.DelID == DelID && x.dd.DateDue >= x.j.dr.DateAdded).Select(x => new { x.j.dr.ID, x.j.dr.DateAdded, x.j.dr.UserID, x.j.u.FirstName, x.j.u.LastName }).ToList<object>();
        }



        // POST: Deliverable Reviewers
        [ResponseType(typeof(DeliverableReviewersViewModel))]
        public async Task<IHttpActionResult> Post(DeliverableReviewer delr)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            delr.DateAdded = DateTime.Now;
            db.DeliverableReviewers.Add(delr);
            await db.SaveChangesAsync();
            //var UID = int.Parse(HttpContext.Current.Request.Cookies["authToken"].Value);
            //var logDescription = "added a new Reviewer.";
            //var h = new Helpers.Helpers();
            //var del = db.Deliverables.Where(x => x.ID == delr.DelID).First();
            //h.NewLogEntry(UID, "Deliverable", del.ID, "added " + delr.UserID + " as a reviewable to " + del.ID + ".", logDescription);
            return Ok(DeliverableReviewersViewModel.MapFrom(delr));
        }


        [ResponseType(typeof(DeliverableReviewersViewModel))]
        public async Task<IHttpActionResult> Delete(long id)
        {
            DeliverableReviewer c = await db.DeliverableReviewers.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                db.DeliverableReviewers.Remove(c);
                await db.SaveChangesAsync();
                return Ok(DeliverableReviewersViewModel.MapFrom(c));
            }
        }
    }
}