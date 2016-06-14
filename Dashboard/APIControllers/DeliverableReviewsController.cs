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
    public class DeliverableReviewsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();

        public List<object> Get(int DelDetID)
        {
            return db.DeliverableReviews.Join(db.Users, dc => dc.UserID, u => u.ID, (dc, u) => new { dc, u }).Where(x => x.dc.DelDetID == DelDetID).Select(x => new { x.dc.ID, x.dc.TimeReviewed, x.dc.UserID, x.u.FirstName, x.u.LastName }).ToList<object>();
        }

        // POST: Deliverable Review
        [ResponseType(typeof(DeliverableReviewsViewModel))]
        public async Task<IHttpActionResult> Post(DeliverableReview delr)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.DeliverableReviews.Add(delr);
            await db.SaveChangesAsync();

            var UID = int.Parse(HttpContext.Current.Request.Cookies["authToken"].Value);
            var logDescription = "";
            if (delr.UserID != UID)
            {
                var behalf = db.Users.Where(x => x.ID == delr.UserID).First();
                logDescription = "reviewed a deliverable on behalf of " + behalf.FirstName + " " + behalf.LastName + ".";
            }
            else
            {
                logDescription = "reviewed a deliverable.";
            }
            var h = new Helpers.Helpers();
            h.NewLogEntry(UID, "DeliverableDetail", (int)delr.DelDetID, "Reviewed", logDescription);

            return Ok(DeliverableReviewsViewModel.MapFrom(delr));
        }
    }
}