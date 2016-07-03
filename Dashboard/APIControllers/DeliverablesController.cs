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
using Dashboard.ViewModels;
using System.Web;
using Orizon.Web.Data;
using System.Web.Http.Results;

namespace Dashboard.APIControllers
{

    public class Deliverabes2Controller : ApiController
    {

        private OrizonEntities db = new OrizonEntities();

        #region Deliverables

        [Route("api/v1/deliverables")]
        [ResponseType(typeof(IHttpActionResult))]
        [HttpGet]
        public async Task<IHttpActionResult> GetDeliverables()
        {
            var dels = await db.Deliverables.ToListAsync();
            if (!dels.Any())
            {
                return NotFound();
            }
            return Ok(dels);
        }

        [Route("api/v1/deliverables")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDeliverablesByUser(int userId)
        {
            var dels = await db.Deliverables.Where(x => x.UserID == userId).ToListAsync();
            if (!dels.Any())
            {
                return NotFound();
            }
            return Ok(dels);
        }


        #endregion


        #region Deliverable Detail
        [Route("api/v1/deliverables/detail/{id:int}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetDeliverableDetail(int id)
        {
            var detail = await db.DeliverableDetails.Join(db.Deliverables, dd => dd.DelID, d => d.ID, (dd, d) => new { dd, d }).Where(x => x.dd.ID == id).Select(x => new { x.dd.ID, x.dd.DateDue, x.dd.DateCompleted, x.dd.DelID, x.d.UserID, x.d.Name, x.d.Description, x.d.Frequency }).ToListAsync();
            if (!detail.Any())
            {
                return NotFound();
            }
            return Ok(detail);
        }
        #endregion

        #region Deliverable Reviewers
        [Route("api/v1/deliverables/{delId:int}/reviewers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetReviewers(int delId)
        {
            var reviewers = await db.DeliverableReviewers.Where(x => x.DelID == delId).ToListAsync();
            if (!reviewers.Any())
            {
                return NotFound();
            }
            return Ok(reviewers);
        }

        [Route("api/v1/deliverables/{delId:int}/reviewers")]
        [HttpPost]
        public async Task<IHttpActionResult> PostReviewer(int delId, DeliverableReviewer reviewer)
        {
            try
            {
                db.DeliverableReviewers.Add(reviewer);
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ResponseMessageResult(new HttpResponseMessage(HttpStatusCode.Created));
        }

        [Route("api/v1/deliverables/reviewers/{reviewerId:int}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteReviewer(int reviewerId)
        {
            try
            {
                db.DeliverableReviewers.Remove(await db.DeliverableReviewers.FindAsync(reviewerId));
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return Ok();
        }
        #endregion


        #region Deliverable Reviews
        [Route("api/v1/deliverables/detail/{delDetId:int}/reviews")]
        [HttpGet]
        public async Task<IHttpActionResult> GetReviewByDetailId(int delDetId)
        {
            var reviews = await db.DeliverableReviews.Where(x => x.DelDetID == delDetId).ToListAsync();
            if (!reviews.Any())
            {
                return NotFound();
            }
            return Ok(reviews);
        }

        #endregion

        #region DeliverableDocuments
        /// Convert to Universal Documents controller
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
    public class DeliverablesController : ApiController
    {
        private OrizonEntities db = new OrizonEntities();
        // GET: api/Organizations
        [ResponseType(typeof(List<DeliverablesViewModel>))]
        public async Task<IHttpActionResult> Get(int UserID)
        {


            return Ok(await DeliverablesViewModel.MapFromAsync(db.Deliverables.Where(x => x.UserID == UserID).ToList()));


        }

        public object Get(string Frequency, int DelID, string Period, int Year)
        {
            DateTime curDate = DateTime.Now;
            string curY = curDate.Year.ToString();
            string curM = curDate.Month.ToString(); curM = int.Parse(curM) < 10 ? "0" + curM : curM;
            string curD = curDate.Day.ToString(); curD = int.Parse(curD) < 10 ? "0" + curD : curD;

            int year = Year;
            int month = int.Parse(curM);
            int day = int.Parse(curD);


            if (Frequency == "Weekly")
            {
                string[] dateParts = Period.Split('-');
                year = int.Parse(dateParts[0]);
                month = int.Parse(dateParts[1]);
                day = int.Parse(dateParts[2]);

                //return db.Deliverables.Join(db.DeliverableDetails, d => d.ID, dd => dd.DelID, (d, dd) => new { d, dd }).Where(x => x.d.ID == DelID && x.dd.DateDue >= new DateTime(year, month, day)).Select( x=> new {x.dd.DateDue, x.dd.DateCompleted, x.dd.ID}).First();


            }
            else if (Frequency == "Monthly")
            {
                month = int.Parse(Period);
                day = 1;
                System.Diagnostics.Debug.WriteLine("period = " + Period + " | date = " + year + "-" + month + "-" + day);
            }
            else if (Frequency == "Quarterly")
            {
                day = 1;
                switch (Period)
                {
                    case "Q1":
                        month = 1;
                        break;
                    case "Q2":
                        month = 4;
                        break;
                    case "Q3":
                        month = 7;
                        break;
                    case "Q4":
                        month = 11;
                        break;
                }
            }
            else if (Frequency == "Semi-Annual")
            {
                day = 1;
                switch (Period)
                {
                    case "S1":
                        month = 1;
                        break;
                    case "S2":
                        month = 7;
                        break;
                }
            }
            else if (Frequency == "Annual")
            {
                day = 1;
                month = 1;
            }
            return db.DeliverableDetails.Where(x => x.DelID == DelID && x.DateDue >= new DateTime(year, month, day)).First();
        }


        public List<object> Get(int ID, string DataPull)
        {
            if (DataPull == "Documents")
            {
                return db.DeliverableDocuments.Join(db.Users, dd => dd.UserID, u => u.ID, (dd, u) => new { dd, u }).Where(x => x.dd.DelDetID == ID).Select(x => new { x.dd.ID, x.dd.UserID, x.dd.TimeSubmitted, x.dd.Path, x.dd.Title, x.dd.Type, x.dd.Location, x.u.FirstName, x.u.LastName }).ToList<object>();
            }
            else if (DataPull == "Reviewables")
            {
                return db.ReviewablesByUsers.Where(x => x.UserID == ID).OrderBy(o => o.DateDue).ToList<object>();
            }
            else if (DataPull == "DelDet")
            {
                return db.DeliverableDetails.Join(db.Deliverables, dd => dd.DelID, d => d.ID, (dd, d) => new { dd, d }).Where(x => x.dd.ID == ID).Select(x => new { x.dd.ID, x.dd.DateDue, x.dd.DateCompleted, x.dd.DelID, x.d.UserID, x.d.Name, x.d.Description, x.d.Frequency }).ToList<object>();
            }
            else if (DataPull == "DelStatus")
            {
                return db.DeliverableStatusByUserIDs.Where(x => x.UserID == ID).OrderBy(o => o.Month).ToList<object>();
            }
            else if (DataPull == "DelPastDue")
            {
                return db.PastDueDeliverables.Where(x => x.UserID == ID).OrderByDescending(o => o.DaysPast).ToList<object>();
            }
            else if (DataPull == "DelUpcoming")
            {
                return db.DeliverablesDueInNextThirtyDays.Where(x => x.UserID == ID).OrderByDescending(o => o.DaysPast).ToList<object>();
            }
            else
            {
                return null;
            }
        }


        //public object Get()
        //{
        //    DateTime startRange = DateTime.Now;
        //    DateTime endRange = startRange.AddMonths(1);
        //    return  db.Deliverables.Join(db.DeliverableDetails, d => d.ID, dd => dd.DelID, (d, dd) => new { d, dd }).Where(x => x.d.UserID == 1 && x.dd.DateDue >= startRange && x.dd.DateDue <= endRange && x.dd.DateCompleted == null).Select(x => new { x.d.Name, x.d.Frequency, x.dd.DateDue }).ToList();
        //}

        // POST: Deliverables
        [ResponseType(typeof(DeliverablesViewModel))]
        public async Task<IHttpActionResult> Post(Deliverable del)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.Deliverables.Add(del);
            await db.SaveChangesAsync();

            DeliverableDetail delDet = new DeliverableDetail();
            delDet.DelID = del.ID;
            var newDelDate = del.FirstDueDate.ToString().Split(' ')[0];
            var d = newDelDate.Split('/');
            if (d[0].Length == 1)
            {
                d[0] = '0' + d[0];
            }
            if (d[1].Length == 1)
            {
                d[1] = '0' + d[1];
            }
            newDelDate = d[0] + '/' + d[1] + '/' + d[2];
            System.Diagnostics.Debug.WriteLine(newDelDate);
            var interval = 0;
            if (del.Frequency == "Weekly")
            {
                interval = 1095;
                for (var i = 0; i <= interval; i += 7)
                {
                    delDet.DateDue = DateTime.ParseExact(newDelDate, "MM/dd/yyyy", null).AddDays(i);
                    db.DeliverableDetails.Add(delDet);
                    await db.SaveChangesAsync();
                }

            }
            else if (del.Frequency == "Monthly")
            {
                interval = 36;
                for (var i = 0; i <= interval; i++)
                {
                    delDet.DateDue = DateTime.ParseExact(newDelDate, "MM/dd/yyyy", null).AddMonths(i);
                    db.DeliverableDetails.Add(delDet);
                    await db.SaveChangesAsync();
                }

            }
            else if (del.Frequency == "Quarterly")
            {
                interval = 36;
                for (var i = 0; i <= interval; i += 3)
                {
                    delDet.DateDue = DateTime.ParseExact(newDelDate, "MM/dd/yyyy", null).AddMonths(i);
                    db.DeliverableDetails.Add(delDet);
                    await db.SaveChangesAsync();
                }

            }
            else if (del.Frequency == "Semi-Annual")
            {
                interval = 36;
                for (var i = 0; i <= interval; i += 6)
                {
                    delDet.DateDue = DateTime.ParseExact(newDelDate, "MM/dd/yyyy", null).AddMonths(i);
                    db.DeliverableDetails.Add(delDet);
                    await db.SaveChangesAsync();
                }

            }
            else if (del.Frequency == "Annual")
            {
                interval = 3;
                for (var i = 0; i <= interval; i++)
                {
                    delDet.DateDue = DateTime.ParseExact(newDelDate, "MM/dd/yyyy", null).AddYears(i);
                    db.DeliverableDetails.Add(delDet);
                    await db.SaveChangesAsync();
                }
            }

            var UID = int.Parse(HttpContext.Current.Request.Cookies["authToken"].Value);
            var logDescription = "added a new Deliverable.";
            var h = new Helpers.Helpers();
            h.NewLogEntry(UID, "Deliverables", del.ID, "Added", logDescription);
            return Ok(DeliverablesViewModel.MapFrom(del));
        }


    }
}