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
    public class CommentsController : ApiController
    {
        private DashboardEntities db = new DashboardEntities();


        public List<object> Get(int ID, string Type)
        {
            return db.Comments.Join(db.Users, dc => dc.UserID, u => u.ID, (dc, u) => new { dc, u }).Where(x => x.dc.TypeID == ID && x.dc.Type == Type).Select(x => new { x.dc.ID, x.dc.UserID, x.dc.TimeSubmitted, x.dc.CommentText, x.u.FirstName, x.u.LastName, x.u.Email }).ToList<object>();
        }
        // POST: Deliverable Reviewers
        [ResponseType(typeof(CommentsViewModel))]
        public async Task<IHttpActionResult> Post(Comment com)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            db.Comments.Add(com);
            await db.SaveChangesAsync();

            return Ok(CommentsViewModel.MapFrom(com));
        }

        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put([FromBody]Comment com)
        {
            int ID = com.ID;
            System.Diagnostics.Debug.WriteLine(com.CommentText);

            if (!ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("BadModelState");
                return BadRequest(ModelState);
            }

            if (ID != com.ID)
            {
                System.Diagnostics.Debug.WriteLine("BadReq");
                return BadRequest();
            }

            Comment upCom = (Comment)db.Comments.Where(x => x.ID == ID).First();
            upCom.TimeSubmitted = com.TimeSubmitted;
            upCom.CommentText = com.CommentText;


            db.Comments.Attach(upCom);
            var entry = db.Entry(upCom);
            entry.Property(e => e.TimeSubmitted).IsModified = true;
            entry.Property(e => e.CommentText).IsModified = true;
            // other changed properties
            db.SaveChanges();

            //db.Entry(deldet).State = EntityState.Modified;
            //System.Diagnostics.Debug.WriteLine("Modified");
            try
            {
                System.Diagnostics.Debug.WriteLine("Saved " + ID.ToString() + " | " + com.TimeSubmitted.ToString());
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComExists(ID))
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

        private bool ComExists(long ID)
        {
            return db.Comments.Count(e => e.ID == ID) > 0;
        }

        [ResponseType(typeof(CommentsViewModel))]
        public async Task<IHttpActionResult> Delete(long id)
        {
            Comment c = await db.Comments.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                db.Comments.Remove(c);
                await db.SaveChangesAsync();
                return Ok(CommentsViewModel.MapFrom(c));
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