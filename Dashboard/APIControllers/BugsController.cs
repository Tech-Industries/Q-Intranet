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
using Dashboard.Helpers;
using System.Web;

namespace Dashboard.APIControllers
{
    public class BugsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();


        [ResponseType(typeof(List<object>))]
        public List<object> Get()
        {
            return db.BugsTopLevels.Where(x => x.Status != "Closed").OrderBy(o => o.ID).ToList<object>();
        }

        public List<object> Get(string Status, string Type)
        {
            if (Status == "All" && Type == "All")
            {
                return db.BugsTopLevels.Where(x => x.Status != "Closed").OrderBy(o => o.ID).ToList<object>();
            }
            else if (Status != "All" && Type == "All")
            {
                return db.BugsTopLevels.Where(x => x.Status == Status).OrderBy(o => o.ID).ToList<object>();
            }
            else if (Status == "All" && Type != "All")
            {
                return db.BugsTopLevels.Where(x => x.Type == Type).OrderBy(o => o.ID).ToList<object>();
            }
            else
            {
                return db.BugsTopLevels.Where(x => x.Status == Status && x.Type == Type).OrderBy(o => o.ID).ToList<object>();
            }
        }

        public List<object> Get(int ID, string DataPull)
        {
            if (DataPull == "Comments")
            {
                return db.Comments.Join(db.Users, dc => dc.UserID, u => u.ID, (dc, u) => new { dc, u }).Where(x => x.dc.TypeID == ID && x.dc.Type == "Deliverables").Select(x => new { x.dc.ID, x.dc.UserID, x.dc.TimeSubmitted, x.dc.CommentText, x.u.FirstName, x.u.LastName, x.u.Email }).ToList<object>();
            }
            else if (DataPull == "Bug")
            {
                return db.Bugs.Where(x => x.ID == ID).ToList<object>();
            }
            else
            {
                return null;
            }
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put([FromBody]Bug bug)
        {
            int ID = bug.ID;
            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
                return BadRequest(ModelState);
            }

            if (ID != bug.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
                return BadRequest();
            }

            Bug upBug = (Bug)db.Bugs.Where(x => x.ID == ID).First();
            var logDescription = "";
            if (upBug.Status != bug.Status)
            {
                logDescription = "updated Status from " + upBug.Status + " to " + bug.Status + ".";
                upBug.Status = bug.Status;
            }
            if (upBug.DateClosed != bug.DateClosed)
            {
                upBug.DateClosed = bug.DateClosed;
            }
            if (upBug.AssigneeID != bug.AssigneeID)
            {
                logDescription = "updated AssigneeID from " + upBug.AssigneeID + " to " + bug.AssigneeID + ".";
                upBug.AssigneeID = bug.AssigneeID;
            }




            db.Bugs.Attach(upBug);
            var entry = db.Entry(upBug);
            entry.Property(e => e.Status).IsModified = true;
            entry.Property(e => e.DateClosed).IsModified = true;
            entry.Property(e => e.AssigneeID).IsModified = true;
            // other changed properties
            db.SaveChanges();


            var UID = int.Parse(HttpContext.Current.Request.Cookies["authToken"].Value);
            var h = new Helpers.Helpers();
            h.NewLogEntry(UID, "Bug", bug.ID, "Update", logDescription);
            //db.Entry(deldet).State = EntityState.Modified;
            //System.Diagnostics.Debug.WriteLine("Modified");
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BugExists(ID))
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

        private bool BugExists(long ID)
        {
            return db.Bugs.Count(e => e.ID == ID) > 0;
        }

        [ResponseType(typeof(BugsTopLevelViewModel))]
        public async Task<IHttpActionResult> Post(Bug bug)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.Bugs.Add(bug);
            await db.SaveChangesAsync();
            
            var h = new Helpers.Helpers();
            h.NewLogEntry((int)bug.UserID, "Bug", 0, "Create", "added a new " + bug.Type + ".");
            return Ok(await BugsTopLevelViewModel.MapFromAsync(db.BugsTopLevels.Where(x => x.ID == bug.ID).ToList()));
        }

    }
}