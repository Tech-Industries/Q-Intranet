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
using Newtonsoft.Json.Linq;

namespace Dashboard.APIControllers
{
    public class NotificationsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();


        public List<object> Get(int UserID)
        {
            return db.Notifications.Where(x => x.UserID == UserID && x.UserID != x.CreatorID).OrderByDescending(o => o.DateCreated).Take(3).ToList<object>();
        }

        public List<object> GET(int UserID, bool Viewed)
        {
            if (Viewed == true)
            {
                return db.Notifications.Where(x => x.Viewed == false && x.UserID == UserID && x.UserID != x.CreatorID).OrderByDescending(o => o.DateCreated).ToList<object>();
            }
            else
            {
                var DateViewed = DateTime.Now;
                var flag = db.Notifications.Where(x => x.Viewed == false && x.UserID == UserID && x.UserID != x.CreatorID).ToList();
                flag.ForEach(a =>
               {
                   a.Viewed = true;
                   a.DateViewed = DateViewed;
               });
                db.SaveChanges();
                return new List<object>();
            }
        }
        // POST: Deliverable Reviewers
        public object Post(Notification not)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            not.DateCreated = DateTime.Now;
            if(not.Reference == "Deliverables")
            {
                var DelID = db.DeliverableDetails.Where(x => x.ID == not.ReferenceID).First().DelID;
                var OwnerID = db.Deliverables.Where(x => x.ID == DelID).First().UserID;
                not.UserID = OwnerID;
            }
            var Creator = db.Users.Where(x => x.ID == not.CreatorID).First();
            var Name = Creator.FirstName + " " + Creator.LastName;
            not.Message = Name + " " + not.Message;
            
            db.Notifications.Add(not);
            db.SaveChanges();

            return not;
        }

        [ResponseType(typeof(void))]
        public void Put([FromBody]Notification not)
        {
            int ID = not.ID; ;

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
            }

            if (ID != not.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
            }

            Notification upNot = (Notification)db.Notifications.Where(x => x.ID == ID).First();
            upNot.Viewed = true;
            upNot.DateViewed = DateTime.Now;


            db.Notifications.Attach(upNot);
            var entry = db.Entry(upNot);
            entry.Property(e => e.Viewed).IsModified = true;
            entry.Property(e => e.DateViewed).IsModified = true;
            // other changed properties
            db.SaveChanges();

            //db.Entry(deldet).State = EntityState.Modified;
            //System.Diagnostics.Debug.WriteLine("Modified");
            try
            {
                db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotExists(ID))
                {
                    System.Diagnostics.Debug.WriteLine("Catch If");
                }
                else {
                    System.Diagnostics.Debug.WriteLine("Catch If Else");
                    throw;
                }
            }
        }

        private bool NotExists(long ID)
        {
            return db.Notifications.Count(e => e.ID == ID) > 0;
        }

        [ResponseType(typeof(CommentsViewModel))]
        public object Delete(long id)
        {
            Notification n = db.Notifications.Find(id);
            if (n == null)
            {
                return NotFound();
            }
            else
            {
                db.Notifications.Remove(n);
                db.SaveChanges();
                return n;
            }


        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}