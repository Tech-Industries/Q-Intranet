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
    public class DeliverableReviewersController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();
        // GET: api/Organizations


        public List<object> Get(int DelID)
        {
            return db.DeliverableReviewers.Join(db.Users, dc => dc.UserID, u => u.ID, (dc, u) => new { dc, u }).Where(x => x.dc.DelID == DelID).Select(x => new { x.dc.ID, x.dc.UserID, x.u.FirstName, x.u.LastName }).ToList<object>();
        }



        // POST: Deliverable Reviewers
        [ResponseType(typeof(DeliverableReviewersViewModel))]
        public async Task<IHttpActionResult> Post(DeliverableReviewer delr)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.DeliverableReviewers.Add(delr);
            await db.SaveChangesAsync();

            return Ok(DeliverableReviewersViewModel.MapFrom(delr));
        }
    }
}